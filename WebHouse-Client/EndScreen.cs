namespace WebHouse_Client;
using System.Reflection;
using WebHouse_Client.Networking;
using WebHouse_Client.Logic;

public partial class EndScreen : Form
{
    private PictureBox? Close;
    
    public EndScreen(bool win)
    {
        this.DoubleBuffered = true;
        if (win)
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
        this.Height = Screen.PrimaryScreen.Bounds.Height / 2; //Startgröße
        this.Width = this.Height * 16 / 9;
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.StartPosition = FormStartPosition.CenterScreen;
        SetupUI();
    }

    private void SetupUI()
    {
        Close = new PictureBox()
        {
            BackColor = Color.Transparent,
        };
        Close.Width = (int)(ClientSize.Width / 1920f * 200f);
        Close.Height = Close.Width / 23 * 10;
        Close.Location = new Point(5, ClientSize.Height - Close.Height - 5);
        /*Close.Width = (int)(ClientSize.Width / 1920f * 700);
        Close.Height = Close.Width / 23 * 10;*/
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