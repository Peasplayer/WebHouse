using System.Net.WebSockets;
using Newtonsoft.Json;
using WebHouse_Client.Networking.Packets;
using Websocket.Client;

namespace WebHouse_Client.Networking;

public class NetworkManager
{
    // Globale Instanz
    public static NetworkManager Instance;

    public WebsocketClient Client { get; private set; }
    public RPC Rpc { get; private set; }
    public string Id { get; private set; }
    public string Name { get; private set; }
    public List<Player> Players { get; private set; } = new ();
    public Player LocalPlayer => Players.First(p => p.Id == this.Id);
    
    public NetworkManager()
    {
        Instance = this;
        Rpc = new RPC(this);
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
                            var form = new Form1();
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
                    Lobby.Instance.Close();
                    Lobby.Instance = null;
                });
                break;
            }
        }
    }

    public class RPC
    {
        private NetworkManager _networkManager;
        
        public RPC(NetworkManager networkManager)
        {
            _networkManager = networkManager;
        }

        public void StartGame()
        {
            _networkManager.SendPacket(new Packet(null, PacketDataType.StartGame, _networkManager.Id, "all"));
        }
    }
}