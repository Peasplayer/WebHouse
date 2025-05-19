namespace WebHouse_Client.Networking.Packets;

public class SyncLobbyPacket
{
    public List<Player> Players;

    public SyncLobbyPacket(List<Player> players)
    {
        Players = players;
    }
}