using System.Net.WebSockets;
using System.Reflection;
using WebHouse_Client.Networking;

namespace WebHouse_Client;

public partial class Lobby : Form
{
    public static Lobby? Instance { get; private set; }
    
    private List<Label> _playerList = new ();
    
    public Lobby()
    {
        Instance = this;
        
        InitializeComponent();
        this.DoubleBuffered = true;
        BackgroundImage = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("WebHouse_Client.Resources.Background_Images.LobbyFertig.png")); //BackgroundImage
        this.BackgroundImageLayout = ImageLayout.Stretch;
        this.Width = Screen.PrimaryScreen.Bounds.Width / 2; //Startgröße
        this.Height = Screen.PrimaryScreen.Bounds.Height / 2;
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.StartPosition = FormStartPosition.Manual;//CenterScreen;
        
        this.FormClosing += (s, e) =>
        {
            NetworkManager.Instance.Client.Stop(WebSocketCloseStatus.NormalClosure, "Client closed");
            Application.Exit();
        };
        
        Startbtn.Size = new Size(this.ClientSize.Width / 3, this.ClientSize.Height / 8); //proportionale Größe
        //Button-Position zentriert, 80% Höhe
        Startbtn.Location = new Point((this.ClientSize.Width - Startbtn.Width) / 2, (int)(this.ClientSize.Height * 0.8));
        Startbtn.BackgroundImage = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("WebHouse_Client.Resources.Background_Images.Start.png"));
        
        RefreshPlayerList();
    }

    public void RefreshPlayerList()
    {
        if (NetworkManager.Instance.Players.Count == 0)
            return;
        
        foreach (var text in _playerList)
        {
            text.Dispose();
        }
        _playerList.Clear();

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
        
        Startbtn.Visible = NetworkManager.Instance.LocalPlayer.IsHost && NetworkManager.Instance.Players.Count > 1;
    }

    private void button1_Click(object sender, EventArgs e)
    {
        NetworkManager.Instance.Rpc.StartGame();
    }
}