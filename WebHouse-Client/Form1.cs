using WebHouse_Client.Components;
using System.Reflection;
using System.Drawing;
namespace WebHouse_Client;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
        BackgroundImage = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("WebHouse_Client.Resources.Background_Images.LoginBackground.jpeg"));
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;
        this.ClientSize = new Size(1920, 1080);
    }

    private void GameFormBTN_Click(object sender, EventArgs e)
    {
        Lobby Lobby = new Lobby();
        Lobby.Show();  //Zeige die neue Form
        this.Hide(); //Verstecke die aktuelle Form
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        //throw new System.NotImplementedException();
    }
}