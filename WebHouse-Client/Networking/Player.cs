namespace WebHouse_Client.Networking;

public record struct Player
{
    public string Id;
    public string Name;
    public bool IsHost;
    public bool IsTurn;
}