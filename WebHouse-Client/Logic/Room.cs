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
        RoomName.Stadt => 15,
        RoomName.Wald => 20,
        RoomName.SafeHouse => 25,
        _ => 0
    };

    public Room(RoomName roomType)
    {
        RoomType = roomType;
    }
}