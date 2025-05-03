namespace WebHouse_Client.Logic;

public class ChapterCard
{
    public string Chapter { get; }
    public int Steps { get; }
    public List<CardColor> Requirements { get; }
    
    public Components.ChapterCard Component { get; }

    public ChapterCard(string chapter, int steps, List<CardColor> requirements)
    {
        Chapter = chapter;
        Steps = steps;
        Requirements = requirements;
        
        Component = new Components.ChapterCard(this);
    }
}