using System.ComponentModel;
using WebHouse_Client.Components;
using WebHouse_Client.Logic;

namespace WebHouse_Client.Components;

public class DiscardPile
{
    public Panel Panel;
    public IComponentCard? Component { get; set; }
    public DiscardPile()
    {
        Panel = new BufferPanel();
    }
    
    public static void Disposing()
    {
        var disposeEscapeCard = EscapeCard.SelectedEscapeCard;
        var disposeChapterCard = ChapterCard.SelectedChapterCard;
        
        //if (disposeEscapeCard == null && disposeChapterCard == null) //Schaut ob es eine aktuelle Karte gibt
            //return;
        
        if (disposeChapterCard != null)
        {
            if (disposeChapterCard.Pile != null)
            {
                disposeChapterCard.Pile.Panel.Enabled = true;
                disposeChapterCard.Pile.Panel.Visible = true;
                GameLogic.CurrentEscapeCards.AddRange(disposeChapterCard.Card.PlacedCards);

            }
            
            GameLogic.Inventory.Remove(disposeChapterCard.Card);
            disposeChapterCard.Panel.Dispose();
            
            disposeChapterCard.CardComponent.SetHighlighted(false);
            ChapterCard.SelectedChapterCard = null;
            
            return;
        }
        
        if (disposeEscapeCard != null)
        {
            GameLogic.Inventory.Remove(disposeEscapeCard.Card);
            disposeEscapeCard.Panel.Dispose();
            GameLogic.CurrentEscapeCards.Add(disposeEscapeCard.Card);
            
            disposeEscapeCard.CardComponent.SetHighlighted(false);
            EscapeCard.SelectedEscapeCard = null;
        }
    }
}