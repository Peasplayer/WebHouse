using System.Drawing.Drawing2D;
using WebHouse_Client.Logic;

namespace WebHouse_Client.Components;

public class EscapeCard
{
    private Card CardComponent { get; }
    public Panel Panel => CardComponent.Panel;
    public Logic.EscapeCard Card { get; }

    public EscapeCard(Logic.EscapeCard card)
    {
        Card = card;
        
        CardComponent = new Card(new Size(135, 200), 5, 10, Card.Color.GetColor(), 2);
        Panel.Paint += DrawEscapeCards;
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
}