using System.Drawing.Drawing2D;
using System.Reflection;
using WebHouse_Client.Logic;
using WebHouse_Client.Networking;

namespace WebHouse_Client.Components;

public class EscapeCard : IComponentCard
{
    public static EscapeCard? SelectedEscapeCard;
    
    public Card CardComponent { get; }
    public Panel Panel => CardComponent.Panel;
    public Logic.EscapeCard Card { get; }

    public EscapeCard(Logic.EscapeCard card)
    {
        Card = card;
        
        CardComponent = new Card(5, 10,
            Color.Black, 2, g =>
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;

                if (Card.Type == Logic.EscapeCard.EscapeCardType.Normal)
                {
                    SplashBackground(g);
                    DrawNumber(g);
                    DrawRoom(g);
                }
                else
                {
                    Image SplashBackgroundImage = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("WebHouse_Client.Resources.Background_Images.Opponent.png"));
        
                    var rect = new Rectangle(Panel.ClientRectangle.Width / 20, Panel.ClientRectangle.Width / 20, 
                        Panel.ClientRectangle.Width - Panel.ClientRectangle.Width / 20 * 2, 
                        Panel.ClientRectangle.Height - Panel.ClientRectangle.Width / 20 * 2);
                    g.FillRectangle(new SolidBrush(Color.Black), rect);
                    g.DrawImage(SplashBackgroundImage, rect);

                    var text = card.Type == Logic.EscapeCard.EscapeCardType.OpponentSteps
                        ? card.Number.ToString()
                        : "[" + GameLogic.PlacedChapterCards.Count + "]" + (card.Number > 0 ? " + " + card.Number : "");
                    Font font = new Font(Program.Font, Panel.Height / 10f, FontStyle.Bold, GraphicsUnit.Pixel); 
                    SizeF textSize = g.MeasureString(text, font);
                    PointF textPosition = new PointF((Panel.Width - textSize.Width) / 2, Panel.Height / 20f);
                    g.DrawString(text, font, Brushes.White, textPosition);
                }
            });
        Panel.Tag = this; //Verbindet das Objekt Pannel mit seinem EscapeCard Objekt
        Panel.MouseClick += (_, args) =>
        {
            if (args.Button == MouseButtons.Left)
                OnClick();
        };
        
        //new DraggableControl(Panel); //macht die Karte direkt bewegbar so das er DraggableControler nicht bei ersrstellen aufgerufen werden muss
    }

    private void OnClick()
    {
        if (!NetworkManager.Instance.LocalPlayer.IsTurn || GameLogic.TurnState == 2)
            return;
        
        if (Card.Type != Logic.EscapeCard.EscapeCardType.Normal)
            return;
        
        if (SelectedEscapeCard != null)
        {
            SelectedEscapeCard.CardComponent.SetHighlighted(false);

            //Wenn man die gleiche Karte ancklickt wird sie abgewählt
            if (SelectedEscapeCard == this)
            {
                SelectedEscapeCard = null;
                return;
            }
        }

        //Karte wird abgewählt wenn eine andere Karte ausgewählt wird
        if (ChapterCard.SelectedChapterCard != null)
        {
            ChapterCard.SelectedChapterCard.CardComponent.SetHighlighted(false);
            ChapterCard.SelectedChapterCard = null;
        }

        //Die neue Karte wird ausgewählt
        SelectedEscapeCard = this;
        CardComponent.SetHighlighted(true);
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

    private void DrawNumber(Graphics g)
    {
        float fontSize = Panel.Height / 12f; // Schriftgröße relativ zur Panel-Höhe
        using (Font font = new Font("Arial", fontSize, FontStyle.Bold, GraphicsUnit.Pixel))
        using (var brush = new SolidBrush(Card.Color == CardColor.Yellow ? Color.Black : Color.White))
        {
            string text = Card.Number.ToString();
            SizeF numberSize = g.MeasureString(text, font);

            var centerPos = new PointF(
                (Panel.Width - numberSize.Width) / 2,
                (Panel.Height - numberSize.Height) / 2 + Panel.Height / 10f
            );
            g.DrawString(text, font, brush, centerPos);
        }
    }

    private void DrawRoom(Graphics g) // Zeichnet den Raumnamen in der Kartenmitte
    {
        float fontSize = Panel.Height / 12f; // Schriftgröße relativ zur Panel-Höhe
        using (Font font = new Font("Arial", fontSize, FontStyle.Bold, GraphicsUnit.Pixel))
        using (var brush = new SolidBrush(Color.Black))
        {
            SizeF roomSize = g.MeasureString(Card.Room.ToString(), font);
            var roomPos = new PointF((Panel.Width - roomSize.Width) / 2, (Panel.Height - roomSize.Height) / 2);
            g.DrawString(Card.Room.ToString(), font, brush, roomPos);
        }
    }
}