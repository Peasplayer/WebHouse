using Fleck;

namespace WebHouse_Server;

public class ClientData
{
    public IWebSocketConnection Connection { get; }
    public string Id => Connection.ConnectionInfo.Id.ToString();
    public string? Name;

    public ClientData(IWebSocketConnection connection, string? name = null)
    {
        Connection = connection;
        Name = name;
    }
}