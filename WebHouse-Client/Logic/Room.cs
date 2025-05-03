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
    public string Picture { get; set; }

    public Room(RoomName roomType, string picture)
    {
        RoomType = roomType;
        Picture = picture;
    }
}