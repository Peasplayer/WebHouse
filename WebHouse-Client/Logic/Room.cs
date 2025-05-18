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
        RoomName.SafeHouse => new List<int> { 9, 19 },
        _ => new List<int>()
    };
    
    public int StartField => RoomType switch
    {
        RoomName.HotelZimmer => 9,
        RoomName.Hafen => 8,
        RoomName.Stadt => 7,
        RoomName.Wald => 6,
        RoomName.SafeHouse => 0,
        _ => 9
    };


}