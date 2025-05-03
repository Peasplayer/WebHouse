using WebHouse_Client.Components;
namespace WebHouse_Client;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
        
        this.FormBorderStyle = FormBorderStyle.None; //kein Rand
        this.WindowState = FormWindowState.Maximized; //macht Vollbild
    }

    private void GameFormBTN_Click(object sender, EventArgs e)
    {
        GameForm gameForm = new GameForm();
        gameForm.Show();  //Zeige die neue Form
        this.Hide(); //Verstecke die aktuelle Form
    }
}