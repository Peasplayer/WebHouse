using WebHouse_Client.Logic;
using WebHouse_Client.Networking;

namespace WebHouse_Client.Components;

public class ChapterCardPile
{
    public Panel Panel; //Panel das den Kartenstapel darstellt
    public int Index;
    
    public ChapterCardPile(int index)
    {
        Index = index;
        
        Panel = new BufferPanel
        {
            BackColor = Color.FromArgb(15, Color.Gray),
            BorderStyle = BorderStyle.FixedSingle,
        };

        //Reagiert wenn mit der linken Maustaste auf den Kartenstapel geklickt wird
        Panel.MouseClick += (_, args) =>
        {
            if (args.Button == MouseButtons.Left)
                OnClick();
        };
    }

    //Wird ausgeführt wenn auf den Kartenstapel geklickt wird
    private void OnClick()
    {
        //Wird nur ausgeführt wenn der Spieler am Zug ist und das Spiel in einem Zustand ist in dem die Karten ausgewählt werden können
        if (!NetworkManager.Instance.LocalPlayer.IsTurn || GameLogic.TurnState == 2)
            return;
        
        //Überprüfen ob eine EscapeCard ausgewählt ist
        var selectedChapterCard = ChapterCard.SelectedChapterCard;
        if (selectedChapterCard == null)
        {
            return;
        }
        //Netztwerkaufruf um die ChapterCard auf dem Stapel zu platzieren
        NetworkManager.Rpc.PlaceChapterCard(selectedChapterCard.Card, Index);

        //Karte wird abgewählt
        selectedChapterCard.CardComponent.SetHighlighted(false);
        ChapterCard.SelectedChapterCard = null;
    }

}
    