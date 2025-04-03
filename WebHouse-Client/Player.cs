namespace WebHouse_Client;

public class Player : character
{
    /*public int Zahl { get; set; } //FALSCHER ORT UND NOCH NICHT RICHTIG
    public string Room { get; set; }

    public Card(int Zahl, string Room)
    {
        Zahl = zahl;
        Room = room;
    }*/

    public List<string> Inventory { get; private set; } = new List<string>();

    public Player(string name, int startX, int startY) : base(name, startX, startY)
    {
    }

    public void PickUpItem(string item)
    {
        Inventory.Add(item);
    }

    public override void Move(int deltaX, int deltaY)
    {
        Console.WriteLine($"{Name} bewegt sich zu ({X + deltaX}, {Y + deltaY})");
        base.Move(deltaX, deltaY);
    }
}
