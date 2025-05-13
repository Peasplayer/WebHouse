namespace WebHouse_Client.Logic;

public class ChapterCard
{
    public string Chapter { get; }
    public int Steps { get; }
    public List<CardColor> Requirements { get; }
    public int Counter;
    
    public Components.ChapterCard Component { get; }

    public ChapterCard(string chapter, int steps, List<CardColor> requirements)
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
            
        }
    }
}