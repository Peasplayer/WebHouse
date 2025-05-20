namespace WebHouse_Client.Logic;

public class EscapeCard : ICard
{
    public int Number { get; }
    public string Room { get; }
    public CardColor Color { get; }
    
    public Components.EscapeCard? Component { get; private set; }

    public EscapeCard(int number, string room, CardColor color)
    {
        Number = number;
        Room = room;
        Color = color;
    }

    public void CreateComponent()
    {
        Component = new Components.EscapeCard(this);
    }
}