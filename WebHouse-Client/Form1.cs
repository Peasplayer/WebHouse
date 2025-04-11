namespace WebHouse_Client;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
        var discardPile = new Panel
        {
            Size = new Size(125, 200),
            Location = new Point(10, 10),
            BackColor = Color.Pink
        };
        this.Controls.Add(discardPile);

        int numberOfCards = 5;
        for (int i = 0; i < numberOfCards; i++)
        {
            var card = new Card(new Size(125, 200), 5, 10, Color.Blue, 2);
            card.cardPanel.Location = new Point(200 + i * 140, 100); //Erzeugt Abstand zwischen den Karten
            this.Controls.Add(card.cardPanel);
            new DraggableControler(card.cardPanel, discardPile, 200);
        }
    }
}