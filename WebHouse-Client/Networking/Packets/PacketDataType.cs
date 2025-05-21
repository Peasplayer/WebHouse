namespace WebHouse_Client.Networking.Packets;

public enum PacketDataType : uint
{
    Handshake,
    Disconnect,
    SyncLobby,
    StartGame,
    RequestEscapeCard,
    DrawEscapeCard,
    PlaceEscapeCard,
    RequestChapterCard,
    DrawChapterCard,
    PlaceChapterCard,
    MovePlayer,
    MoveOpponent,
    SwitchTurn
}