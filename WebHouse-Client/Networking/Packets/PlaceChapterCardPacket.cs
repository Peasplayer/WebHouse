namespace WebHouse_Client.Networking.Packets;

public class PlaceChapterCardPacket
{
    public DrawChapterCardPacket ChapterCard;
    public int Pile;

    public PlaceChapterCardPacket(DrawChapterCardPacket chapterCard, int pile)
    {
        ChapterCard = chapterCard;
        Pile = pile;
    }
}