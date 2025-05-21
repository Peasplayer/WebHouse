using WebHouse_Client.Components;

namespace WebHouse_Client.Logic;

public class EscapeCard : ILogicCard
{
    public EscapeCardType Type { get; }
    public int Number { get; }
    public Room.RoomName Room { get; }
    public CardColor Color { get; }
    
    public IComponentCard? Component { get; private set; }

    public EscapeCard(EscapeCardType type, int number)
    {
        Type = type;
        Number = number;
        Room = Logic.Room.RoomName.HotelZimmer;
    }
    
    public EscapeCard(int number, Room.RoomName room, CardColor color)
    {
        Number = number;
        Room = room;
        Color = color;
    }

    public void CreateComponent()
    {
        Component = new Components.EscapeCard(this);
    }

    public enum EscapeCardType
    {
        Normal,
        OpponentSteps,
        OpponentCards,
    }
}