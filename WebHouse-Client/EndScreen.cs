namespace WebHouse_Client;
using System.Reflection;
using WebHouse_Client.Networking;
using WebHouse_Client.Logic;

public partial class EndScreen : Form
{
    private PictureBox? Close;
    
    //Überprüft ob gewonnen oder verloren wurde
    public EndScreen(bool win)
    {
        this.DoubleBuffered = true;
        //Wenn gewonnen wurde wird das gewimmer Bild gezeigt
        if (win)
        {
            this.BackgroundImage = Image.FromStream(
                Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream("WebHouse_Client.Resources.Background_Images.Victory.png"));
        }
        //Wenn verloren wurde wird das verlierer Bild gezeigt
        else
        {
            this.BackgroundImage = Image.FromStream(
                Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream("WebHouse_Client.Resources.Background_Images.GameOver.png"));
        }

        this.BackgroundImageLayout = ImageLayout.Stretch; //Passt das Hintergrundbild an die Größe des Fensters an
        
        //Fenstergröße: halbe Bildschirmhöhe, im 16:9-Format
        this.Height = Screen.PrimaryScreen.Bounds.Height / 2; //Startgröße
        this.Width = this.Height * 16 / 9;
        
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.StartPosition = FormStartPosition.CenterScreen;
        SetupUI(); //UI Elemente starten
    }

    //Erstellt die UI Elemente
    private void SetupUI()
    {
        //Knopf zum schließen des Fensters
        Close = new PictureBox()
        {
            BackColor = Color.Transparent,
        };
        //Größe und Position wird relativ zur Fenstergröße gesetzt
        Close.Width = (int)(ClientSize.Width / 1920f * 200f);
        Close.Height = Close.Width / 23 * 10;
        Close.Location = new Point(5, ClientSize.Height - Close.Height - 5); //Bild für den Close button wird unten links positioniert
        Close.Image = Image.FromStream(
            Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("WebHouse_Client.Resources.Images.ExitBtn.png"));
        Close.SizeMode = PictureBoxSizeMode.StretchImage;
        Close.BringToFront(); //Bringt den Knopf in den Vordergrund damit er über dem Hintergrundbild liegt

        Close.Click += Closebtn_Click; //Wenn auf den Knopf gedrückt wird passiert etwas
        this.Controls.Add(Close);
    }

    //Wenn auf den Knopf geklickt wird wird das Programm neu gestartet
    private void Closebtn_Click(object sender, EventArgs e)
    {
        Application.Restart();
        Environment.Exit(0);
    }
}