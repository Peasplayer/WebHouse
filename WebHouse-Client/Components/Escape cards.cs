using WebHouse_Client.Components;
using System.Drawing.Drawing2D;

namespace WebHouse_Client;

public class Escape_cards
{
    public Card Card { get; }
    public Panel CardPanel => Card.cardPanel;
    
    private int number;
    private string room;
    private Color color;

    public Escape_cards(int number, string room, Color color)
    {
        this.number = number;
        this.room = room;
        this.color = color;
        
        Card = new Card(new Size(135, 200), 5, 10, color, 2);
        CardPanel.Paint += DrawEscapeCards;
    }
    private void DrawEscapeCards(object sender, PaintEventArgs e)
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
            string text = number.ToString();
            SizeF size = g.MeasureString(text, font);
            
            // Nummer links oben
            var leftPos = new PointF(10, 10);
            g.DrawString(text, font, brush, leftPos);

            // Nummer rechts oben
            var rightPos = new PointF(CardPanel.Width - size.Width - 10, 10);
            g.DrawString(text, font, brush, rightPos);
        }
    }
    private void DrawRoom(Graphics g) // Zeichnet den Raumnamen in der Kartenmitte
    {
        using (Font font = new Font("Arial", 10, FontStyle.Bold))
        using (var brush = new SolidBrush(Color.Black))
        {
            SizeF roomSize = g.MeasureString(room.ToString(), font);
            var roomPos = new PointF((CardPanel.Width - roomSize.Width)/2, (CardPanel.Height - roomSize.Height) / 2);
            g.DrawString(room, font, brush, roomPos);
        }
    }
}