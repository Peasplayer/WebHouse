using System.Reflection;
using WebHouse_Client.Networking;

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
        
        Startbtn.Size = new Size(500 * ClientSize.Width / 1920, 125 * ClientSize.Height / 1080);
        Startbtn.Location = new Point((ClientSize.Width - Startbtn.Size.Width) / 2, 800 * ClientSize.Height / 1080);
        Startbtn.BackgroundImage = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("WebHouse_Client.Resources.Background_Images.Start.png"));

        textBox1.Size = new Size(500 * ClientSize.Width / 1920, 125 * ClientSize.Height / 1080); //Höhe wird durch den Font überschrieben
        textBox1.Location = new Point(900 * ClientSize.Width / 1920, 194 * ClientSize.Height / 1080);
        textBox1.Font = new System.Drawing.Font("Segoe UI", 75F * ClientSize.Height / 1080, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);

        
        textBox2.Size = new Size(500 * ClientSize.Width / 1920, 125 * ClientSize.Height / 1080); //Höhe wird durch den Font überschrieben
        textBox2.Location = new Point(900 * ClientSize.Width / 1920, 347 * ClientSize.Height / 1080);
        textBox2.Font = new System.Drawing.Font("Segoe UI", 75F * ClientSize.Height / 1080, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);

    }

    private void Startbtn_Click(object sender, EventArgs e)
    {
        if (textBox2.Text == "debug")
        {
            var form = new GameForm();
            form.Show();
            this.Hide();
            return;
        }

        if (textBox1.Text == "" || textBox2.Text == "") 
        {
            MessageBox.Show("Bitte gebe eine IP und einen Namen an!", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (textBox1.Text.Length > 12)
        {
            MessageBox.Show("Der Name darf nur 12 Zeichen lang sein!", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (textBox1.Text.Contains('[') || textBox1.Text.Contains(']') )
        {
            MessageBox.Show("Der Name darf weder '[' noch ']' enthalten!", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        Startbtn.Enabled = false;
        
        // Der Client verbindet sich mit dem Server und gibt seinen Namen an
        // Als Task wird so nicht das öffnen der Forms blockiert
        Task.Run(() =>
        {
            var net = new NetworkManager();
            try
            {
                net.Connect("ws://" + (textBox2.Text == "fox" ? "fox.peasplayer.xyz" : textBox2.Text) + ":8443", textBox1.Text);
            }
            catch (Exception _)
            {
                MessageBox.Show("Keine Verbindung zum Server möglich. Ist die IP-Addresse korrekt?", "Fehler",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                this.BeginInvoke(() =>
                {
                    Startbtn.Enabled = true;
                });
            }

            this.BeginInvoke(() =>
            {
                Lobby lobby = new Lobby();
                lobby.Location = this.Location;
                lobby.Show();
                this.Hide();
            });
        });
    }
}