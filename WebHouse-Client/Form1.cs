using WebHouse_Client.Components;
using System.Reflection;
using System.Drawing;
namespace WebHouse_Client;

public partial class Form1 : Form
{
    public Form1()
    {
        BackgroundImage = Image.FromFile("C:\\Users\\DuBis\\RiderProjects\\WebHouseLennox\\WebHouse-Client\\Resources\\Background_Images\\LoginBackground.jpeg");
        //string path = Path.Combine(Application.StartupPath, "Resources", "Background_Images", "LoginBackground.jpeg");
        //Image image = Image.FromStream(
            //Assembly.GetExecutingAssembly().GetManifestResourceStream("WebHouse_Client.Resources.Background_Images.LoginBackground.jpeg"));
        //string path = Path.Combine(Application.StartupPath, "Resources", "Background_Images", "LoginBackground.jpeg");
        //this.FormBorderStyle = FormBorderStyle.None; //kein Rand
        //this.WindowState = FormWindowState.Maximized; //macht Vollbild
        //Assembly.GetExecutingAssembly().GetManifestResourceStream
            //("WebHouse_Client.Resources.Background_Images.LoginBackground.jpeg");
        InitializeComponent();
        this.Width = 1920;
        this.Height = 1080;
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