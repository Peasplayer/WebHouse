namespace WebHouse_Client;
using System.Reflection;
using WebHouse_Client.Networking;
using WebHouse_Client.Logic;

public partial class EndScreen : Form
{
    static public new Boolean WinOrLose = true;
    private PictureBox? HomeScreen;
    public PictureBox? LobbyScreen;


    public EndScreen()
    {
        this.DoubleBuffered = true;
        SetupUI();
        if (WinOrLose)
        {
            this.BackgroundImage = Image.FromStream(
                Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream("WebHouse_Client.Resources.Background_Images.Victory.png"));
        }
        else
        {
            this.BackgroundImage = Image.FromStream(
                Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream("WebHouse_Client.Resources.Background_Images.GameOver.png"));
        }

        this.BackgroundImageLayout = ImageLayout.Stretch;
        this.Width = Screen.PrimaryScreen.Bounds.Width / 2;
        this.Height = Screen.PrimaryScreen.Bounds.Height / 2;
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.StartPosition = FormStartPosition.CenterScreen;

    }

    private void SetupUI()
    {
        HomeScreen = new PictureBox()
        {
            BackColor = Color.Transparent,
            Location = new Point((int)(ClientSize.Width / 1920f * 200f), (int)(ClientSize.Height / 1080f * 1800f)),
        };
        HomeScreen.Width = (int)(ClientSize.Width / 1920f * 700);
        HomeScreen.Height = HomeScreen.Width / 23 * 10;
        HomeScreen.Image = Image.FromStream(
            Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("WebHouse_Client.Resources.Images.Zurück.png")); //bild noch ändern
        HomeScreen.SizeMode = PictureBoxSizeMode.StretchImage;
        HomeScreen.BringToFront();

        HomeScreen.Click += HomeButton_Click;
        this.Controls.Add(HomeScreen);


        LobbyScreen = new PictureBox()
        {
            BackColor = Color.Transparent,
            Location = new Point((int)(ClientSize.Width / 1920f * 5500f), (int)(ClientSize.Height / 1080f * 1800f)),
        };
        LobbyScreen.Width = (int)(ClientSize.Width / 1920f * 700f);
        LobbyScreen.Height = LobbyScreen.Width / 23 * 10;
        LobbyScreen.Image = Image.FromStream(
            Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("WebHouse_Client.Resources.Images.Zurück.png")); //bild noch ändern
        LobbyScreen.SizeMode = PictureBoxSizeMode.StretchImage;
        LobbyScreen.BringToFront();

        LobbyScreen.Click += LobbyButton_Click;
        this.Controls.Add(LobbyScreen);
    }

    private void HomeButton_Click(object sender, EventArgs e)
    {
        var form = new Form1();
        form.Show();
        this.Close();
        return;
    }

    private void LobbyButton_Click(object sender, EventArgs e)
    {

        var net = new NetworkManager();
        try
        {
            net.Connect(NetworkManager.currentUrl, NetworkManager.nickName);

        }
        catch (Exception ex)
        {
            MessageBox.Show("Fehler beim Verbinden: " + ex.Message);
        }
        var form = new Lobby();
        form.Show();
        this.Close();
    }
}