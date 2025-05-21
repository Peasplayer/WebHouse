using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace WebHouse_Client;

public partial class StartScreen : Form
{
    public StartScreen()
    {
        InitializeComponent();
        this.DoubleBuffered = true;
        BackgroundImage = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("WebHouse_Client.Resources.Background_Images.StartScreen.jpg"));
        this.BackgroundImageLayout = ImageLayout.Stretch;
        this.Height = Screen.PrimaryScreen.Bounds.Height / 2;
        this.Width = this.Height * 16 / 9;
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.StartPosition = FormStartPosition.CenterScreen;
        
        Start.Size = new Size(300 * ClientSize.Width / 1920, 150 * ClientSize.Height / 1080);
        Start.Location = new Point((ClientSize.Width - Start.Size.Width) / 2, 600 * ClientSize.Height / 1080);
        Start.BackgroundImage = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("WebHouse_Client.Resources.Background_Images.Startbtn.png"));
        Start.BackgroundImageLayout = ImageLayout.Stretch;
        
        Regeln.Size = new Size(Start.Width, Start.Height);
        Regeln.Location = new Point((ClientSize.Width - Regeln.Size.Width) / 2 , 750 * ClientSize.Height / 1080);
        Regeln.BackgroundImage = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("WebHouse_Client.Resources.Background_Images.Regelnbtn.png"));
        Regeln.BackgroundImageLayout = ImageLayout.Stretch;
    }

    private void Start_Click_1(object sender, EventArgs e)
    {
        var form = new Form1();
        form.Show();
        this.Close();
    }
}