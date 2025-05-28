using System.Reflection;
using WebHouse_Client.Networking;

namespace WebHouse_Client;

public partial class Menu : Form
{
    public Menu()
    {
        InitializeComponent(); 
        this.DoubleBuffered = true;
        //Setzt das Hintergrundbild des Menüs
        BackgroundImage = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("WebHouse_Client.Resources.Background_Images.LogIn.png"));
        this.BackgroundImageLayout = ImageLayout.Stretch;
        
        //Fenstergröße im verhältnis zur Bildschirmgröße
        this.Height = Screen.PrimaryScreen.Bounds.Height / 2; //Startgröße
        this.Width = this.Height * 16 / 9;
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.StartPosition = FormStartPosition.Manual;
        //Wenn das Fenster geschlossen wird wird das Startbildschir wieder angezeigt
        this.Closing += (_, _) =>
        {
            var form = new StartScreen();
            form.Show();
        };
        
        //Die Größe und die Position der UI Elemente werden relativ zur Fenstergröße gesetzt
        Startbtn.Size = new Size(500 * ClientSize.Width / 1920, 125 * ClientSize.Height / 1080);
        Startbtn.Location = new Point((ClientSize.Width - Startbtn.Size.Width) / 2, 800 * ClientSize.Height / 1080);
        Startbtn.BackgroundImage = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("WebHouse_Client.Resources.Background_Images.Start.png"));

        //Textbox für den namen
        textBox1.Size = new Size(500 * ClientSize.Width / 1920, 125 * ClientSize.Height / 1080); //Höhe wird durch den Font überschrieben
        textBox1.Location = new Point(900 * ClientSize.Width / 1920, 194 * ClientSize.Height / 1080);
        textBox1.Font = new System.Drawing.Font("Segoe UI", 75F * ClientSize.Height / 1080, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
        
        //Textbox für die IP
        textBox2.Size = new Size(500 * ClientSize.Width / 1920, 125 * ClientSize.Height / 1080); //Höhe wird durch den Font überschrieben
        textBox2.Location = new Point(900 * ClientSize.Width / 1920, 347 * ClientSize.Height / 1080);
        textBox2.Font = new System.Drawing.Font("Segoe UI", 75F * ClientSize.Height / 1080, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
    }

    //Wenn auf den Start Button geklickt wird wird eine Verbindung zum Server aufgebaut
    private void Startbtn_Click(object sender, EventArgs e)
    {
        if (textBox1.Text.Trim() == "" || textBox2.Text.Trim() == "") 
        {
            MessageBox.Show("Bitte gebe eine IP und einen Namen an!", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        //Überprüft ob der Name nur Buchstaben und Zahlen enthält und ob er nicht länger als 12 Zeichen ist
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
                if (!net.Client.IsRunning)
                    return;
                
                Lobby lobby = new Lobby();
                Lobby.Instance = lobby;
                lobby.Location = this.Location;
                lobby.Show();
                this.Hide();
            });
        });
    }
}