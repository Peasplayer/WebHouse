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
        snapTargets.Add(discardPile); 
        
        var drawPile = new Panel
        {
            Size = new Size(125, 200),
            Location = new Point(10, 250),
            BackColor = Color.LightGreen
        };
        this.Controls.Add(drawPile);
        snapTargets.Add(drawPile);

        int numberOfCards = 5;
        for (int i = 0; i < numberOfCards; i++)
        {
            var card = new Card(new Size(125, 200), 5, 10, Color.Blue, 2);
            card.cardPanel.Location = new Point(200 + i * 140, 100);
            this.Controls.Add(card.cardPanel);
            
            new DraggableControler(card.cardPanel, snapTargets, 200);
        }
    }
}