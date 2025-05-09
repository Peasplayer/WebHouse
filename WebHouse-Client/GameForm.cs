﻿using System.Reflection;
using WebHouse_Client.Components;
using WebHouse_Client.Logic;
using ChapterCard = WebHouse_Client.Logic.ChapterCard;

namespace WebHouse_Client;

public partial class GameForm : Form
{
    List<Button> GameField = new List<Button>();
    Button weiterButton = new Button(); //testknopf
    int position = 0;
    private PictureBox? roomImage;
    private PictureBox? figureBox;
    private Panel? inventoryContainer;
    private List<ChapterCard> chapterCards = new List<ChapterCard>();
    
    public GameForm()
    {
        InitializeComponent(); 
        this.WindowState = FormWindowState.Maximized; //macht Vollbild
        this.SizeChanged += (_, _) =>
        {
            CreateGameField();
        };
        
        CreateGameField();
        TestKnopf();
        
        GameLogic.Start();
    }

    public void CreateGameField()
    {
        // Bild laden
        Image image = Image.FromStream(
            Assembly.GetExecutingAssembly().GetManifestResourceStream("WebHouse_Client.Resources.Background_Images." + GameLogic.CurrentRoom.Picture));

        if (roomImage != null)
        {
            roomImage.Dispose();
        }
        
        //roomImage erstellen
        roomImage = new PictureBox();
        roomImage.Image = image;
        roomImage.SizeMode = PictureBoxSizeMode.Zoom;

        //Größe auf ein Viertel des Fensters setzen
        var width = GetRelativeSize(ClientSize, true, percentage: 60);
        var height = GetRelativeSize(ClientSize, true, percentage: 60) * 9 / 16;
        if (height > GetRelativeSize(ClientSize, false, percentage: 60))
        {
            width = GetRelativeSize(ClientSize, false, percentage: 60) * 16 / 9;
            height = GetRelativeSize(ClientSize, false, percentage: 60);
        }
        roomImage.Width = width;
        roomImage.Height = height;
        //Oben rechts positionieren
        roomImage.Location = new Point(ClientSize.Width - roomImage.Width, 0);
        //roomImage zur Form hinzufügen
        Controls.Add(roomImage);

        if (inventoryContainer != null)
        {
            inventoryContainer.Dispose();
        }
        
        inventoryContainer = new Panel();
        inventoryContainer.Size = new Size(GetRelativeSize(ClientSize, true, percentage: 50), GetRelativeSize(ClientSize, false, percentage: 40));
        inventoryContainer.Location = new Point(ClientSize.Width - inventoryContainer.Width, ClientSize.Height - inventoryContainer.Height);
        inventoryContainer.BackColor = Color.Aquamarine;
        Controls.Add(inventoryContainer);

        if (chapterCards.Count == 0)
        {
            for (int i = 0; i < 5; i++)
            {
                var card = new ChapterCard("Test", i + 1, new List<CardColor> {CardColor.Red, CardColor.Blue, CardColor.Green});
                Controls.Add(card.Component.Panel);
                card.Component.Panel.BringToFront();
                new DraggableControl(card.Component.Panel);
                chapterCards.Add(card);
            }
        }

        var cardWidth = GetRelativeSize(inventoryContainer.Size, true, percentage: 20);
        var cardHeight = cardWidth * 3 / 2;
        if (cardHeight > inventoryContainer.Height)
        {
            cardHeight = inventoryContainer.Height;
            cardWidth = cardHeight * 2 / 3;
        }
        foreach (var card in chapterCards)
        {
            card.Component.CardComponent.Size = new Size(cardWidth, cardHeight);
            card.Component.Panel.Location =
                inventoryContainer.Location with { X = inventoryContainer.Location.X + chapterCards.IndexOf(card) * cardWidth };
        }
        
        //Alte PictureBox entfernen
        if (figureBox != null)
        {
            figureBox.Dispose();
        }

        // Neue PictureBox erzeugen
        figureBox = new PictureBox();
        figureBox.Size = new Size(GetRelativeSize(roomImage.Size, true, 70), GetRelativeSize(roomImage.Size, false, 70));
        figureBox.BackColor = Color.Blue;
        figureBox.SizeMode = PictureBoxSizeMode.StretchImage;
        //Bild für PictureBox: figureBox.Image = Image.FromFile("pfad_zur_figur.png");
        roomImage.Controls.Add(figureBox);

        position = 0;
        SetFigurePosition();
    }
    
    private void SetFigurePosition()
    {
        var fields = Fields[GameLogic.CurrentRoom.RoomType];
        if (position >= 0 && position < fields.Count)
        {
            var point = fields[position];
            figureBox.Location = new Point(
                point.X * roomImage.Width / 1920,
                point.Y * roomImage.Width / 1920
            );
        }
    }

    private void TestKnopf()
    {
        weiterButton.Text = "Weiter";
        weiterButton.Size = new Size(100, 30);
        weiterButton.Location = new Point(10, 10);
        weiterButton.Click += figureMovement;
        this.Controls.Add(weiterButton);
    }

    private void figureMovement(object sender, EventArgs e)
    {
        position++;

        if (position >= Fields[GameLogic.CurrentRoom.RoomType].Count)
        {
            position = 0;
            GameLogic.SwitchRoom();
            CreateGameField();
            return;
        }

        SetFigurePosition();
    }

    private int GetRelativeSize(Size size, bool width, int? pixels = null, int? percentage = null)
    {
        if (pixels != null)
        {
            return pixels.Value * (width ? size.Width : size.Height) / (width ? 1920 : 1080);
        }

        if (percentage != null)
        {
            return (width ? size.Width : size.Height) / 100 * percentage.Value; 
        }
        
        throw new ArgumentException("Either pixels or percentage must be provided.");
    }

    private static Dictionary<Room.RoomName, List<Point>> Fields = new Dictionary<Room.RoomName, List<Point>>()
    {
        { Room.RoomName.HotelZimmer, new ()
        {
            new (95, 115),
            new (240, 115),
            new (370, 190),
            new (405, 305),
            new (355, 420),
            new (280, 540),
            new (230, 665),
            new (245, 795),
            new (385, 820),
            new (530, 850),
            new (675, 855),
            new (790, 750),
            new (1055, 775),
            new (1205, 780),
            new (1345, 855),
            new (1460, 765),
            new (1465, 630),
            new (1520, 495),
            new (1595, 370),
            new (1675, 250),
            new (1750, 135),
        } },
        { Room.RoomName.Hafen, new ()
        {
            new (65, 155),
            new (195, 210),
            new (310, 280),
            new (310, 400),
            new (310, 515),
            new (370, 620),
            new (495, 645),
            new (550, 760),
            new (680, 800),
            new (815, 830),
            new (1180, 810),
            new (1290, 720),
            new (1410, 680),
            new (1490, 580),
            new (1490, 470),
            new (1490, 360),
            new (1490, 250),
            new (1535, 155),
            new (1670, 140),
            new (1800, 130)
        } },
        { Room.RoomName.Stadt, new ()
        {
            new (50, 55),
            new (175, 100),
            new (220, 210),
            new (270, 330),
            new (285, 455),
            new (270, 575),
            new (355, 705),
            new (405, 825),
            new (505, 930),
            new (640, 950),
            new (765, 905),
            new (1040, 910),
            new (1170, 930),
            new (1285, 855),
            new (1395, 755),
            new (1420, 640),
            new (1550, 630),
            new (1675, 620),
            new (1785, 540)
            
        } },
        { Room.RoomName.Wald, new ()
        {
            new (50, 720),
            new (160, 695),
            new (260, 615),
            new (335, 520),
            new (450, 485),
            new (570, 470),
            new (685, 490),
            new (800, 505),
            new (1020, 540),
            new (1130, 585),
            new (1240, 630),
            new (1355, 620),
            new (1470, 580),
            new (1580, 510),
            new (1630, 410),
            new (1670, 310),
            new (1720, 215),
            new (1800, 120),
        } },
        { Room.RoomName.SafeHouse, new ()
        {
            new (110, 765),
            new (245, 805),
            new (385, 825),
            new (520, 855),
            new (665, 905),
            new (795, 880),
            new (1040, 880),
            new (1160, 875),
            new (1280, 835),
            new (1395, 755),
            new (1360, 645),
            new (1365, 510),
            new (20, 25),
            new (130, 40),
            new (160, 140),
            new (275, 155),
            new (390, 195),
            new (500, 180),
            new (580, 85),
            new (690, 60),
            new (810, 25),
            new (1035, 25),
            new (1105, 115),
            new (1220, 135),
            new (1330, 125),
            new (1440, 175),
            new (1390, 270),
            new (1360, 370),
            new (1360, 500),

        } },
    };
}