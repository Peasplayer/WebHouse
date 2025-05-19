using System.Reflection;
using WebHouse_Client.Components;
using WebHouse_Client.Logic;
using ChapterCard = WebHouse_Client.Logic.ChapterCard;
using EscapeCard = WebHouse_Client.Logic.EscapeCard;

namespace WebHouse_Client;

public partial class GameForm : Form
{
    private Button playerMoveButton = new Button();
    private Button opponentMoveButton = new Button();

    private PictureBox? roomImage;
    private PictureBox? playerImage;
    private PictureBox? opponentImage;
    private Panel? inventoryContainer;
    private Panel? drawPile1;
    private Button? drawChapterCardButton;
    private Button? drawEscapeCardButton;
    private Panel? drawPile4;
    private List<DiscardPile> discardPiles = new List<DiscardPile>();
    private Panel? infoPanel;

    private Rectangle boardContainer;
    private int widthUnit;
    private int heightUnit;

    public GameForm()
    {
        InitializeComponent();
        AddTempButtons();

        this.WindowState = FormWindowState.Maximized; //macht Vollbild
        this.SizeChanged += (_, _) => { RenderBoard(); };

        GameLogic.Start(this);

        RenderBoard();
    }

    public void RenderBoard()
    {
        boardContainer = new Rectangle();

        var boardWidth = ClientSize.Width;
        var boardHeight = ClientSize.Width * 9 / 16;
        if (boardHeight > ClientSize.Height)
        {
            boardWidth = ClientSize.Height * 16 / 9;
            boardHeight = ClientSize.Height;
        }

        boardContainer.Width = boardWidth;
        boardContainer.Height = boardHeight;
        boardContainer.Location = new Point((ClientSize.Width - boardContainer.Width) / 2,
            (ClientSize.Height - boardContainer.Height) / 2);

        widthUnit = boardContainer.Width / 32;
        heightUnit = boardContainer.Height / 18;

        if (roomImage == null)
        {
            roomImage = new PictureBox();
            roomImage.SizeMode = PictureBoxSizeMode.Zoom;
            Controls.Add(roomImage);
        }

        // Bild immer neu setzen
        roomImage.Image = Image.FromStream(
            Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("WebHouse_Client.Resources.Background_Images." +
                                           GameLogic.CurrentRoom.Picture));

        roomImage.BackColor = Color.Green;
        roomImage.Width =
            16 * widthUnit; //Math.Min(9 * heightUnit, 20 * widthUnit * 9 / 16);//heightUnit * 9;//roomImageHeight;
        roomImage.Height = 9 * heightUnit; //roomImage.Height * 16 / 9;//widthUnit * 20;//roomImageWidth;
        //Oben rechts positionieren
        roomImage.Location =
            new Point(boardContainer.X + 15 * widthUnit,
                boardContainer.Y +
                heightUnit); //new Point(boardContainer.X + boardContainer.Width - widthUnit - roomImage.Width, boardContainer.Y + heightUnit);

        if (inventoryContainer == null)
        {
            inventoryContainer = new Panel();
            inventoryContainer.BackColor = Color.Aquamarine;
            Controls.Add(inventoryContainer);
        }

        inventoryContainer.Size =
            new Size(16 * widthUnit,
                6 * heightUnit); //(GetRelativeSize(ClientSize, true, percentage: 50), GetRelativeSize(ClientSize, false, percentage: 33.34));
        inventoryContainer.Location =
            new Point(boardContainer.X + 15 * widthUnit,
                boardContainer.Y +
                11 * heightUnit); //new Point(boardContainer.X + boardContainer.Width - widthUnit - inventoryContainer.Width, boardContainer.Y + 11 * heightUnit);

        var cardHeight = Math.Min(inventoryContainer.Height,
            GetRelativeSize(inventoryContainer.Size, true, percentage: 16.67) * 3 / 2); //cardWidth * 3 / 2;
        var cardWidth = cardHeight * 2 / 3; //GetRelativeSize(inventoryContainer.Size, true, percentage: 16.67);
        for (var i = 0; i < GameLogic.Inventory.Count; i++)
        {
            var card = GameLogic.Inventory[i];
            var location = new Point(inventoryContainer.Location.X + (cardWidth / 6)
                                                                   + i * cardWidth + i * (cardWidth / 6),
                inventoryContainer.Location.Y + (inventoryContainer.Height - cardHeight) / 2);
            var size = new Size(cardWidth, cardHeight);

            if (card is EscapeCard escapeCard)
            {
                escapeCard.Component.Panel.Size = size;
                escapeCard.Component.Panel.Location = location;
                escapeCard.Component.Panel.BringToFront();
            }

            if (card is ChapterCard chapterCard)
            {
                chapterCard.Component.Panel.Size = size;
                chapterCard.Component.Panel.Location = location;
                chapterCard.Component.Panel.BringToFront();
            }
        }

        //Alte PictureBox entfernen
        if (playerImage == null)
        {
            // Neue PictureBox erzeugen
            playerImage = new PictureBox();
            playerImage.BackColor = Color.Transparent;
            playerImage.SizeMode = PictureBoxSizeMode.Zoom;
            playerImage.Image = Image.FromStream(
                Assembly.GetExecutingAssembly().GetManifestResourceStream
                    ("WebHouse_Client.Resources.Images.Figure.png"));
            playerImage.Image.RotateFlip(RotateFlipType.RotateNoneFlipX);
            roomImage.Controls.Add(playerImage);
        }

        playerImage.Size = new Size(GetRelativeSize(roomImage.Size, true, 80),
            GetRelativeSize(roomImage.Size, false, 120));

        //Alte PictureBox entfernen
        if (opponentImage == null)
        {
            //Neue PictureBox erzeugen
            opponentImage = new PictureBox();
            opponentImage.BackColor = Color.Transparent;
            opponentImage.SizeMode = PictureBoxSizeMode.Zoom;
            opponentImage.Image = Image.FromStream(
                Assembly.GetExecutingAssembly().GetManifestResourceStream
                    ("WebHouse_Client.Resources.Images.Opponent.png"));
            opponentImage.Image.RotateFlip(RotateFlipType.RotateNoneFlipX);
            roomImage.Controls.Add(opponentImage);
        }

        opponentImage.Size = new Size(GetRelativeSize(roomImage.Size, true, 80),
            GetRelativeSize(roomImage.Size, false, 120));

        UpdatePositions();

        if (drawPile1 == null)
        {
            drawPile1 = new Panel();
            drawPile1.BackColor = Color.Yellow;
            Controls.Add(drawPile1);
        }

        drawPile1.Size = new Size(widthUnit * 2, heightUnit * 3);
        drawPile1.Location = new Point(boardContainer.X + 12 * widthUnit, boardContainer.Y + 1 * heightUnit);


        
        if (drawChapterCardButton == null)
        {
            GameLogic.InitializeDeck();
            drawChapterCardButton = new Button();
            drawChapterCardButton.Text = "ChapterCard";
            drawChapterCardButton.MouseClick += (_, args) =>
            {
                if (args.Button != MouseButtons.Left || GameLogic.Inventory.Count >= 5 || GameLogic.ChapterDeck.Count == 0)
                    return;

                // Karte aus dem Deck ziehen
                var card = GameLogic.ChapterDeck[0];
                GameLogic.ChapterDeck.RemoveAt(0);

                Controls.Add(card.Component.Panel);
                card.Component.Panel.BringToFront();

                GameLogic.Inventory.Add(card);
                
                RenderBoard();
            };
            drawChapterCardButton.BackColor = Color.Yellow;
            Controls.Add(drawChapterCardButton);
        }


        drawChapterCardButton.Size = new Size(widthUnit * 2, heightUnit * 3);
        drawChapterCardButton.Location =
            new Point(boardContainer.X + 12 * widthUnit, boardContainer.Y + 6 * heightUnit);

        if (drawEscapeCardButton == null)
        {
            drawEscapeCardButton = new Button();
            drawEscapeCardButton.Text = "EscapeCard";
            drawEscapeCardButton.MouseClick += (_, args) =>
            {
                if (args.Button != MouseButtons.Left || GameLogic.Inventory.Count >= 5)
                    return;

                var escapeCard = new EscapeCard(Random.Shared.Next(15) + 1, "Test", Random.Shared.Next(3) switch
                {
                    0 => CardColor.Red,
                    1 => CardColor.Green,
                    2 => CardColor.Blue,
                    _ => CardColor.Red
                });
                Controls.Add(escapeCard.Component.Panel);
                escapeCard.Component.Panel.BringToFront();
                GameLogic.Inventory.Add(escapeCard);

                RenderBoard();
            };
            drawEscapeCardButton.BackColor = Color.Yellow;
            Controls.Add(drawEscapeCardButton);
        }

        drawEscapeCardButton.Size = new Size(widthUnit * 2, heightUnit * 3);
        drawEscapeCardButton.Location =
            new Point(boardContainer.X + 12 * widthUnit, boardContainer.Y + 10 * heightUnit);

        if (drawPile4 == null)
        {
            drawPile4 = new Panel();
            drawPile4.BackColor = Color.Yellow;
            Controls.Add(drawPile4);
        }

        drawPile4.Size = new Size(widthUnit * 2, heightUnit * 3);
        drawPile4.Location = new Point(boardContainer.X + 12 * widthUnit, boardContainer.Y + 14 * heightUnit);

        if (infoPanel == null)
        {
            infoPanel = new Panel();
            infoPanel.BackColor = Color.Red;
            Controls.Add(infoPanel);
        }

        if (discardPiles.Count == 0)
        {
            for (int i = 0; i < 9; i++)
            {
                var pile = new DiscardPile();
                discardPiles.Add(pile);
                Controls.Add(pile.Panel);
            }
        }

        foreach (var pile in discardPiles)
        {
            pile.Panel.Size = new Size(2 * widthUnit, 3 * heightUnit);
            pile.Panel.Location = new Point(boardContainer.X + (2 + discardPiles.IndexOf(pile) % 3 * 3) * widthUnit,
                boardContainer.Y + (6 + discardPiles.IndexOf(pile) / 3 * 4) * heightUnit);
            pile.Panel.BringToFront();
            pile.Panel.Invalidate();
        }

        foreach (var card in GameLogic.PlacedChapterCards)
        {
            card.Component.Panel.Size = new Size(2 * widthUnit, 3 * heightUnit);
            card.Component.Panel.Location =
                new Point(boardContainer.X + (2 + discardPiles.IndexOf(card.Component.Pile) % 3 * 3) * widthUnit,
                    boardContainer.Y + (6 + discardPiles.IndexOf(card.Component.Pile) / 3 * 4) * heightUnit);
            card.Component.Panel.BringToFront();
        }

        infoPanel.Size = new Size(widthUnit * 10, heightUnit * 4);
        infoPanel.Location = new Point(boardContainer.X + 1 * widthUnit, boardContainer.Y + 1 * heightUnit);
    }
    /*private void DrawChapterCard()
    {
        if (GameLogic.Inventory.Count >= 5 || GameLogic.ChapterDeck.Count == 0)
            return;

        var card = GameLogic.ChapterDeck.FirstOrDefault();
        if (card == null) return;

        GameLogic.ChapterDeck.Remove(card);
        Controls.Add(card.Component.Panel);
        card.Component.Panel.BringToFront();
        GameLogic.Inventory.Add(card);

        RenderBoard();
    }*/
    public void UpdatePositions()
    {
        var fields = Fields[GameLogic.CurrentRoom.RoomType];
        if (GameLogic.PlayerPosition >= 0 && GameLogic.PlayerPosition < fields.Count)
        {
            var point = fields[GameLogic.PlayerPosition];
            playerImage.Location = new Point(
                point.X * roomImage.Width / 1920,
                (point.Y - 50) * roomImage.Width / 1920
            );
        }

        if (GameLogic.OpponentPosition >= 0 && GameLogic.OpponentPosition < fields.Count)
        {
            var point = fields[GameLogic.OpponentPosition];
            opponentImage.Location = new Point(
                point.X * roomImage.Width / 1920,
                (point.Y - 50) * roomImage.Width / 1920
            );
        }
    }

    private void AddTempButtons()
    {
        playerMoveButton.Text = "Move Player";
        playerMoveButton.Size = new Size(100, 50);
        playerMoveButton.Location = new Point(10, 10);
        playerMoveButton.Click += (_, _) => GameLogic.MovePlayer(1);
        Controls.Add(playerMoveButton);

        opponentMoveButton.Text = "Move Opponent";
        opponentMoveButton.Size = new Size(100, 50);
        opponentMoveButton.Location = new Point(10, 70);
        opponentMoveButton.Click += (_, _) => GameLogic.MoveOpponent(1);
        Controls.Add(opponentMoveButton);
    }

    private int GetRelativeSize(Size size, bool width, int? pixels = null, double? percentage = null)
    {
        if (pixels != null)
        {
            return pixels.Value * (width ? size.Width : size.Height) / (width ? 1920 : 1080);
        }

        if (percentage != null)
        {
            return (int)Math.Round((width ? size.Width : size.Height) * percentage.Value / 100f,
                MidpointRounding.AwayFromZero);
        }

        throw new ArgumentException("Either pixels or percentage must be provided.");
    }

    private static Dictionary<Room.RoomName, List<Point>> Fields = new Dictionary<Room.RoomName, List<Point>>()
    {
        {
            Room.RoomName.HotelZimmer, new()
            {
                new(95, 115),
                new(240, 115),
                new(370, 190),
                new(405, 305),
                new(355, 420),
                new(280, 540),
                new(230, 665),
                new(245, 795),
                new(385, 820),
                new(530, 850),
                new(675, 855),
                new(790, 750),
                new(1055, 775),
                new(1205, 780),
                new(1345, 855),
                new(1460, 765),
                new(1465, 630),
                new(1520, 495),
                new(1595, 370),
                new(1675, 250),
                new(1750, 135),
            }
        },
        {
            Room.RoomName.Hafen, new()
            {
                new(65, 155),
                new(195, 210),
                new(310, 280),
                new(310, 400),
                new(310, 515),
                new(370, 620),
                new(495, 645),
                new(550, 760),
                new(680, 800),
                new(815, 830),
                new(1180, 810),
                new(1290, 720),
                new(1410, 680),
                new(1490, 580),
                new(1490, 470),
                new(1490, 360),
                new(1490, 250),
                new(1535, 155),
                new(1670, 140),
                new(1800, 130)
            }
        },
        {
            Room.RoomName.Stadt, new()
            {
                new(50, 55),
                new(175, 100),
                new(220, 210),
                new(270, 330),
                new(285, 455),
                new(270, 575),
                new(355, 705),
                new(405, 825),
                new(505, 930),
                new(640, 950),
                new(765, 905),
                new(1040, 910),
                new(1170, 930),
                new(1285, 855),
                new(1395, 755),
                new(1420, 640),
                new(1550, 630),
                new(1675, 620),
                new(1785, 540)

            }
        },
        {
            Room.RoomName.Wald, new()
            {
                new(50, 720),
                new(160, 695),
                new(260, 615),
                new(335, 520),
                new(450, 485),
                new(570, 470),
                new(685, 490),
                new(800, 505),
                new(1020, 540),
                new(1130, 585),
                new(1240, 630),
                new(1355, 620),
                new(1470, 580),
                new(1580, 510),
                new(1630, 410),
                new(1670, 310),
                new(1720, 215),
                new(1800, 120),
            }
        },
        {
            Room.RoomName.SafeHouse, new()
            {
                new(110, 765),
                new(245, 805),
                new(385, 825),
                new(520, 855),
                new(665, 905),
                new(795, 880),
                new(1040, 880),
                new(1160, 875),
                new(1280, 835),
                new(1395, 755),
                new(1360, 645),
                new(1365, 510),
                new(20, 25),
                new(130, 40),
                new(160, 140),
                new(275, 155),
                new(390, 195),
                new(500, 180),
                new(580, 85),
                new(690, 60),
                new(810, 25),
                new(1035, 25),
                new(1105, 115),
                new(1220, 135),
                new(1330, 125),
                new(1440, 175),
                new(1390, 270),
                new(1360, 370),
                new(1360, 500),
            }
        },
    };
}