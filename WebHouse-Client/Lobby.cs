using System.Reflection;
using WebHouse_Client.Networking;

namespace WebHouse_Client;

public partial class Lobby : Form
{
    public static Lobby Instance { get; private set; }
    
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
        this.StartPosition = FormStartPosition.CenterScreen;
        
        Startbtn.Size = new Size(this.ClientSize.Width / 3, this.ClientSize.Height / 8); //proportionale Größe
        //Button-Position zentriert, 80% Höhe
        Startbtn.Location = new Point((this.ClientSize.Width - Startbtn.Width) / 2, (int)(this.ClientSize.Height * 0.8));
        Startbtn.BackgroundImage = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("WebHouse_Client.Resources.Background_Images.Start.png"));
    }

    private void button1_Click(object sender, EventArgs e)
    {
        GameForm gameForm = new GameForm();
        gameForm.Show();
        this.Hide();
    }
}