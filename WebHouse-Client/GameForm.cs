namespace WebHouse_Client;

public partial class GameForm : Form
{
    public GameForm()
    {
        InitializeComponent();
        this.FormBorderStyle = FormBorderStyle.None; //kein Rand
        this.WindowState = FormWindowState.Maximized; //macht Vollbild
    }
}