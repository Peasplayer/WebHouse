using Fleck;
using Newtonsoft.Json;
using WebHouse_Server.Packets;

namespace WebHouse_Server;

public class NetworkManager
{
    // Globale Instanz
    public static NetworkManager Instance;
    
    public WebSocketServer Server { get; private set; }
    public Dictionary<string, ClientData> Clients { get; }

    public NetworkManager()
    {
        Instance = this;
        Clients = new Dictionary<string, ClientData>();
    }

    // Startet den Server und richtet alle Event-Listener ein
    public void StartWebsocket(int port)
    {
        Server = new WebSocketServer("ws://0.0.0.0:" + port);
        Server.Start(clientConnection =>
        {
            clientConnection.OnError = error =>
            {
                FleckLog.Error(string.Format("Error with {0}: {1}", clientConnection.ConnectionInfo.Id, error.Message));
                OnDisconnect(clientConnection);
            };
            clientConnection.OnOpen = () =>
            {
                OnConnect(clientConnection);
            };
            clientConnection.OnClose = () =>
            {
                OnDisconnect(clientConnection);
            };
            clientConnection.OnMessage = message =>
            {
                OnMessage(clientConnection, message);
            };
        });
    }
    
    // Connection-Listener
    private void OnConnect(IWebSocketConnection connection)
    {
        FleckLog.Info("Connect: " + connection.ConnectionInfo.Id);
        Clients.Add(connection.ConnectionInfo.Id.ToString(), new ClientData(connection, isHost: Clients.Count == 0));
    }

    // Disconnection-Listener
    private void OnDisconnect(IWebSocketConnection connection)
    {
        var client = Clients[connection.ConnectionInfo.Id.ToString()];
        FleckLog.Info("Disconnect: " + client.Id);
        Clients.Remove(client.Id);
        if (client.IsHost && Clients.Count > 0)
        {
            Clients.First().Value.IsHost = true;
        }
        
        this.SendPacket(new Packet(new SyncLobbyPacket(Clients.Values.ToList().ConvertAll(c => 
            new SyncLobbyPacket.Player(){Id = c.Id, Name = c.Name, IsHost = c.IsHost})), PacketDataType.SyncLobby, "server", "all"));;
    }

    // Message-Listener
    private void OnMessage(IWebSocketConnection connection, string message)
    {
        FleckLog.Info("Message from " + connection.ConnectionInfo.Id + ": " + message);

        var packet = JsonConvert.DeserializeObject<Packet>(message);
        if (packet == null)
        {
            FleckLog.Warn("Malformed packet received!");
            return;
        }
        
        Task.Run(() => HandlePacket(packet, connection));
    }

    // Verarbeitet die empfangenen Packets je nach Typ
    private void HandlePacket(Packet packet, IWebSocketConnection connection)
    {
        switch (packet.DataType)
        {
            case PacketDataType.Handshake:
            {
                var handshake = JsonConvert.DeserializeObject<HandshakePacket>(packet.Data);
                if (handshake == null)
                {
                    FleckLog.Warn("Malformed packet received!");
                    break;
                }
                
                FleckLog.Info($"Received Handshake from {packet.Sender} with name {handshake.Name}");

                // Falls der Name schon vergeben ist, wird eine Zahl dran gehängt
                var name = handshake.Name;
                var iteration = 0;
                while (Clients.Values.ToList().Find(c => c.Name == name) != null)
                {
                    iteration++;
                    name = handshake.Name + " (" + iteration + ")";
                }
                
                Clients[connection.ConnectionInfo.Id.ToString()].Name = name;
                
                this.SendPacket(new Packet(new HandshakePacket(connection.ConnectionInfo.Id.ToString(), name), PacketDataType.Handshake, "server", connection.ConnectionInfo.Id.ToString()));
                
                this.SendPacket(new Packet(new SyncLobbyPacket(Clients.Values.ToList().ConvertAll(c => 
                    new SyncLobbyPacket.Player(){Id = c.Id, Name = c.Name, IsHost = c.IsHost})), PacketDataType.SyncLobby, "server", "all"));;
                break;
            }
            default:
            {
                SendPacket(packet);
                break;
            }
        }
    }

    // Sendet Packets je nach Empfänger an die Clients
    public void SendPacket(Packet packet)
    {
        if (packet.Receivers.Contains("all"))
        {
            foreach (var (_, clientData) in Clients)
            {
                if (clientData.Connection.IsAvailable)
                {
                    clientData.Connection.Send(JsonConvert.SerializeObject(packet));
                }
                else
                {
                    OnDisconnect(clientData.Connection);
                }
            }
        }
        else
        {
            foreach (var receiver in packet.Receivers)
            {
                if (!Clients.TryGetValue(receiver, out ClientData? receiverClient))
                    continue;
                
                if (!receiverClient.Connection.IsAvailable)
                {
                    OnDisconnect(receiverClient.Connection);
                    continue;
                }
                
                receiverClient.Connection.Send(JsonConvert.SerializeObject(packet));
            }
        }
    }
}