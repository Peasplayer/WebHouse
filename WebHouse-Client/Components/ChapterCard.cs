using System.Drawing.Drawing2D;
using WebHouse_Client.Logic;

namespace WebHouse_Client.Components;

public class ChapterCard
{
    public static ChapterCard? SelectedChapterCard;
    
    public Card CardComponent { get; }
    public DiscardPile? Pile { get; set; }
    public Panel Panel => CardComponent.Panel;
    public Logic.ChapterCard Card { get; }

    public ChapterCard(Logic.ChapterCard card)
    {
        Card = card;
        
        CardComponent = new Card(5, 10, Color.Black, 2, g =>
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            DrawTitle(g);
            DrawArrow(g);
            DrawNeededColors(g);
            DrawCounter(g);
        });
        
        Panel.Tag = this; //Ermöglicht das zugreifen auf ein bestimmtes ChapterCard Objekt
        Panel.MouseClick += (_, args) =>
        {
            if (args.Button == MouseButtons.Left)
                OnClick();
        };
        //new DraggableControl(Panel); //macht die Karte direkt bewegbar so das er DraggableControler nicht bei ersrstellen aufgerufen werden muss
    }

    private void OnClick()
    {
        //Überprüfen ob eine EscapeCard ausgewählt ist
        if (EscapeCard.SelectedEscapeCard != null)
        {
            if (Pile != null)
            {
                //Überprüft ob die EscapeCard an die ChapterCard angelegt werden darf
                if (Card.DoesEscapeCardMatch(EscapeCard.SelectedEscapeCard.Card))
                {
                    GameLogic.PlaceEscapeCard(EscapeCard.SelectedEscapeCard.Card, Card);
                }
                else
                {
                    EscapeCard.SelectedEscapeCard.CardComponent.SetHighlighted(false);
                }
           
                EscapeCard.SelectedEscapeCard = null;
                return;
            }
            
            EscapeCard.SelectedEscapeCard.CardComponent.SetHighlighted(false); 
            EscapeCard.SelectedEscapeCard = null;
        }

        if (Pile != null)
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
    
    private void DrawTitle(Graphics g)
    {
        Font font = new Font("Arial", Panel.Height / 15f, FontStyle.Bold); // größer (vorher 12)
        SizeF textSize = g.MeasureString(Card.Chapter, font);
        PointF textPosition = new PointF((Panel.Width - textSize.Width) / 2, 30);
        g.DrawString(Card.Chapter, font, Brushes.White, textPosition);
    }

    private void DrawArrow(Graphics g)
    {
        int shaftHeight = Panel.Height / 6;
        int shaftWidth = Panel.Width / 2;
        int arrowHeight = Panel.Height / 4;
        int arrowHeadWidth = Panel.Width / 5;
        int startX = 0;
        int centerY = (Panel.Height - arrowHeight) / 2 + Panel.Height / 10;

        Rectangle shaftRect = new Rectangle(startX, centerY, shaftWidth, shaftHeight);

        using var arrowBackground = new SolidBrush(Color.White);
        using var numberFont = new Font("Arial", Panel.Height / 10f, FontStyle.Bold);
        using var numberColor = new SolidBrush(Color.Black);

        var path = new GraphicsPath();
        path.StartFigure();
        path.AddLine(shaftRect.Right - 1, centerY + shaftHeight / 2 + arrowHeight / 2, shaftRect.Right + arrowHeadWidth, centerY + shaftHeight / 2);
        path.AddLine(shaftRect.Right + arrowHeadWidth, centerY + shaftHeight / 2, shaftRect.Right - 1, centerY + shaftHeight / 2 - arrowHeight / 2);
        path.AddLine(shaftRect.Right - 1, centerY + shaftHeight / 2 - arrowHeight / 2, shaftRect.Right - 1, centerY + shaftHeight / 2 + arrowHeight / 2);
        path.CloseFigure();
        
        /*path.AddLine(shaftRect.Left, shaftRect.Top, shaftRect.Right, shaftRect.Top); 
        path.AddLine(shaftRect.Right, shaftRect.Top, shaftRect.Right + arrowHeadWidth, shaftRect.Top + arrowHeight / 2); 
        path.AddLine(shaftRect.Right + arrowHeadWidth, shaftRect.Top + arrowHeight / 2, shaftRect.Right, shaftRect.Bottom); 
        path.AddLine(shaftRect.Right, shaftRect.Bottom, shaftRect.Left, shaftRect.Bottom); 
        */path.CloseFigure();
        
        g.FillPath(arrowBackground, path);
        g.FillRectangle(arrowBackground, shaftRect);

        string text = Card.Steps.ToString();
        SizeF textSize = g.MeasureString(text, numberFont);
        float textX = shaftRect.Left + (shaftRect.Width - textSize.Width) / 2;
        float textY = shaftRect.Top + (shaftRect.Height - textSize.Height) / 2;
        g.DrawString(text, numberFont, numberColor, textX, textY);
    }
    
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
