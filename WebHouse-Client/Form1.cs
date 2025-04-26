using WebHouse_Client.Components;
namespace WebHouse_Client;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
        var snapTargets = new List<Control>();

        var discardPile = new Panel
        {
            Size = new Size(125, 200),
            Location = new Point(10, 10),
            BackColor = Color.Pink
        };
        this.Controls.Add(discardPile);

        var EscapeCards = new Escape_cards(2, "Hotellzimmer", Color.Green);
        EscapeCards.CardPanel.Location = new Point(150, 150);
        this.Controls.Add(EscapeCards.CardPanel);
        
        snapTargets.Add(discardPile);
        new DraggableControler(EscapeCards.CardPanel, snapTargets,100);
    }

    private void GameFormBTN_Click(object sender, EventArgs e)
    {
        GameForm gameForm = new GameForm();
        gameForm.Show();  //Zeige die neue Form
        this.Hide(); //Verstecke die aktuelle Form
    }
}