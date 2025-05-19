namespace WebHouse_Server.Packets;

public class SyncLobbyPacket
{
    public List<Player> Players;

    public SyncLobbyPacket(List<Player> players)
    {
        Players = players;
    }

    public struct Player
    {
        public string Id;
        public string Name;
        public bool IsHost;
    }
}