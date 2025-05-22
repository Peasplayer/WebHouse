namespace WebHouse_Client.Networking.Packets;

public enum PacketDataType : uint
{
    Handshake,
    Disconnect,
    SyncLobby,
    StartGame,
    StopGame,
    RequestEscapeCard,
    DrawEscapeCard,
    PlaceEscapeCard,
    DiscardEscapeCard,
    RequestChapterCard,
    DrawChapterCard,
    PlaceChapterCard,
    DiscardChapterCard,
    SetChapterCardsEmpty,
    MovePlayer,
    MoveOpponent,
    SwitchTurn
}