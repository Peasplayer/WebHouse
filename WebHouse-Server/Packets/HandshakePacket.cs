namespace WebHouse_Server.Packets;

public class HandshakePacket
{
    public string? Id;
    public string? Name;

    public HandshakePacket(string? id, string? name)
    {
        Id = id;
        Name = name;
    }
}