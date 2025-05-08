using System.Drawing.Drawing2D;
using System.Reflection;
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
        
        CardComponent = new Card(new Size(135, 200), 5, 10,
            Color.Black, 2);
        Panel.Paint += DrawEscapeCards;
    }
    private void DrawEscapeCards(object? sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        
        SplashBackground(g);
        DrawNumber(g);
        DrawRoom(g);
    }
    private void SplashBackground(Graphics g) //Splash Hintergrung
    {
        Image SplashBackgroundImage = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("WebHouse_Client.Resources.Background_Images.SplashBackground.png"));
        
        var rect = new Rectangle(Panel.ClientRectangle.Width / 20, Panel.ClientRectangle.Width / 20, 
            Panel.ClientRectangle.Width - Panel.ClientRectangle.Width / 20 * 2, 
            Panel.ClientRectangle.Height - Panel.ClientRectangle.Width / 20 * 2);
        g.FillRectangle(new SolidBrush(Card.Color.GetColor()), rect);
        g.DrawImage(SplashBackgroundImage, rect);
    }
    private void DrawNumber(Graphics g) //Zeichnet die Nummer unter dem Raumnamen
    {
        using (Font font = new Font("Arial", 15, FontStyle.Bold))
        using (var brush = new SolidBrush(Color.White))
        {
            string text = Card.Number.ToString();
            SizeF numberSize = g.MeasureString(text, font);
            
            var centerPos = new PointF((Panel.Width - numberSize.Width)/2, ((Panel.Height - numberSize.Height) / 2)+ 25);
            g.DrawString(text, font, brush, centerPos);
        }
    }
    private void DrawRoom(Graphics g) // Zeichnet den Raumnamen in der Kartenmitte
    {
        using (Font font = new Font("Arial", 14, FontStyle.Bold))
        using (var brush = new SolidBrush(Color.Black))
        {
            SizeF roomSize = g.MeasureString(Card.Room, font);
            var roomPos = new PointF((Panel.Width - roomSize.Width)/2, (Panel.Height - roomSize.Height) / 2);
            g.DrawString(Card.Room, font, brush, roomPos);
        }
    }
}