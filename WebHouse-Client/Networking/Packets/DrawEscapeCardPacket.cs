using WebHouse_Client.Logic;

namespace WebHouse_Client.Networking.Packets;

public class DrawEscapeCardPacket
{
    public EscapeCard.EscapeCardType Type { get; }
    public int Number { get; }
    public Room.RoomName Room { get; }
    public CardColor Color { get; }

    public DrawEscapeCardPacket(EscapeCard.EscapeCardType type, int number, Room.RoomName room, CardColor color)
    {
        Type = type;
        Number = number;
        Room = room;
        Color = color;
    }
}