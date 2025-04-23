namespace WebHouse_Client;

public partial class GameForm : Form
{

    List<Button> GameField = new List<Button>();
    static List<Room> Rooms;
    Button weiterButton = new Button(); //testknopf
    int position = 0;
    private int roomIndex;
    static private Room currentRoom;

    public GameForm()
    {
        InitializeComponent();
        //this.FormBorderStyle = FormBorderStyle.None; //kein Rand
        this.WindowState = FormWindowState.Maximized; //macht Vollbild
        Rooms = new List<Room>
        {
            new Room(Room.RoomName.HotelZimmer, hotelFields, "Hotel.png"), //Raum wird mit den Daten gefüllt
            new Room(Room.RoomName.Hafen, HafenFields, "Hafen.png"), //Raum wird mit den Daten gefüllt
            new Room(Room.RoomName.Stadt, StadtFields, "Stadt.png"), //Raum wird mit den Daten gefüllt
            new Room(Room.RoomName.Wald, WaldFields, "Wald.png"), //Raum wird mit den Daten gefüllt
            new Room(Room.RoomName.SafeHouse, SafehouseFields, "Safehouse.png"), //Raum wird mit den Daten gefüllt
        };
        roomIndex = 0;
        CurrentRoom();
        TestKnopf();
        MarkiereAktuellesFeld();
    }

    public void CurrentRoom()
    {
        currentRoom = Rooms[roomIndex];
        CreateGamefield();
    }

    public void CreateGamefield()
    {
        foreach (Button btn in GameField) //vorhandenen Buttons entfernen und die Liste leeren
        {
            this.Controls.Remove(btn); //entfernt Buttons
            btn.Dispose(); //entfernt die Buttons aus dem Speicher (nicht nur Grafisch)
        }

        GameField.Clear();

        int[,] fields = currentRoom.Fields; //Das Aktuelle Array mit den Koordinaten
        int fieldNumber = fields.GetLength(0); //Die Anzahl der Felder rausbekommen

        for (int i = 0; i < fieldNumber; i++) //NOCH ANPASSEN (damit es bei jedem Raum passt)
        {
            Button fieldButton = new Button(); //Erstelle einen neuen Button
            fieldButton.Width = 40; //Setze die Breite des Buttons
            fieldButton.Height = 40; //Setze die Höhe des Buttons
            fieldButton.Text = (i + 1).ToString();

            //Hole die x- und y-Koordinaten
            int x = fields[i, 0];
            int y = fields[i, 1];

            //Positioniere den Button basierend auf den Koordinaten
            fieldButton.Location = new Point(x - fieldButton.Width / 2, y - fieldButton.Height / 2);

            //Füge den Button zu den Steuerelementen der Form hinzu
            this.Controls.Add(fieldButton);

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
            roomIndex++;
            CurrentRoom();
        }

        //Neues Feld markieren
        MarkiereAktuellesFeld();
    }

    private void MarkiereAktuellesFeld() //Blau = eigene Figur
    {
        GameField[position].BackColor = Color.Blue;
    }
    

    private static int[,] hotelFields = new int[20, 2] //2D Array um die exakten Koordinaten des Raumes zu speichern
    {
        { 100, 100 },
        { 200, 100 },
        { 300, 100 },
        { 400, 100 },
        { 500, 100 },
        { 600, 100 },
        { 700, 100 },
        { 800, 100 },
        { 900, 100 },
        { 1000, 100 },
        { 1100, 100 },
        { 1200, 100 },
        { 1300, 100 },
        { 1400, 100 },
        { 1500, 100 },
        { 1600, 100 },
        { 1700, 100 },
        { 1800, 100 },
        { 1900, 100 },
        { 2000, 100 },
    };
    
    private static int[,] HafenFields = new int[20, 2] //2D Array um die exakten Koordinaten des Raumes zu speichern
    {
        { 100, 200 },
        { 200, 200 },
        { 300, 200 },
        { 400, 200 },
        { 500, 200 },
        { 600, 200 },
        { 700, 200 },
        { 800, 200 },
        { 900, 200 },
        { 1000, 200 },
        { 1100, 200 },
        { 1200, 200 },
        { 1300, 200 },
        { 1400, 200 },
        { 1500, 200 },
        { 1600, 200 },
        { 1700, 200 },
        { 1800, 200 },
        { 1900, 200 },
        { 2000, 200 },
    };
    
    private static int[,] StadtFields = new int[20, 2] //2D Array um die exakten Koordinaten des Raumes zu speichern
    {
        { 100, 300 },
        { 200, 300 },
        { 300, 300 },
        { 400, 300 },
        { 500, 300 },
        { 600, 300 },
        { 700, 300 },
        { 800, 300 },
        { 900, 300 },
        { 1000, 300 },
        { 1100, 300 },
        { 1200, 300 },
        { 1300, 300 },
        { 1400, 300 },
        { 1500, 300 },
        { 1600, 300 },
        { 1700, 300 },
        { 1800, 300 },
        { 1900, 300 },
        { 2000, 300 },
    };
    
    private static int[,] WaldFields = new int[20, 2] //2D Array um die exakten Koordinaten des Raumes zu speichern
    {
        { 100, 100 },
        { 200, 100 },
        { 300, 100 },
        { 400, 100 },
        { 500, 100 },
        { 600, 100 },
        { 700, 100 },
        { 800, 100 },
        { 900, 100 },
        { 1000, 100 },
        { 1100, 100 },
        { 1200, 100 },
        { 1300, 100 },
        { 1400, 100 },
        { 1500, 100 },
        { 1600, 100 },
        { 1700, 100 },
        { 1800, 100 },
        { 1900, 100 },
        { 2000, 100 },
    };
    
    private static int[,] SafehouseFields = new int[20, 2] //2D Array um die exakten Koordinaten des Raumes zu speichern
    {
        { 100, 200 },
        { 200, 200 },
        { 300, 200 },
        { 400, 200 },
        { 500, 200 },
        { 600, 200 },
        { 700, 200 },
        { 800, 200 },
        { 900, 200 },
        { 1000, 200 },
        { 1100, 200 },
        { 1200, 200 },
        { 1300, 200 },
        { 1400, 200 },
        { 1500, 200 },
        { 1600, 200 },
        { 1700, 200 },
        { 1800, 200 },
        { 1900, 200 },
        { 2000, 200 },
    };
}