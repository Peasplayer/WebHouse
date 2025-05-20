namespace WebHouse_Client.Networking;

public record struct Player
{
    public string Id;
    public string Name;
    public bool IsHost;
    public bool IsTurn;

    public Player(string id, string name, bool isHost, bool isTurn)
    {
        Id = id;
        Name = name;
        IsHost = isHost;
        IsTurn = isTurn;
    }
}