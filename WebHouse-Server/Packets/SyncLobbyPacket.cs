namespace WebHouse_Server.Packets;

public class SyncLobbyPacket
{
    public List<Player> Players; //Liste aller Spieler im Lobby

    public SyncLobbyPacket(List<Player> players)
    {
        Players = players;
    }

    //Information die der Spieler dem Server übermittelt
    public struct Player
    {
        public string Id;
        public string Name;
        public bool IsHost;
    }
}