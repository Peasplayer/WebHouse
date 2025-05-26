namespace WebHouse_Client.Logic;

public class Room
{
    //Die Namen der Räume
    public enum RoomName
    {
        Hotelzimmer,
        Hafen,
        Stadt,
        Wald,
        Safehouse,
    }
    
    public RoomName RoomType { get; set; }
    //Das Bild des Raumes und der Name des Raumes werden verbunden
    public string Picture => RoomType switch
    {
        RoomName.Hotelzimmer => "Hotel.jpg",
        RoomName.Hafen => "Hafen.jpg",
        RoomName.Stadt => "Stadt.jpg",
        RoomName.Wald => "Wald.jpg",
        RoomName.Safehouse => "Safehouse.jpg",
        _ => "Hotel.jpg"
    };
    //Gibt die Anzahl an Schritten der Röume an
    public int Steps => RoomType switch
    {
        RoomName.Hotelzimmer => 21,
        RoomName.Hafen => 20,
        RoomName.Stadt => 19,
        RoomName.Wald => 18,
        RoomName.Safehouse => 29,
        _ => 0
    };

    //List der ChapterCard für jeden einzelnen Raum getreu zum Originalspiel
    public List<ChapterCard> ChapterCards => RoomType switch
    {
        RoomName.Hotelzimmer => new ()
        {
            new ChapterCard(RoomType, 1, new List<CardColor> { CardColor.Pink, CardColor.Pink }),
            new ChapterCard(RoomType, 1, new List<CardColor> { CardColor.Yellow, CardColor.Yellow }),
            new ChapterCard(RoomType, 1, new List<CardColor> { CardColor.Blue, CardColor.Blue }),
            new ChapterCard(RoomType, 1, new List<CardColor> { CardColor.Red, CardColor.Red }),
            new ChapterCard(RoomType, 2, new List<CardColor> { CardColor.Green, CardColor.Green, CardColor.Green }),
            new ChapterCard(RoomType, 2, new List<CardColor> { CardColor.Yellow, CardColor.Yellow, CardColor.Yellow }),
            new ChapterCard(RoomType, 2, new List<CardColor> { CardColor.Red, CardColor.Blue, CardColor.Pink }),
            new ChapterCard(RoomType, 3, new List<CardColor> { CardColor.Red, CardColor.Red, CardColor.Blue, CardColor.Blue }),
            new ChapterCard(RoomType, 3, new List<CardColor> { CardColor.Pink, CardColor.Pink, CardColor.Green, CardColor.Green }),
            new ChapterCard(RoomType, 4, new List<CardColor> { CardColor.Pink, CardColor.Blue, CardColor.Red, CardColor.Green, CardColor.Yellow })
        },
        RoomName.Hafen => new ()
        {
            new ChapterCard(RoomType, 1, new List<CardColor> { CardColor.Pink, CardColor.Pink }),
            new ChapterCard(RoomType, 1, new List<CardColor> { CardColor.Green, CardColor.Green }),
            new ChapterCard(RoomType, 2, new List<CardColor> { CardColor.Green, CardColor.Red, CardColor.Green }),
            new ChapterCard(RoomType, 2, new List<CardColor> { CardColor.Red, CardColor.Red, CardColor.Red }),
            new ChapterCard(RoomType, 2, new List<CardColor> { CardColor.Blue, CardColor.Blue, CardColor.Blue }),
            new ChapterCard(RoomType, 3, new List<CardColor> { CardColor.Red, CardColor.Red, CardColor.Yellow, CardColor.Yellow }),
            new ChapterCard(RoomType, 3, new List<CardColor> { CardColor.Yellow, CardColor.Yellow, CardColor.Green, CardColor.Green }),
            new ChapterCard(RoomType, 3, new List<CardColor> { CardColor.Pink, CardColor.Pink, CardColor.Yellow, CardColor.Yellow }),
            new ChapterCard(RoomType, 4, new List<CardColor> { CardColor.Blue, CardColor.Blue, CardColor.Blue, CardColor.Pink, CardColor.Pink })
        },
        RoomName.Stadt => new ()
        {
            new ChapterCard(RoomType, 1, new List<CardColor> { CardColor.Yellow, CardColor.Yellow }),
            new ChapterCard(RoomType, 2, new List<CardColor> { CardColor.Pink, CardColor.Pink, CardColor.Pink }),
            new ChapterCard(RoomType, 2, new List<CardColor> { CardColor.Blue, CardColor.Red, CardColor.Yellow }),
            new ChapterCard(RoomType, 3, new List<CardColor> { CardColor.Blue, CardColor.Blue, CardColor.Yellow, CardColor.Yellow }),
            new ChapterCard(RoomType, 3, new List<CardColor> { CardColor.Red, CardColor.Red, CardColor.Blue, CardColor.Blue }),
            new ChapterCard(RoomType, 3, new List<CardColor> { CardColor.Red, CardColor.Red, CardColor.Green, CardColor.Green }),
            new ChapterCard(RoomType, 4, new List<CardColor> { CardColor.Pink, CardColor.Blue, CardColor.Red, CardColor.Green, CardColor.Yellow }),
            new ChapterCard(RoomType, 4, new List<CardColor> { CardColor.Green, CardColor.Green, CardColor.Green, CardColor.Pink, CardColor.Pink })
        },
        RoomName.Wald => new ()
        {
            new ChapterCard(RoomType, 2, new List<CardColor> { CardColor.Green, CardColor.Red, CardColor.Green }),
            new ChapterCard(RoomType, 2, new List<CardColor> { CardColor.Pink, CardColor.Blue, CardColor.Red }),
            new ChapterCard(RoomType, 3, new List<CardColor> { CardColor.Pink, CardColor.Pink, CardColor.Blue, CardColor.Blue }),
            new ChapterCard(RoomType, 3, new List<CardColor> { CardColor.Yellow, CardColor.Yellow, CardColor.Red, CardColor.Red }),
            new ChapterCard(RoomType, 4, new List<CardColor> { CardColor.Pink, CardColor.Blue, CardColor.Red, CardColor.Green, CardColor.Yellow }),
            new ChapterCard(RoomType, 4, new List<CardColor> { CardColor.Yellow, CardColor.Yellow, CardColor.Yellow, CardColor.Green, CardColor.Green }),
            new ChapterCard(RoomType, 5, new List<CardColor> { CardColor.Red, CardColor.Red, CardColor.Blue, CardColor.Blue, CardColor.Pink, CardColor.Pink })
        },
        RoomName.Safehouse => new ()
        {
            new ChapterCard(RoomType, 3, new List<CardColor> { CardColor.Blue, CardColor.Blue, CardColor.Green, CardColor.Green }),
            new ChapterCard(RoomType, 3, new List<CardColor> { CardColor.Yellow, CardColor.Yellow, CardColor.Red, CardColor.Red }),
            new ChapterCard(RoomType, 4, new List<CardColor> { CardColor.Pink, CardColor.Blue, CardColor.Red, CardColor.Green, CardColor.Yellow }),
            new ChapterCard(RoomType, 4, new List<CardColor> { CardColor.Blue, CardColor.Blue, CardColor.Blue, CardColor.Pink, CardColor.Pink }),
            new ChapterCard(RoomType, 5, new List<CardColor> { CardColor.Green, CardColor.Green, CardColor.Green, CardColor.Pink, CardColor.Pink, CardColor.Pink }),
            new ChapterCard(RoomType, 5, new List<CardColor> { CardColor.Yellow, CardColor.Yellow, CardColor.Yellow, CardColor.Red, CardColor.Red, CardColor.Red })
        }
    };

    //Die SpecialCard die jeder Raum hat
    public ChapterCard SpecialCard => RoomType switch
    {
        RoomName.Hotelzimmer => new ChapterCard(RoomType, 3, new List<CardColor> { CardColor.White, CardColor.White, CardColor.White }, true),
        RoomName.Hafen => new ChapterCard(RoomType, 3, new List<CardColor> { CardColor.White, CardColor.White, CardColor.White }, true),
        RoomName.Stadt => new ChapterCard(RoomType, 4, new List<CardColor> { CardColor.White, CardColor.White, CardColor.White, CardColor.White }, true),
        RoomName.Wald => new ChapterCard(RoomType, 4, new List<CardColor> { CardColor.White, CardColor.White, CardColor.White, CardColor.White }, true),
        RoomName.Safehouse => new ChapterCard(RoomType, 5, new List<CardColor> { CardColor.White, CardColor.White, CardColor.White, CardColor.White, CardColor.White }, true),
    };
    
    public Room(RoomName roomType)
    {
        RoomType = roomType;
    }
    
    //Die Felder bei denen der Verfolger um ein Feld bewegt wird wenn der Spieler drauf kommt
    public List<int> OpponentMoveTriggerFields => RoomType switch
    {
        RoomName.Hotelzimmer => new List<int> { 10, 13, 18 },
        RoomName.Hafen => new List<int> { 9, 10, 11 },
        RoomName.Stadt => new List<int> { 9, 11, 13 },
        RoomName.Wald => new List<int> { 13 },
        RoomName.Safehouse => new List<int> { 24, 25 },
        _ => new List<int>()
    };
    
    //Die Felder auf denen der Spieler in jedem Raum startet
    public int StartField => RoomType switch
    {
        RoomName.Hotelzimmer => 9,
        RoomName.Hafen => 8,
        RoomName.Stadt => 7,
        RoomName.Wald => 6,
        RoomName.Safehouse => 17,
        _ => 0
    };
}