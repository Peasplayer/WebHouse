namespace WebHouse_Server.Packets;

//Handshake zwischen Client und Server mit der ID und dem Namen des Clients
public class HandshakePacket
{
    public string? Id; //ID des Clients der sich verbindet
    public string? Name; //Name des Clients der sich verbindet

    public HandshakePacket(string? id, string? name)
    {
        Id = id;
        Name = name;
    }
}