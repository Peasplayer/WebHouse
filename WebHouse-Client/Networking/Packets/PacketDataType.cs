namespace WebHouse_Client.Networking.Packets;

public enum PacketDataType : uint
{
    Handshake,
    SyncLobby,
    StartGame,
    RequestEscapeCard,
    DrawEscapeCard,
    PlaceEscapeCard,
    RequestChapterCard,
    DrawChapterCard,
    PlaceChapterCard,
    MovePlayer,
    MoveOpponent
}