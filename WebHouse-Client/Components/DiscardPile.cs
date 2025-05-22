using WebHouse_Client.Logic;
using WebHouse_Client.Networking;

namespace WebHouse_Client.Components;

public class DiscardPile
{
    public Panel Panel;
    
    public DiscardPile()
    {
        Panel = new BufferPanel();
    }
    
    public static void Disposing()
    {
        if (!NetworkManager.Instance.LocalPlayer.IsTurn || GameLogic.TurnState == 2)
            return;
        
        var disposeEscapeCard = EscapeCard.SelectedEscapeCard;
        var disposeChapterCard = ChapterCard.SelectedChapterCard;
        
        if (disposeChapterCard != null)
        {
            if (disposeChapterCard.Pile != null)
            {
                NetworkManager.Rpc.MoveOpponent(1);
                NetworkManager.Rpc.DiscardChapterCard(disposeChapterCard.Card, disposeChapterCard.Pile.Index);
            }
            
            GameLogic.Inventory.Remove(disposeChapterCard.Card);
            //disposeChapterCard.Panel.Dispose();
            
            disposeChapterCard.CardComponent.SetHighlighted(false);
            ChapterCard.SelectedChapterCard = null;
            
            return;
        }
        
        if (disposeEscapeCard != null)
        {
            GameLogic.Inventory.Remove(disposeEscapeCard.Card);
            disposeEscapeCard.Panel.Dispose();
            NetworkManager.Rpc.DiscardEscapeCard(disposeEscapeCard.Card);
            
            disposeEscapeCard.CardComponent.SetHighlighted(false);
            EscapeCard.SelectedEscapeCard = null;
        }
    }
}