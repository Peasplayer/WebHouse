using WebHouse_Client.Components;
using WebHouse_Client.Logic;

namespace WebHouse_Client;

public partial class Form1 : Form
{
    private List<Control> snapTargets = new List<Control>();
    private Logic.ChapterCard activeChapterCard;

    public Form1()
    {
        InitializeComponent();

        this.FormBorderStyle = FormBorderStyle.None; //kein Rand
        this.WindowState = FormWindowState.Maximized; //macht Vollbild

        CreateTestCards();
        CreateChapterCardStack();
    }

    private void GameFormBTN_Click(object sender, EventArgs e)
    {
        GameForm gameForm = new GameForm();
        gameForm.Show(); //Zeige die neue Form
        this.Hide(); //Verstecke die aktuelle Form
    }

    private void CreateTestCards()
    {
        var logicEscapeCard1 = new Logic.EscapeCard(1, "Hotelzimmer", CardColor.Red);
        var logicEscapeCard2 = new Logic.EscapeCard(2, "Stadt", CardColor.Green);

        var escapeCard1 = new Components.EscapeCard(logicEscapeCard1);
        var escapeCard2 = new Components.EscapeCard(logicEscapeCard2);

        this.Controls.Add(escapeCard1.Panel);
        this.Controls.Add(escapeCard2.Panel);

        escapeCard1.Panel.Location = new Point(100, 100);
        escapeCard2.Panel.Location = new Point(100, 200);

        new DraggableControl(escapeCard1.Panel, snapTargets);
        new DraggableControl(escapeCard2.Panel, snapTargets);
    }

    private void CreateChapterCardStack()
    {
        var logicChapterCard =
            new Logic.ChapterCard("Kapitel 1", 3, new List<CardColor> { CardColor.Red, CardColor.Green });
        var chapterCard = new Components.ChapterCard(logicChapterCard);

        this.Controls.Add(chapterCard.Panel);
        chapterCard.Panel.Location = new Point(300, 200);

        snapTargets.Add(chapterCard.Panel);
    }
}