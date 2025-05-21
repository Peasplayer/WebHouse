using WebHouse_Client.Logic;

namespace WebHouse_Client.Components;

public class ChapterCardPile
{
    public Panel Panel;
    
    public ChapterCardPile()
    {
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
    