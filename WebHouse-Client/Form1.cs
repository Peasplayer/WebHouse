using WebHouse_Client.Components;
using WebHouse_Client.Logic;
using WinFormsTest;

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
        
        var LogicChapterCard = new Logic.ChapterCard("Kappitle 1", 3, new List<CardColor>{CardColor.Red,CardColor.Blue,CardColor.Green});
        var ChapterCard = new Components.ChapterCard(LogicChapterCard);
        ChapterCard.Panel.Location = new Point(100, 100);
        this.Controls.Add(ChapterCard.Panel);
        
        var logicEscape = new Logic.EscapeCard(1, "Keller", CardColor.Blue);
        var escapeCard = new Components.EscapeCard(logicEscape);
        escapeCard.Panel.Location = new Point(300, 100);
        this.Controls.Add(escapeCard.Panel);
    }

    private void GameFormBTN_Click(object sender, EventArgs e)
    {
        GameForm gameForm = new GameForm();
        gameForm.Show(); //Zeige die neue Form
        this.Hide(); //Verstecke die aktuelle Form
    }
}