using System.Net.WebSockets;
using Newtonsoft.Json;
using WebHouse_Client.Logic;
using WebHouse_Client.Networking.Packets;
using Websocket.Client;

namespace WebHouse_Client.Networking;

public class NetworkManager
{
    // Globale Instanz
    public static NetworkManager Instance;

    public WebsocketClient Client { get; private set; }
    public string Id { get; private set; }
    public string Name { get; private set; }
    public List<Player> Players { get; private set; } = new ();
    public Player LocalPlayer => Players.First(p => p.Id == this.Id);
    
    public NetworkManager()
    {
        Instance = this;
    }

    // Verbindet den Client mit dem Server und führt den Handshake durch
    public void Connect(string url, string name)
    {
        Client = new WebsocketClient(new Uri(url));
        
        // Client wird konfiguriert
        Client.IsReconnectionEnabled = false;
        Client.ErrorReconnectTimeout = null;
        Client.ReconnectTimeout = TimeSpan.FromSeconds(5);
        
        // Nachrichten empfangen und an den Listener weiterleiten
        Client.MessageReceived.Subscribe(msg =>
        {
            if (msg.MessageType == WebSocketMessageType.Text && msg.Text != null)
            {
                Console.WriteLine("MESSAGE:" + msg.Text);
                // Nachricht wird in Packet umgewandelt
                var packet = JsonConvert.DeserializeObject<Packet>(msg.Text);
                if (packet == null)
                {
                    Console.WriteLine("Received malformed packet!");
                    return;
                }
                
                // Packet wird in Task verarbeitet um den Empfänger-Thread nicht zu blockieren
                Task.Run(() => HandlePacket(packet));
            }
        });
        
        Client.DisconnectionHappened.Subscribe(info =>
        {
            Console.WriteLine(info.Type + ": " + info.CloseStatus);
        });
        
        // Wartet bis die Verbindung hergestellt wurde oder fehlschlägt
        Client.StartOrFail().Wait();
        
        // Sendet den Handshake mit dem gewünschten Namen
        this.SendPacket(new Packet(new HandshakePacket(null, name), PacketDataType.Handshake, "no-id(ea)", "server"));
    }

    // Sendet Packets an den Server
    public void SendPacket(Packet packet)
    {
        this.Client.Send(JsonConvert.SerializeObject(packet));
    }

    // Verarbeitet empfangene Packets je nach Typ
    private void HandlePacket(Packet packet)
    {
        switch (packet.DataType)
        {
            case PacketDataType.Handshake:
            {
                var handshake = JsonConvert.DeserializeObject<HandshakePacket>(packet.Data);
                if (handshake == null || handshake.Id == null || handshake.Name == null)
                {
                    Console.WriteLine("Received malformed packet!");
                    return;
                }

                this.Name = handshake.Name;
                this.Id = handshake.Id;
                Console.WriteLine($"Successful handshake: {this.Name} ({this.Id})");
                break;
            }
            case PacketDataType.Disconnect:
            {
                Client.Stop(WebSocketCloseStatus.NormalClosure, "Server requested closure");

                MessageBox.Show("Die Verbindung wurde geschlossen!\nGrund: " + packet.Data, "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);

                Task.Run(() =>
                {
                    Task.Delay(1000).Wait();
                    if (Lobby.Instance != null)
                    {
                        Lobby.Instance.BeginInvoke(() =>
                        {
                            Lobby.Instance.Close();
                            Lobby.Instance = null;
                            var form = new Menu();
                            form.Show();
                        });
                    }
                });
                break;
            }
            case PacketDataType.SyncLobby:
            {
                var syncLobby = JsonConvert.DeserializeObject<SyncLobbyPacket>(packet.Data);
                if (syncLobby == null)
                {
                    Console.WriteLine("Received malformed packet!");
                    return;
                }
                
                this.Players = syncLobby.Players;
                Lobby.Instance?.BeginInvoke(() =>
                {
                    Lobby.Instance?.RefreshPlayerList();
                });
                break;
            }
            case PacketDataType.StartGame:
            {
                Lobby.Instance?.BeginInvoke(() =>
                {
                    GameForm gameForm = new GameForm();
                    gameForm.Show();
                    Lobby.Instance.ClosedByGame = true;
                    Lobby.Instance.Close();
                    Lobby.Instance = null;
                });
                break;
            }
            case PacketDataType.RequestEscapeCard:
            {
                if (!LocalPlayer.IsHost)
                    return;

                Rpc.DrawEscapeCard(packet.Sender);
                break;
            }
            case PacketDataType.DrawEscapeCard:
            {
                var drawEscapeCard = JsonConvert.DeserializeObject<DrawEscapeCardPacket>(packet.Data);
                if (drawEscapeCard == null)
                {
                    Console.WriteLine("Received malformed packet!");
                    return;
                }

                var escapeCard = new EscapeCard(drawEscapeCard.Type, drawEscapeCard.Number, drawEscapeCard.Room, drawEscapeCard.Color);
                GameLogic.DrawEscapeCard(escapeCard);
                break;
            }
            case PacketDataType.PlaceEscapeCard:
            {
                var placeEscapeCard = JsonConvert.DeserializeObject<PlaceEscapeCardPacket>(packet.Data);
                if (placeEscapeCard == null)
                {
                    Console.WriteLine("Received malformed packet!");
                    return;
                }
        
                var card = GameLogic.PlacedChapterCards.Find(c => ((Components.ChapterCard)c.Component).Pile.Index == placeEscapeCard.Pile);
                if (card != null)
                {
                    GameLogic.PlaceEscapeCard(new EscapeCard(EscapeCard.EscapeCardType.Normal, placeEscapeCard.Number, placeEscapeCard.Room, placeEscapeCard.Color), card);
                }
                break;
            }
            case PacketDataType.RequestChapterCard:
            {
                if (!LocalPlayer.IsHost)
                    return;
                
                Rpc.DrawChapterCard(packet.Sender);
                break;
            }
            case PacketDataType.DrawChapterCard:
            {
                var drawChapterCard = JsonConvert.DeserializeObject<DrawChapterCardPacket>(packet.Data);
                if (drawChapterCard == null)
                {
                    Console.WriteLine("Received malformed packet!");
                    return;
                }

                var chapterCard = new ChapterCard(drawChapterCard.Chapter, drawChapterCard.Steps,
                    drawChapterCard.Requirements);
                GameLogic.DrawChapterCard(chapterCard);
                break;
            }
            case PacketDataType.PlaceChapterCard:
            {
                var placeChapterCard = JsonConvert.DeserializeObject<PlaceChapterCardPacket>(packet.Data);
                if (placeChapterCard == null)
                {
                    Console.WriteLine("Received malformed packet!");
                    return;
                }
        
                var card = new ChapterCard(placeChapterCard.ChapterCard.Chapter, placeChapterCard.ChapterCard.Steps, placeChapterCard.ChapterCard.Requirements);
                GameLogic.PlaceChapterCard(card, placeChapterCard.Pile);
                break;
            }
            case PacketDataType.MovePlayer:
            {
                var steps = JsonConvert.DeserializeObject<int?>(packet.Data);
                if (steps == null)
                {
                    Console.WriteLine("Received malformed packet!");
                    return;
                }
                
                GameLogic.MovePlayer(steps.Value);
                break;
            }
            case PacketDataType.MoveOpponent:
            {
                var steps = JsonConvert.DeserializeObject<int?>(packet.Data);
                if (steps == null)
                {
                    Console.WriteLine("Received malformed packet!");
                    return;
                }
                
                GameLogic.MoveOpponent(steps.Value);
                break;
            }
        }
    }

    public static class Rpc
    {
        public static void StartGame()
        {
            Instance.SendPacket(new Packet(null, PacketDataType.StartGame, Instance.Id, "all"));
        }

        public static void RequestEscapeCard()
        {
            Instance.SendPacket(new Packet(null, PacketDataType.RequestEscapeCard, Instance.Id, "all"));
        }

        public static void DrawEscapeCard(string id)
        {
            var escapeCard = GameLogic.CurrentEscapeCards[0];
            GameLogic.CurrentEscapeCards.Remove(escapeCard);
            
            Instance.SendPacket(new Packet(new DrawEscapeCardPacket(escapeCard.Type, escapeCard.Number, escapeCard.Room, escapeCard.Color), PacketDataType.DrawEscapeCard, Instance.Id, id));
        }

        public static void PlaceEscapeCard(EscapeCard card, int pile)
        {
            GameLogic.Inventory.Remove(card);
            card.Component?.Panel.Dispose();
            Instance.SendPacket(new Packet(new PlaceEscapeCardPacket(card.Number, card.Room, card.Color, pile), PacketDataType.PlaceEscapeCard, Instance.Id, "all"));
        }
        
        public static void RequestChapterCard()
        {
            Instance.SendPacket(new Packet(null, PacketDataType.RequestChapterCard, Instance.Id, "all"));
        }

        public static void DrawChapterCard(string id)
        {
            var card = GameLogic.CurrentChapterCards.First();
            GameLogic.CurrentChapterCards.Remove(card);
            
            Instance.SendPacket(new Packet(new DrawChapterCardPacket(card.Chapter, card.Steps, card.Requirements), PacketDataType.DrawChapterCard, Instance.Id, id));
        }

        public static void PlaceChapterCard(ChapterCard card, int pile)
        {
            card.Component.Panel.Dispose();
            GameLogic.Inventory.Remove(card);
            Instance.SendPacket(new Packet(new PlaceChapterCardPacket(new DrawChapterCardPacket(card.Chapter, card.Steps, card.Requirements), pile), PacketDataType.PlaceChapterCard, Instance.Id, "all"));
        }

        public static void MovePlayer(int steps)
        {
            Instance.SendPacket(new Packet(steps, PacketDataType.MovePlayer, Instance.Id, "all"));
        }

        public static void MoveOpponent(int steps)
        {
            Instance.SendPacket(new Packet(steps, PacketDataType.MoveOpponent, Instance.Id, "all"));
        }
    }
}