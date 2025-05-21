namespace WebHouse_Server.Packets;

public enum PacketDataType : uint
{
    Handshake,
    Disconnect,
    SyncLobby,
    StartGame,
}