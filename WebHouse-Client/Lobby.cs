namespace WebHouse_Client;

public partial class Lobby : Form
{
    public Lobby()
    {
        InitializeComponent();
        BackgroundImage = Image.FromFile("C:\\Users\\DuBis\\RiderProjects\\WebHouseLennox\\WebHouse-Client\\Resources\\Background_Images\\Lobby.jpg");
        this.Width = 1920;
        this.Height = 1080;
    }

    private void button1_Click(object sender, EventArgs e)
    {
        GameForm gameForm = new GameForm();
        gameForm.Show();  //Zeige die neue Form
        this.Hide(); //Verstecke die aktuelle Form
    }
}