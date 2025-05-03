namespace WebHouse_Client.Logic;

public class EscapeCard
{
    public int Number { get; }
    public string Room { get; }
    public CardColor Color { get; }
    
    public Components.EscapeCard Component { get; }

    public EscapeCard(int number, string room, CardColor color)
    {
        Number = number;
        Room = room;
        Color = color;
        
        Component = new Components.EscapeCard(this);
    }
}