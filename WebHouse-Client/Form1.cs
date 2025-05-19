using WebHouse_Client.Components;
using System.Reflection;
using System.Drawing;
namespace WebHouse_Client;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
        this.DoubleBuffered = true;
        BackgroundImage = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("WebHouse_Client.Resources.Background_Images.LogIn.png"));
        this.BackgroundImageLayout = ImageLayout.Stretch;
        this.Width = Screen.PrimaryScreen.Bounds.Width / 2; //Startgröße
        this.Height = Screen.PrimaryScreen.Bounds.Height / 2;
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.StartPosition = FormStartPosition.CenterScreen;
        
        Startbtn.Size = new Size(500 * Width / 1920, 125 * Height / 1080);
        Startbtn.Location = new Point((Width - Startbtn.Size.Width) / 2, 800 * Height / 1080);
        Startbtn.BackgroundImage = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("WebHouse_Client.Resources.Background_Images.Start.png"));

        textBox1.Size = new Size(500 * Width / 1920, 125 * Height / 1080);
        textBox1.Location = new Point(900 * Width / 1920, 180 * Height / 1080);
        
        textBox2.Size = new Size(500 * Width / 1920, 125 * Height / 1080);
        textBox2.Location = new Point(900 * Width / 1920, 325 * Height / 1080);
    }

    private void Startbtn_Click(object sender, EventArgs e)
    {
        if (textBox1.Text == "" || textBox2.Text == "")
        {
            MessageBox.Show("Bitte gebe eine IP und einen Namen an!", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        
        Lobby lobby = new Lobby();
        lobby.Show();
        this.Hide();
    }
}