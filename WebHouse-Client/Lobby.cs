using WebHouse_Client.Components;
using System.Reflection;
using System.Drawing;

namespace WebHouse_Client;

public partial class Lobby : Form
{
    private float aspectRatio;

    public Lobby()
    {
        InitializeComponent();
        BackgroundImage = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("WebHouse_Client.Resources.Background_Images.LobbyFertig.png")); //BackgroundImage
        this.BackgroundImageLayout = ImageLayout.Stretch;
        this.Width = 1920; //Startgröße
        this.Height = 1080;
        this.MaximizeBox = false;
        aspectRatio = (float)this.Width / this.Height; //Seitenverhältnis berechnen
        this.ResizeEnd += Lobby_ResizeEnd; //Größe ändern Event
        UpdateStartButtonLayout();
        Startbtn.BackgroundImage = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("WebHouse_Client.Resources.Background_Images.Start.png"));
    }

    private void Lobby_ResizeEnd(object sender, EventArgs e)
    {
        // Nur Höhe anpassen, damit das Seitenverhältnis bleibt
        int newHeight = (int)(this.Width / aspectRatio);
        this.Size = new Size(this.Width, newHeight);
        UpdateStartButtonLayout();
    }

    private void UpdateStartButtonLayout()
    {
        Startbtn.Size = new Size(this.ClientSize.Width / 3, this.ClientSize.Height / 8); //proportionale Größe
        //Button-Position zentriert, 80% Höhe
        int x = (this.ClientSize.Width - Startbtn.Width) / 2; //mittig
        int y = (int)(this.ClientSize.Height * 0.8);
        Startbtn.Location = new Point(x, y);
    }

    private void button1_Click(object sender, EventArgs e)
    {
        GameForm gameForm = new GameForm();
        gameForm.Show();
        this.Hide();
    }
}