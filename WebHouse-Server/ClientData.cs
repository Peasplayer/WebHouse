using Fleck;

namespace WebHouse_Server;

public class ClientData
{
    public IWebSocketConnection Connection { get; }
    public string Id => Connection.ConnectionInfo.Id.ToString();
    public string? Name;
    public bool IsHost;

    public ClientData(IWebSocketConnection connection, string? name = null, bool isHost = false)
    {
        Connection = connection;
        Name = name;
        IsHost = isHost;
    }
}