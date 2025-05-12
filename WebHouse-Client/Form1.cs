using WebHouse_Client.Components;
using WebHouse_Client.Logic;

namespace WebHouse_Client;

public partial class Form1 : Form
{
    private List<Control> snapTargets = new List<Control>();
    private Logic.ChapterCard ChapterCard;

    public Form1()
    {
        InitializeComponent();

        this.FormBorderStyle = FormBorderStyle.None; //kein Rand
        this.WindowState = FormWindowState.Maximized; //macht Vollbild
        
    }

    private void GameFormBTN_Click(object sender, EventArgs e)
    {
        GameForm gameForm = new GameForm();
        gameForm.Show(); //Zeige die neue Form
        this.Hide(); //Verstecke die aktuelle Form
    }
}