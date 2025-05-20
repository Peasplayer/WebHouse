using WebHouse_Client.Components;

namespace WebHouse_Client.Logic;

public class ChapterCard : ILogicCard
{
    public Room.RoomName Chapter { get; }
    public int Steps { get; }
    public List<CardColor> Requirements { get; }
    public int Counter;
    public List<EscapeCard> PlacedCards { get; } = new();
    
    public IComponentCard? Component { get; private set; }

    public ChapterCard(Room.RoomName chapter, int steps, List<CardColor> requirements)
    {
        Chapter = chapter;
        Steps = steps;
        Requirements = requirements;
    }

    public void CreateComponent()
    {
        Component = new Components.ChapterCard(this);
    }
    
    public bool DoesEscapeCardMatch(EscapeCard escapeCard)
    {
        return Requirements.Contains(escapeCard.Color) && escapeCard.Number >= Counter;
    }
    
    public void AddEscapeCard(EscapeCard escapeCard)
    {
        if (Component == null)
            return;
        if (!DoesEscapeCardMatch(escapeCard))
            return;
        
        Counter = escapeCard.Number;
        Requirements.Remove(escapeCard.Color);
        PlacedCards.Add(escapeCard);

        if (Requirements.Count == 0)
        {
            ((Components.ChapterCard)Component).Pile.Panel.Enabled = true;
            ((Components.ChapterCard)Component).Pile.Panel.Visible = true;
            Component.Panel.Dispose();
            GameLogic.PlacedChapterCards.Remove(this);
            GameLogic.MovePlayer(Steps);
            GameLogic.CurrentEscapeCards.AddRange(PlacedCards);
        }
    }
}