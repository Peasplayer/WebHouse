namespace WebHouse_Client.Logic;

public enum CardColor
{
    Red,
    Green,
    Blue,
    Yellow,
    Pink
}

public static class CardColorConverter
{
    public static Color GetColor(this CardColor cardColor)
    {
        return cardColor switch
        {
            CardColor.Red => Color.Red,
            CardColor.Green => Color.Green,
            CardColor.Blue => Color.Blue,
            CardColor.Yellow => Color.Yellow,
            CardColor.Pink => Color.Magenta,
            _ => throw new ArgumentOutOfRangeException(nameof(cardColor), cardColor, null)
        };
    }
}