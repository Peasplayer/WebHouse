namespace WebHouse_Client;
using System.Reflection;
using WebHouse_Client.Networking;
using WebHouse_Client.Logic;

public partial class EndScreen : Form
{
    static public new Boolean WinOrLose;
    private PictureBox? Close;


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
        Close = new PictureBox()
        {
            BackColor = Color.Transparent,
            Location = new Point((int)(ClientSize.Width / 1920f * 200f), (int)(ClientSize.Height / 1080f * 1800f)),
        };
        Close.Width = (int)(ClientSize.Width / 1920f * 700);
        Close.Height = Close.Width / 23 * 10;
        Close.Image = Image.FromStream(
            Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("WebHouse_Client.Resources.Images.ExitBtn.png"));
        Close.SizeMode = PictureBoxSizeMode.StretchImage;
        Close.BringToFront();

        Close.Click += Closebtn_Click;
        this.Controls.Add(Close);
    }

    private void Closebtn_Click(object sender, EventArgs e)
    {
        Application.Restart();
        Environment.Exit(0);
    }
}