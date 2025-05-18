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
            CardColor.Red => Color.FromArgb(255, 228, 3, 9),
            CardColor.Green => Color.FromArgb(255, 114,177, 29),
            CardColor.Blue => Color.FromArgb(255, 0, 159, 220),
            CardColor.Yellow => Color.FromArgb(255, 255, 243, 0),
            CardColor.Pink => Color.FromArgb(255, 243, 77, 146),
            _ => throw new ArgumentOutOfRangeException(nameof(cardColor), cardColor, null)
        };
    }
}