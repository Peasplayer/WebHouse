using WebHouse_Client.Logic;

namespace WebHouse_Client.Networking.Packets;

public class PlaceEscapeCardPacket
{
    public int Number { get; }
    public Room.RoomName Room { get; }
    public CardColor Color { get; }
    public int Pile { get; }

    public PlaceEscapeCardPacket(int number, Room.RoomName room, CardColor color, int pile)
    {
        Number = number;
        Room = room;
        Color = color;
        Pile = pile;
    }
}