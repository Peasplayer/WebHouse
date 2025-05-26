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
        //Wird nur ausgeführt wenn der Spueler am Zug ist und das Spiel in einem Zustand ist in dem die Karten ausgewählt werden können
        if (!NetworkManager.Instance.LocalPlayer.IsTurn || GameLogic.TurnState == 2)
            return;
        //Überprüfen ob eine EscapeCard oder ChapterCard ausgewählt ist
        var disposeEscapeCard = EscapeCard.SelectedEscapeCard;
        var disposeChapterCard = ChapterCard.SelectedChapterCard;
        
        //Wenn eine ChapterCard ausgewählt ist
        if (disposeChapterCard != null)
        {
            //Fals die Karte auf einem Ablagestapel liegt wird sie auf dem Ablagestapel abgelegt
            if (disposeChapterCard.Pile != null)
            {
                NetworkManager.Rpc.MoveOpponent(1); //Der Verfolger wird bewegt als Strafe für das Ablegen einer ChapterCard
                NetworkManager.Rpc.DiscardChapterCard(disposeChapterCard.Card, disposeChapterCard.Pile.Index); //Dem Server wird mitgeteilt das eine ChapterCard abgelegt wurde
            }
            //Karte wird aus dem Inventar und aus der UI gelöscht
            GameLogic.Inventory.Remove(disposeChapterCard.Card);
            disposeChapterCard.Panel.Dispose();
            
            //Setzt die Auswahl und die Hervorhebung der Karte zurück
            disposeChapterCard.CardComponent.SetHighlighted(false);
            ChapterCard.SelectedChapterCard = null;
            
            return;
        }
        //Wenn eine EscapeCard ausgewählt ist
        if (disposeEscapeCard != null)
        {
            //Karte aus dem Inventar und der UI löschen
            GameLogic.Inventory.Remove(disposeEscapeCard.Card);
            disposeEscapeCard.Panel.Dispose();
            NetworkManager.Rpc.DiscardEscapeCard(disposeEscapeCard.Card); //Dem Server wird mitgeteilt das eine EscapeCard abgelegt wurde
            //Setzt die Auswahl und die Hervorhebung der Karte zurück
            disposeEscapeCard.CardComponent.SetHighlighted(false);
            EscapeCard.SelectedEscapeCard = null;
        }
    }
}