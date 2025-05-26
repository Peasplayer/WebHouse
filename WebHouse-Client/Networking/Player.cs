namespace WebHouse_Client.Networking;

//Information die der Spieler dem Server übermittelt
public record struct Player
{
    public string Id;
    public string Name;
    public bool IsHost;
    public bool IsTurn;
}