namespace WebHouse_Client.Logic;

public class Room
{
    public enum RoomName
    {
        HotelZimmer,
        Hafen,
        Stadt,
        Wald,
        SafeHouse,
    }
    
    public RoomName RoomType { get; set; }
    public string Picture => RoomType switch
    {
        RoomName.HotelZimmer => "Hotel.jpg",
        RoomName.Hafen => "Hafen.jpg",
        RoomName.Stadt => "Stadt.jpg",
        RoomName.Wald => "Wald.jpg",
        RoomName.SafeHouse => "Safehouse.jpg",
        _ => "Hotel.jpg"
    };
    public int Steps => RoomType switch
    {
        RoomName.HotelZimmer => 21,
        RoomName.Hafen => 20,
        RoomName.Stadt => 19,
        RoomName.Wald => 18,
        RoomName.SafeHouse => 29,
        _ => 0
    };

    public List<ChapterCard> ChapterCards => RoomType switch
    {
        RoomName.HotelZimmer => new ()
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
        RoomName.SafeHouse => new ()
        {
            new ChapterCard(RoomType, 3, new List<CardColor> { CardColor.Blue, CardColor.Blue, CardColor.Green, CardColor.Green }),
            new ChapterCard(RoomType, 3, new List<CardColor> { CardColor.Yellow, CardColor.Yellow, CardColor.Red, CardColor.Red }),
            new ChapterCard(RoomType, 4, new List<CardColor> { CardColor.Pink, CardColor.Blue, CardColor.Red, CardColor.Green, CardColor.Yellow }),
            new ChapterCard(RoomType, 4, new List<CardColor> { CardColor.Blue, CardColor.Blue, CardColor.Blue, CardColor.Pink, CardColor.Pink }),
            new ChapterCard(RoomType, 5, new List<CardColor> { CardColor.Green, CardColor.Green, CardColor.Green, CardColor.Pink, CardColor.Pink, CardColor.Pink }),
            new ChapterCard(RoomType, 5, new List<CardColor> { CardColor.Yellow, CardColor.Yellow, CardColor.Yellow, CardColor.Red, CardColor.Red, CardColor.Red })
        }
    };

    public Room(RoomName roomType)
    {
        RoomType = roomType;
    }
    
    public List<int> OpponentMoveTriggerFields => RoomType switch
    {
        RoomName.HotelZimmer => new List<int> { 10, 13, 18 },
        RoomName.Hafen => new List<int> { 9, 10, 11 },
        RoomName.Stadt => new List<int> { 9, 11, 13 },
        RoomName.Wald => new List<int> { 13 },
        RoomName.SafeHouse => new List<int> { 24, 25 },
        _ => new List<int>()
    };
    
    public int StartField => RoomType switch
    {
        RoomName.HotelZimmer => 9,
        RoomName.Hafen => 8,
        RoomName.Stadt => 7,
        RoomName.Wald => 6,
        RoomName.SafeHouse => 17,
        _ => 0
    };
}