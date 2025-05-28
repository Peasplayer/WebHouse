using WebHouse_Client.Components;

namespace WebHouse_Client.Logic;

public class EscapeCard : ILogicCard
{
    public EscapeCardType Type { get; } //Der Typ der EscapeCard
    public int Number { get; } //Die Nummer der EscapeCard
    public Room.RoomName Room { get; } //Der Raumname der EscapeCard,
    public CardColor Color { get; } //Die Farbe der EscapeCard
    
    public IComponentCard? Component { get; private set; }

    public EscapeCard(EscapeCardType type, int number, Room.RoomName room = default, CardColor color = default)
    {
        Type = type;
        Number = number;
        Room = room;
        Color = color;
    }

    //Erstellt das Panel das die EscapeCard darstellt
    public void CreateComponent()
    {
        Component = new Components.EscapeCard(this);
    }
    //Die Typen die eine EscapeCard sein kann
    public enum EscapeCardType
    {
        Normal, //Normale EscapeCard
        OpponentSteps, //EscapeCard die es den Verfolger erlaubt Schritte zu machen
        OpponentCards, //EscapeCard die es dem Verfolger erlaubt zu eine Feste menge anschritten zu machen plus die Anzahl an offenen ChapterCards
    }
}