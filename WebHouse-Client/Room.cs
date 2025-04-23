namespace WebHouse_Client;

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
    public int[,] Fields { get; set; }
    public string Picture { get; set; }

    public Room(RoomName roomType, int[,] fields, string picture)
    {
        RoomType = roomType;
        Fields = fields;
        Picture = picture;
    }
}