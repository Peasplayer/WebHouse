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
    public string Id { get; private set; }
    public string Name { get; private set; }
    
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
        }
    }
}