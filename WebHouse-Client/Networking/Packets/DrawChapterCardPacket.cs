using WebHouse_Client.Logic;

namespace WebHouse_Client.Networking.Packets;

public class DrawChapterCardPacket
{
    public Room.RoomName Chapter { get; }
    public int Steps { get; }
    public List<CardColor> Requirements { get; }
    public int Counter { get; }

    public DrawChapterCardPacket(Room.RoomName chapter, int steps, List<CardColor> requirements)
    {
        Chapter = chapter;
        Steps = steps;
        Requirements = requirements;
        Counter = 0;
    }
}