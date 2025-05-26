namespace WebHouse_Server.Packets;

//Typen von Packets die der Server an den Client senden darf.
public enum PacketDataType : uint
{
    Handshake,
    Disconnect,
    SyncLobby,
    StartGame,
    StopGame
}