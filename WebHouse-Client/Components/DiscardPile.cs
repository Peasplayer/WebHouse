namespace WebHouse_Client.Components;

public class DiscardPile
{
    public List<Panel> discardPile { get; } = new(); //Liste der Ablagestappeln
    private readonly List<ChapterCard?> placedCards = new(); //Liste der Karten die auf den Ablagestapeln liegen

    public DiscardPile(Control parent, Point startPosition)
    {
        int spacing = 10;
        int width = 135;
        int height = 200;

        for (int i = 0; i < 9; i++)
        {
            Panel discardPlace = new Panel
            {
                Size = new Size(width, height),
                BackColor = Color.Gray,
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(startPosition.X + i * (width + spacing), startPosition.Y),
                Tag = i //Indesx zur Identifizierung des Slots
            };

            discardPlace.MouseClick += discardPlaceClicked;

            discardPile.Add(discardPlace); //Ablagestapel zur Liste hinzufügen
            placedCards.Add(null);  //Setzt die Liste in der gespeichert wird ob eine Karte auf dem Ablagestapel liegt auf null
            parent.Controls.Add(discardPlace);
        }
    }

    private void discardPlaceClicked(object? sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Left)
        {
            return;
        }

        if (sender is not Panel slot)
        {
            return;
        }

        var selectedChapterCard = ChapterCard.SelectedChapterCard;
        if (selectedChapterCard == null)
        {
            return;
        }

        int index = (int)slot.Tag; //Index des Ablagestapels auslesen

        if (placedCards[index] != null) //Wenn auf dem Ablagestapel schon eine Karte liegt wird nichts gemacht
        {
            return;
        }

        //Wenn eine Karte schon auf einem Ablagestapel liegt wird sie aus dem Ablagestaple entfernt wenn die Karte bewegt oder entfernt wird
        if (selectedChapterCard.DiscardPileInstance != null)
        {
            selectedChapterCard.DiscardPileInstance.RemoveCard(selectedChapterCard);
        }
        

        //karte auf dem Ablagestapel legen
        var panel = selectedChapterCard.Panel;
        panel.Location = slot.Location;
        panel.BringToFront();

        placedCards[index] = selectedChapterCard;

        //Speichert auf welchem Ablagestapel die Karte liegt
        selectedChapterCard.DiscardPileInstance = this;

        slot.Enabled = false; //Ablagestapel wird deaktiviert wenn eine Karte draufliegt

        //Karte wird abgewählt
        selectedChapterCard.CardComponent.SetHighlighted(false);
        ChapterCard.SelectedChapterCard = null;
    }

    //Entfernt die Karte von dem Ablagestapel
    public void RemoveCard(ChapterCard? chapterCard)
    {
        for (int i = 0; i < placedCards.Count; i++)
        {
            if (placedCards[i] == chapterCard)
            {
                placedCards[i] = null; //Ablagestapple kann wieder genutzt werden
                discardPile[i].Enabled = true; //Ablagestapple kann wieder genutzt werden
                break;
            }
        }
    }
}
    