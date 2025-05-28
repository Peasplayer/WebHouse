using System.Drawing.Drawing2D;
using WebHouse_Client.Logic;
using WebHouse_Client.Networking;

namespace WebHouse_Client.Components;

public class ChapterCard : IComponentCard
{
    public static ChapterCard? SelectedChapterCard; //Die aktuell ausgewählte ChapterCard
    
    public Card CardComponent { get; } //Die Komponente die die Karte darstellt
    public ChapterCardPile? Pile { get; set; } //Der Kartenstapel auf den die karte gelegt werden kann
    public Panel Panel => CardComponent.Panel; //Panel das die Karte darstellt
    public Logic.ChapterCard Card { get; } //Die Logik die die Karte haben soll

    public ChapterCard(Logic.ChapterCard card)
    {
        Card = card;
        
        //Zeichnet die Karte 
        CardComponent = new Card(5, 10, Color.Black, 2, g =>
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            DrawTitle(g);
            DrawArrow(g);
            DrawNeededColors(g);
            DrawCounter(g);
        });
        
        Panel.Tag = this; //Ermöglicht das zugreifen auf ein bestimmtes ChapterCard Objekt
        //Wenn die Karte mit der Linken mouse Taste angeklickt wird wird die OnClick Methode aufgerufen
        Panel.MouseClick += (_, args) =>
        {
            if (args.Button == MouseButtons.Left)
                OnClick();
        };
    }

    //Wird ausgeführt wenn auf die Karte geklickt wird
    private void OnClick()
    {
        //Wenn der Spieler nicht am Zug ist oder wenn das Spiel in einem Zustand ist in dem die Karten nicht ausgewählt werden sollen
        if (!NetworkManager.Instance.LocalPlayer.IsTurn || GameLogic.TurnState == 2)
            return;
        
        //Überprüfen ob eine EscapeCard ausgewählt ist
        if (EscapeCard.SelectedEscapeCard != null)
        {
            //Wenn die Karte eine SpecialCard ist und keine Anforderungen hat wird sie abgewählt
            if (Card.IsSpecial && Card.Requirements.Count == 0)
            {
                EscapeCard.SelectedEscapeCard.CardComponent.SetHighlighted(false);
                EscapeCard.SelectedEscapeCard = null;
                return;
            }
            //Wenn die ChapterCard auf einem Ablagestapel liegt oder eine SpecialCard ist
            if (Pile != null || Card.IsSpecial)
            {
                //Überprüft ob die EscapeCard an die ChapterCard angelegt werden darf
                if (Card.DoesEscapeCardMatch(EscapeCard.SelectedEscapeCard.Card))
                {
                    NetworkManager.Rpc.PlaceEscapeCard(EscapeCard.SelectedEscapeCard.Card, Card.IsSpecial ? -1 : Pile.Index);
                }
                else
                {
                    EscapeCard.SelectedEscapeCard.CardComponent.SetHighlighted(false);
                }
           
                EscapeCard.SelectedEscapeCard = null;
                return;
            }
            //Wenn die EscapeCard nicht an die ChapterCard angelegt werden darf wird sie abgewählt und die Methode beendet
            EscapeCard.SelectedEscapeCard.CardComponent.SetHighlighted(false); 
            EscapeCard.SelectedEscapeCard = null;
        }
        //Wenn die Karte eine SpecialCard ist wird sie nicht ausgewählt
        if (Card.IsSpecial)
            return;
        
        //Prüft ob die ChapterCard schon ausgewählt ist
        if (SelectedChapterCard == this)
        {
            //Wenn sie schon ausgewählt ist wird sie abgewählt
            CardComponent.SetHighlighted(false);
            SelectedChapterCard = null;
        }
        else
        {
            //Wenn sie noch nicht ausgewählt ist wird sie ausgewählt
            if (SelectedChapterCard != null)
                SelectedChapterCard.CardComponent.SetHighlighted(false);

            SelectedChapterCard = this;
            CardComponent.SetHighlighted(true);
        }
    }
    //Zeichnet den Titel der Karte. Die größe des textes ist abhängig von der größe des Panels und daher von der größe des Bildschirms
    private void DrawTitle(Graphics g)
    {
        Font font = new Font("Arial", Panel.Width, FontStyle.Bold, GraphicsUnit.Pixel); // größer (vorher 12)
        var ratioSize = g.MeasureString(Card.Chapter.ToString(), font);
        font = new Font("Arial", (int)(Panel.Width * 0.8 * ratioSize.Height / ratioSize.Width), FontStyle.Bold, GraphicsUnit.Pixel);
        SizeF textSize = g.MeasureString(Card.Chapter.ToString(), font);
        PointF textPosition = new PointF((Panel.Width - textSize.Width) / 2, Panel.Height / 8f);
        g.DrawString(Card.Chapter.ToString(), font, Brushes.White, textPosition);
    }

    //Zeichnet den Pfeil der die Anzahl der Schritte anzeigt
    private void DrawArrow(Graphics g)
    {
        int shaftHeight = Panel.Height / 6;
        int shaftWidth = (int)(Panel.Width * 0.65f); // Pfeilschaft verlängert
        int arrowHeight = Panel.Height / 4;
        int arrowHeadWidth = Panel.Width / 4; // größerer Pfeilkopf
        int startX = 0;
        float centerY = (Panel.Height - arrowHeight) / 2f + Panel.Height / 20f;

        RectangleF shaftRect = new RectangleF(startX, centerY, shaftWidth, shaftHeight);

        using var arrowBackground = new SolidBrush(Color.White);
        using var numberFont = new Font("Arial", shaftHeight, FontStyle.Bold, GraphicsUnit.Pixel);
        using var numberColor = new SolidBrush(Color.Black);

        var path = new GraphicsPath();
        path.StartFigure();
        path.AddLine(shaftRect.Right - 1, centerY + shaftHeight / 2f + arrowHeight / 2f,
            shaftRect.Right + arrowHeadWidth, centerY + shaftHeight / 2f);
        path.AddLine(shaftRect.Right + arrowHeadWidth, centerY + shaftHeight / 2f,
            shaftRect.Right - 1, centerY + shaftHeight / 2f - arrowHeight / 2f);
        path.AddLine(shaftRect.Right - 1, centerY + shaftHeight / 2f - arrowHeight / 2f,
            shaftRect.Right - 1, centerY + shaftHeight / 2f + arrowHeight / 2f);
        path.CloseFigure();

        g.FillPath(arrowBackground, path);
        g.FillRectangle(arrowBackground, shaftRect);

        string text = Card.Steps.ToString();
        SizeF textSize = g.MeasureString(text, numberFont);
        float textX = shaftRect.Left + (shaftRect.Width - textSize.Width) / 2;
        float textY = shaftRect.Top + (shaftRect.Height - textSize.Height) / 2;
        g.DrawString(text, numberFont, numberColor, textX, textY);
    }
    //Zeichnet die benötigten Farben für die Karte. Die größe der Kästen ist abhängig von der größe des Panels und daher von der größe des Bildschirms
    private void DrawNeededColors(Graphics g)
    {
        int dotWidth = Panel.Width / 8;
        int dotHeight = Panel.Height / 8;
        int cornerRadius = Math.Min(dotWidth, dotHeight) / 4;
        int space = Panel.Width / 50; // größerer Abstand zwischen Farbkästen
        int totalWidth = Card.Requirements.Count * dotWidth + (Card.Requirements.Count - 1) * space;
        int startX = (Panel.Width - totalWidth) / 2;
        int y = Panel.Height - dotHeight - Panel.Height / 30;

        for (int i = 0; i < Card.Requirements.Count; i++)
        {
            Color color = Card.Requirements[i].GetColor();
            using var brush = new SolidBrush(color);
            Rectangle rect = new Rectangle(startX + i * (dotWidth + space), y, dotWidth, dotHeight);
            using var path = RoundedRectangle(rect, cornerRadius);
            g.FillPath(brush, path);
        }
    }
    //Berechnet eine Rechteck mit abgerundeten Ecken.
    private GraphicsPath RoundedRectangle(Rectangle rect, int cornerRadius)
    {
        int diameter = cornerRadius * 2;
        GraphicsPath path = new GraphicsPath();
        path.AddArc(rect.Left, rect.Top, diameter, diameter, 180, 90);
        path.AddArc(rect.Right - diameter, rect.Top, diameter, diameter, 270, 90);
        path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
        path.AddArc(rect.Left, rect.Bottom - diameter, diameter, diameter, 90, 90);
        path.CloseFigure();
        return path;
    }
    //Malt eine Zahl oben rechts auf die Karte die anzeigt das nur EscapeCards angelegt werden können die größer oder gleich dieser Zahl sind
    private void DrawCounter(Graphics g)
    {
        if(Card.Counter > 0)
        {
            using var font = new Font("Arial", 10, FontStyle.Bold);
            using var brush = new SolidBrush(Color.White);
        
            string text = $"#{Card.Counter}";
            SizeF textSize = g.MeasureString(text, font);
            float padding = 8; //Abstand zum Rand
            float x = Panel.Width - textSize.Width - padding;
            float y = padding;
            g.DrawString(text, font, brush, x, y);
        }
    }
}
