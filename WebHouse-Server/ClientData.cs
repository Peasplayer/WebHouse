using Fleck;

namespace WebHouse_Server;

public class ClientData
{
    public IWebSocketConnection Connection { get; } //Websocket Verbindung des Clients
    public string Id => Connection.ConnectionInfo.Id.ToString(); //Eindeutige ID des Clients
    public string? Name; //Der Name des Clients
    public bool IsHost; //Gibt an ob der Client auch der Host ist

    public ClientData(IWebSocketConnection connection, string? name = null, bool isHost = false)
    {
        Connection = connection;
        Name = name;
        IsHost = isHost;
    }
}