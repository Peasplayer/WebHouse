using System.Drawing.Drawing2D;
using WebHouse_Client.Logic;

namespace WebHouse_Client.Components;

public class EscapeCard
{
    private Card CardComponent { get; }
    public Panel Panel => CardComponent.Panel;
    public Logic.EscapeCard Card { get; }
    
    private static List<EscapeCard> EscapeCardsList = new List<EscapeCard>();

    public EscapeCard(Logic.EscapeCard card)
    {
        Card = card;
        
        CardComponent = new Card(new Size(135, 200), 5, 10, Card.Color.GetColor(), 2);
        Panel.Paint += DrawEscapeCards;
        Panel.Tag = this; //Verbindet das Objekt Pannel mit seinem EscapeCard Objekt
        EscapeCardsList.Add(this);
    }
    private void DrawEscapeCards(object? sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        
        DrawNumber(g);
        DrawRoom(g);
    }

    private void DrawNumber(Graphics g) //Zeichnet die Nummern oben, links und rechts
    {
        using (Font font = new Font("Arial", 10, FontStyle.Bold))
        using (var brush = new SolidBrush(Color.Black))
        {
            string text = Card.Number.ToString();
            SizeF size = g.MeasureString(text, font);
            
            // Nummer links oben
            var leftPos = new PointF(10, 10);
            g.DrawString(text, font, brush, leftPos);

            // Nummer rechts oben
            var rightPos = new PointF(Panel.Width - size.Width - 10, 10);
            g.DrawString(text, font, brush, rightPos);
        }
    }

    private void DrawRoom(Graphics g) // Zeichnet den Raumnamen in der Kartenmitte
    {
        using (Font font = new Font("Arial", 10, FontStyle.Bold))
        using (var brush = new SolidBrush(Color.Black))
        {
            SizeF roomSize = g.MeasureString(Card.Room, font);
            var roomPos = new PointF((Panel.Width - roomSize.Width)/2, (Panel.Height - roomSize.Height) / 2);
            g.DrawString(Card.Room, font, brush, roomPos);
        }
    }
    
    /*
     Ordnet die Karten in einer Horizontalen Line an. Nutzbar für das Inventar
    private void PositionCards()
    {
        int spacing = 10; //Abstand zwischen den einzelnen Karten
        int startX = 0; //Startposition für die erste Karte

        //Berechne die Position für jeder Karte
        for (int i = 0; i < EscapeCardsList.Count; i++)
        {
            var card = EscapeCardsList[i];
            int x = startX + i * (card.Panel.Width + spacing); //Berechne die X-Position
            card.Panel.Location = new Point(x, card.Panel.Top);
        }
    }
    */
}