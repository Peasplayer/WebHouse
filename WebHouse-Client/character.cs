namespace WebHouse_Client;

public class character
{
    public int X { get;  set; }
    public int Y { get;  set; }
    public string Name { get; set; }

    public character(string name, int startX, int startY)
    {
        Name = name;
        X = startX;
        Y = startY;
    }

    public virtual void Move(int deltaX, int deltaY)
    {
        X += deltaX;
        Y += deltaY;
    }
}