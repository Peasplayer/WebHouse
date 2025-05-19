using WebHouse_Client.Logic;

namespace WebHouse_Client.Components;

public class DiscardPile
{
    public Panel Panel;
    
    public DiscardPile()
    {
        Panel = new Panel
        {
            BackColor = Color.Gray,
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

        Panel.Enabled = false;
        Panel.Visible = false;
        selectedChapterCard.Panel.Location = Panel.Location;
        selectedChapterCard.Panel.Size = Panel.Size;
        selectedChapterCard.Panel.BringToFront();
        selectedChapterCard.Pile = this;
        
        GameLogic.PlaceChapterCard(selectedChapterCard.Card);

        //Karte wird abgewählt
        selectedChapterCard.CardComponent.SetHighlighted(false);
        ChapterCard.SelectedChapterCard = null;
    }
}
    