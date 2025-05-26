using System.Net.WebSockets;
using System.Reflection;
using WebHouse_Client.Networking;

namespace WebHouse_Client;

public partial class Lobby : Form
{
    public static Lobby? Instance { get; set; }

    public bool ClosedByGame = false; 
    private List<Label> _playerList = new (); //List aller Spieler in der Lobby
    
    public Lobby()
    {
        Instance = this;
        
        InitializeComponent(); 
        this.DoubleBuffered = true;
        //Setzt die Hintergrundfarbe der Lobby
        BackgroundImage = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("WebHouse_Client.Resources.Background_Images.LobbyFertig.png")); //BackgroundImage
        this.BackgroundImageLayout = ImageLayout.Stretch;
        this.Height = Screen.PrimaryScreen.Bounds.Height / 2;//Startgröße
        this.Width = this.Height * 16 / 9; 
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.StartPosition = FormStartPosition.Manual;//CenterScreen;
        
        //Wird aufgerufen wenn das Fenster geschlossen wird
        this.FormClosing += (s, e) =>
        {
            if (ClosedByGame)
                return;
            
            NetworkManager.Instance.Client.Stop(WebSocketCloseStatus.NormalClosure, "Client closed"); //Stoppt den Client und schließt die Verbindung zum Server
            
            //Zeigt das Menü wirder an
            var form = new Menu();
            form.StartPosition = FormStartPosition.Manual;
            form.Location = Location;
            form.Show();
        };
        
        Startbtn.Size = new Size(this.ClientSize.Width / 3, this.ClientSize.Height / 8); //proportionale Größe
        
        //Button-Position zentriert, 80% Höhe
        Startbtn.Location = new Point((this.ClientSize.Width - Startbtn.Width) / 2, (int)(this.ClientSize.Height * 0.8));
        Startbtn.BackgroundImage = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("WebHouse_Client.Resources.Background_Images.Start.png"));
        
        RefreshPlayerList(); //Spieleranzeige aktualisieren
    }

    //Aktualisiert die Spieleranzeige in der Lobby
    public void RefreshPlayerList()
    {
        //Wenn keine Spieler in der Lobby sind wird nichts gemacht
        if (NetworkManager.Instance.Players.Count == 0)
            return;
        
        foreach (var text in _playerList)
        {
            text.Dispose();
        }
        _playerList.Clear();

        //Für jeden Spieler in der Lobby wir ein label erstellt. In diesem Label wird der Name des Spielers angezeigt und ob er Host ist.
        foreach (var player in NetworkManager.Instance.Players)
        {
            var text = new Label();
            text.BackColor = Color.FromArgb(169, 145, 101);
            text.ForeColor = player.IsHost ? Color.DarkRed : Color.Black;
            text.Text = player.Name + (player.IsHost ? " [Host]" : "") + (player.Id == NetworkManager.Instance.Id ? " [Du]" : "");
            text.UseCompatibleTextRendering = true;
            text.Font = new Font(Program.Font, 20F * ClientSize.Height / 1080);
            text.Size = TextRenderer.MeasureText(string.Concat(Enumerable.Repeat("M", 24)), text.Font);
            text.Location = new Point(ClientSize.Width / 4, ClientSize.Height / 3 + 
                (int)(text.Size.Height * 1.2 * NetworkManager.Instance.Players.IndexOf(player)));
            Controls.Add(text);
            _playerList.Add(text);
        }
        
        Startbtn.Visible = NetworkManager.Instance.LocalPlayer == null ? false : NetworkManager.Instance.LocalPlayer.IsHost && NetworkManager.Instance.Players.Count > 1;
    }

    //Button zum Starten des Spiels
    private void button1_Click(object sender, EventArgs e)
    {
        NetworkManager.Rpc.StartGame();
    }
}