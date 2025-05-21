using WebHouse_Client.Networking;

namespace WebHouse_Client.Components;

public class DiscardPile
{
    public Panel Panel;
    public int Index;
    
    public DiscardPile(int index)
    {
        Index = index;
        
        Panel = new BufferPanel
        {
            BackColor = Color.FromArgb(15, Color.Gray),
            BorderStyle = BorderStyle.FixedSingle,
        };

        Panel.MouseClick += (_, args) =>
        {
            if (args.Button == MouseButtons.Left)
                OnClick();
        };
    }

    private void OnClick()
    {
        var selectedChapterCard = ChapterCard.SelectedChapterCard;
        if (selectedChapterCard == null)
        {
            return;
        }
        
        NetworkManager.Instance.Rpc.PlaceChapterCard(selectedChapterCard.Card, Index);

        //Karte wird abgewählt
        selectedChapterCard.CardComponent.SetHighlighted(false);
        ChapterCard.SelectedChapterCard = null;
    }
}
    