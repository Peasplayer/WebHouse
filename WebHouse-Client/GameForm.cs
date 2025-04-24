using System.Reflection;
using WebHouse_Client.Logic;

namespace WebHouse_Client;

public partial class GameForm : Form
{
    List<Button> GameField = new List<Button>();
    Button weiterButton = new Button(); //testknopf
    int position = 0;
    private PictureBox? roomImage;

    // Temporär zum erstellen der Felder
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
                x = y = 0;
                slot.Location = new Point(x * roomImage.Width / 1920, y * roomImage.Width / 1920);
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
        MarkiereAktuellesFeld();
        
        GameLogic.Start();
    }

    public void CreateGameField()
    {
        //bild laden
        Image image = Image.FromStream(
            Assembly.GetExecutingAssembly().GetManifestResourceStream("WebHouse_Client.Resources.Background_Images.Hotel.jpg"));// + GameLogic.CurrentRoom.Picture));

        if (roomImage != null)
        {
            roomImage.Dispose();
        }
        
        //pictureBox erstellen
        PictureBox pictureBox = new PictureBox();
        pictureBox.Image = image;
        pictureBox.SizeMode = PictureBoxSizeMode.Zoom;

        //Größe auf ein Viertel des Fensters setzen
        pictureBox.Width = GetPercentage(true, 60);
        pictureBox.Height = GetPercentage(true, 60) * 9 / 16;

        //Oben rechts positionieren
        pictureBox.Location = new Point(this.ClientSize.Width - pictureBox.Width, 0);

        Panel panel = new Panel();
        panel.Size = new Size(70 * pictureBox.Width / 1920, 70 * pictureBox.Height / 1080);
        panel.Location = new Point(x * pictureBox.Width / 1920, y * pictureBox.Width / 1920);
        panel.BackColor = Color.Fuchsia;
        pictureBox.Controls.Add(panel);
        pictureBox.BringToFront();
        slot = panel;
        
        //PictureBox zur Form hinzufügen
        this.Controls.Add(pictureBox);
        roomImage = pictureBox;
        
        foreach (Button btn in GameField) //vorhandenen Buttons entfernen und die Liste leeren
        {
            this.Controls.Remove(btn); //entfernt Buttons
            btn.Dispose(); //entfernt die Buttons aus dem Speicher (nicht nur Grafisch)
        }

        GameField.Clear();

        var fields = Fields[GameLogic.CurrentRoom.RoomType];
        foreach (var point in fields)
        {
            Button fieldButton = new Button(); //Erstelle einen neuen Button
            //fieldButton.Width = 40; //Setze die Breite des Buttons
            //fieldButton.Height = 40; //Setze die Höhe des Buttons
            fieldButton.Size = new Size(70 * pictureBox.Width / 1920, 70 * pictureBox.Height / 1080);
            fieldButton.Text = (fields.IndexOf(point) + 1).ToString();

            //Positioniere den Button basierend auf den Koordinaten
            //fieldButton.Location = new Point(point.X - fieldButton.Width / 2, point.Y - fieldButton.Height / 2);
            fieldButton.Location = new Point(point.X * pictureBox.Width / 1920, point.Y * pictureBox.Width / 1920);
            fieldButton.BringToFront();
            
            //Füge den Button zu den Steuerelementen der Form hinzu
            roomImage.Controls.Add(fieldButton);

            GameField.Add(fieldButton); //Fügt das Feld der Liste Hinzu
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
        //Aktuelles Feld zurücksetzen
        GameField[position].BackColor = SystemColors.Control;

        //Position erhöhen
        position++;
        if (position >= GameField.Count)
        {
            position = 0; //Zurück zum Anfang
            GameLogic.SwitchRoom();
            CreateGameField();
        }

        //Neues Feld markieren
        MarkiereAktuellesFeld();
    }

    private void MarkiereAktuellesFeld() //Blau = eigene Figur
    {
        GameField[position].BackColor = Color.Blue;
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
            new (100, 200),
            new (200, 200),
            new (300, 200),
            new (400, 200),
            new (500, 200),
            new (600, 200),
            new (700, 200),
            new (800, 200),
            new (900, 200),
            new (1000, 200),
            new (1100, 200),
            new (1200, 200),
            new (1300, 200),
            new (1400, 200),
            new (1500, 200),
            new (1600, 200),
            new (1700, 200),
            new (1800, 200),
            new (1900, 200),
            new (2000, 200)
        } },
        { Room.RoomName.Stadt, new ()
        {
            new (100, 300),
            new (200, 300),
            new (300, 300),
            new (400, 300),
            new (500, 300),
            new (600, 300),
            new (700, 300),
            new (800, 300),
            new (900, 300),
            new (1000, 300),
            new (1100, 300),
            new (1200, 300),
            new (1300, 300),
            new (1400, 300),
            new (1500, 300),
            new (1600, 300),
            new (1700, 300),
            new (1800, 300),
            new (1900, 300)
            
        } },
        { Room.RoomName.Wald, new ()
        {
            new (100, 400),
            new (200, 400),
            new (300, 400),
            new (400, 400),
            new (500, 400),
            new (600, 400),
            new (700, 400),
            new (800, 400),
            new (900, 400),
            new (1000, 400),
            new (1100, 400),
            new (1200, 400),
            new (1300, 400),
            new (1400, 400),
            new (1500, 400),
            new (1600, 400),
            new (1700, 400),
            new (1800, 400)
        } },
        { Room.RoomName.SafeHouse, new ()
        {
            new (100, 500),
            new (200, 500),
            new (300, 500),
            new (400, 500),
            new (500, 500),
            new (600, 500),
            new (700, 500),
            new (800, 500),
            new (900, 500),
            new (1000, 500),
            new (1100, 500),
            new (1200, 500),
            new (1300, 500),
            new (1400, 500),
            new (1500, 500),
            new (1600, 500),
            new (1700, 500),
            new (1800, 500),
            new (1900, 500),
            new (2000, 500)
        } },
    };

    private int x;
    private int y;
    private Panel slot;
}