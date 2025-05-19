namespace WebHouse_Client.Logic;

public class ChapterCard : ICard
{
    public Room.RoomName Chapter { get; }
    public int Steps { get; }
    public List<CardColor> Requirements { get; }
    public int Counter;
    
    public Components.ChapterCard Component { get; }

    public ChapterCard(Room.RoomName chapter, int steps, List<CardColor> requirements)
    {
        Chapter = chapter;
        Steps = steps;
        Requirements = requirements;
        
        Component = new Components.ChapterCard(this);
    }
    
    public bool DoesEscapeCardMatch(EscapeCard escapeCard)
    {
        return Requirements.Contains(escapeCard.Color) && escapeCard.Number >= Counter;
    }
    
    public void AddEscapeCard(EscapeCard escapeCard)
    {
        if (!DoesEscapeCardMatch(escapeCard))
            return;
        
        Counter = escapeCard.Number;
        Requirements.Remove(escapeCard.Color);

        if (Requirements.Count == 0)
        {
            Component.Pile.Panel.Enabled = true;
            Component.Pile.Panel.Visible = true;
            Component.Panel.Dispose();
            GameLogic.PlacedChapterCards.Remove(this);
            GameLogic.MovePlayer(Steps);
        }
    }
}