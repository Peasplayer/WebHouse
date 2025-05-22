using WebHouse_Client.Components;
using WebHouse_Client.Networking;

namespace WebHouse_Client.Logic;

public class ChapterCard : ILogicCard
{
    public Room.RoomName Chapter { get; }
    public int Steps { get; }
    public List<CardColor> Requirements { get; }
    public int Counter;
    public bool IsSpecial { get; }
    public List<EscapeCard> PlacedCards { get; } = new();
    
    public IComponentCard? Component { get; private set; }

    public ChapterCard(Room.RoomName chapter, int steps, List<CardColor> requirements, bool isSpecial = false)
    {
        Chapter = chapter;
        Steps = steps;
        Requirements = requirements;
        IsSpecial = isSpecial;
    }

    public void CreateComponent()
    {
        if (Component != null)
            Component.Panel.Dispose();
        Component = new Components.ChapterCard(this);
    }
    
    public bool DoesEscapeCardMatch(EscapeCard escapeCard)
    {
        return escapeCard.Type == EscapeCard.EscapeCardType.Normal && (Requirements.Contains(escapeCard.Color) || (IsSpecial && escapeCard.Room == Chapter)) && escapeCard.Number >= Counter;
    }
    
    public void AddEscapeCard(EscapeCard escapeCard)
    {
        if (Component == null)
            return;
        if (!DoesEscapeCardMatch(escapeCard))
            return;
        
        Counter = escapeCard.Number;
        Requirements.Remove(IsSpecial ? CardColor.White : escapeCard.Color);
        PlacedCards.Add(escapeCard);

        if (Requirements.Count == 0)
        {
            if (!IsSpecial)
            {
                ((Components.ChapterCard)Component).Pile.Panel.Enabled = true;
                ((Components.ChapterCard)Component).Pile.Panel.Visible = true;
                Component.Panel.Dispose();
                GameLogic.PlacedChapterCards.Remove(this);
            }
            
            if (NetworkManager.Instance.LocalPlayer.IsHost)
            {
                NetworkManager.Rpc.MovePlayer(Steps);
                GameLogic.CurrentEscapeCards.AddRange(PlacedCards);
            }
        }
    }
}