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
        
        this.Closing += (_, _) =>
        {
            Application.Exit();
        };
        
        Start.Size = new Size(300 * ClientSize.Width / 1920, 150 * ClientSize.Height / 1080);
        Start.Location = new Point((ClientSize.Width - Start.Size.Width) / 2, 600 * ClientSize.Height / 1080);
        Start.BackgroundImage = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("WebHouse_Client.Resources.Background_Images.Startbtn.png"));
        Start.BackgroundImageLayout = ImageLayout.Stretch;
        Start.MouseClick += (_, _) =>
        {
            var form = new Menu();
            form.Location = this.Location;
            form.Show();
            this.Hide();
        };
        
        Regeln.Size = new Size(Start.Width, Start.Height);
        Regeln.Location = new Point((ClientSize.Width - Regeln.Size.Width) / 2 , 750 * ClientSize.Height / 1080);
        Regeln.BackgroundImage = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("WebHouse_Client.Resources.Background_Images.Regelnbtn.png"));
        Regeln.BackgroundImageLayout = ImageLayout.Stretch;
        Regeln.MouseClick += (_, _) =>
        {
            var form = new Rules();
            form.Location = this.Location;
            form.Show();
            this.Hide();
        };

        var infoButton = new Button();
        Controls.Add(infoButton);

        infoButton.BackgroundImage = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("WebHouse_Client.Resources.Background_Images.Moses.png"));
        infoButton.BackgroundImageLayout = ImageLayout.Stretch;
        infoButton.BackColor = Color.Transparent;
        infoButton.FlatStyle = FlatStyle.Popup;
        infoButton.Size = new Size(Start.Width, Start.Height);
        infoButton.Location = new Point((ClientSize.Width - Regeln.Size.Width) / 2 , 900 * ClientSize.Height / 1080);
        infoButton.MouseClick += (_, _) => 
            MessageBox.Show("Das Spiel \"Sebastian Fitzek - Safehouse\" ist Eigentum des moses. Verlag. Dieses Programm ist ein Schulprojekt ohne jeglichen kommerziellen Nutzen." + 
                            "\n\nDanke an moses. für das bereitstellen der originalen Bilder! (Rechte gehören dem Verlag)." +
                            "\n\nTom Störmer, Jannis Edler, Alexander Deschner, Joschua Kux, Lennox Frank von der JPRS Friedberg");
    }
}