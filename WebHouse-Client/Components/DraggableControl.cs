namespace WebHouse_Client.Components;

public class DraggableControl
{
    private Control Control { get; }
    private List<Control> SnapTargets { get; }

    private static Control selectedCard = null;
    private static Color originalColor;

    public DraggableControl(Control control, List<Control> snapTargets)
    {
        Control = control;
        SnapTargets = snapTargets;

        //Wenn auf die Karte geklickt wird
        Control.Click += (sender, e) =>
        {
            if (selectedCard == null)
            {
                selectedCard = Control;
                originalColor = Control.BackColor;
                Control.BackColor = Color.LightBlue;
            }
            else if (selectedCard == Control)
            {
                //Auswahl aufheben
                Control.BackColor = originalColor;
                selectedCard = null;
            }
        };

        foreach (var target in SnapTargets)
        {
            target.Click += (sender, e) =>
            {
                if (selectedCard == null)
                    return;

                //Legt die Karte zentriert auf das Ziel
                selectedCard.Location = new Point(
                    target.Left + (target.Width - selectedCard.Width) / 2,
                    target.Top + (target.Height - selectedCard.Height) / 2
                );

                selectedCard.SendToBack(); 

                //Auswahl und Farbe zurücksetzen
                selectedCard.BackColor = originalColor;
                selectedCard = null;
            };
        }
    }
}