using WebHouse_Client.Components;
using System.Reflection;
using System.Drawing;
namespace WebHouse_Client;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
        BackgroundImage = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("WebHouse_Client.Resources.Background_Images.LogIn.png"));
        this.BackgroundImageLayout = ImageLayout.Stretch;
        this.Width = 1920; //Startgröße
        this.Height = 1080;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.StartPosition = FormStartPosition.CenterScreen;
        Startbtn.BackgroundImage = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("WebHouse_Client.Resources.Background_Images.Start.png"));
    }

    private void Startbtn_Click(object sender, EventArgs e)
    {
        Lobby lobby = new Lobby();
        lobby.Show();
        this.Hide();
    }
}