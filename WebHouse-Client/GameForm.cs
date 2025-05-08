using System.Reflection;
using WebHouse_Client.Logic;

namespace WebHouse_Client;

public partial class GameForm : Form
{
    List<Button> GameField = new List<Button>();
    Button weiterButton = new Button(); //testknopf
    int position = 0;
    private PictureBox? roomImage;

    //Temporär zum erstellen der Felder
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        switch (keyData)
        {
            case Keys.Left:
            {
                x -= 5;
                slot.Location = new Point(x * roomImage.Width / 1920, y * roomImage.Width / 1920);
                break;
            }
            case Keys.Right:
            {
                x += 5;
                slot.Location = new Point(x * roomImage.Width / 1920, y * roomImage.Width / 1920);
                break;
            }
            case Keys.Up:
            {
                y -= 5;
                slot.Location = new Point(x * roomImage.Width / 1920, y * roomImage.Width / 1920);
                break;
            }
            case Keys.Down:
            {
                y += 5;
                slot.Location = new Point(x * roomImage.Width / 1920, y * roomImage.Width / 1920);
                break;
            }
            case Keys.Enter:
            {
                Console.WriteLine("new (" + x + ", " + y + "),");
                break;
            }
        }

        return base.ProcessCmdKey(ref msg, keyData);
    }
    
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

        //Hintergrundbild setzen
        PictureBox pictureBox = new PictureBox();
        pictureBox.Image = image;
        pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
        pictureBox.Width = GetPercentage(true, 60);
        pictureBox.Height = GetPercentage(true, 60) * 9 / 16;
        pictureBox.Location = new Point(this.ClientSize.Width - pictureBox.Width, 0);
        this.Controls.Add(pictureBox);
        roomImage = pictureBox;

        //Alte PictureBox entfernen
        if (figureBox != null)
        {
            figureBox.Dispose();
        }

        // Neue PictureBox erzeugen
        figureBox = new PictureBox();
        figureBox.Size = new Size(50, 50);
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

    private int GetPercentage(bool width, int percentage)
    {
        return (width ? this.ClientSize.Width : this.ClientSize.Height) / 100 * percentage; 
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

    private int x;
    private int y;
    private Panel slot;
    private PictureBox figureBox;
}