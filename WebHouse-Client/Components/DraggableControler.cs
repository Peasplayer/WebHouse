namespace WebHouse_Client.Components;

public class DraggableControler
{
    public Control Control { get; }
    private bool isDragging;
    private Point startLocation;
    private int ZIndex;
    private Control snapTarget;
    private int snapRadius;
    private List<Control> snapTargets;

    public DraggableControler(Control control, List<Control> snapTargets, int snapRadius)
    {
        Control = control;
        this.snapTargets = snapTargets;
        this.snapRadius = snapRadius;

        //Wird ausgeführt wenn die Maustase gedrückt wird
        Control.MouseDown += (sender, e) =>
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                startLocation = e.Location;
                ZIndex = Control.Parent.Controls.GetChildIndex(Control); //Z-Index der Karte merken
                Control.BringToFront(); //Karte wird in den Vordergrund gebracht damit sie nicht hinter einem Stable verschwindet
            }
        };
        
        //Wird ausgeführt wenn die Maustaste losgelassen wird
        Control.MouseUp += (sender, e) =>
        {
            if (!isDragging)
            {
                return; //Wenn die Karte nicht bewegt wird wird nichts gemacht
            } 
            isDragging = false;

            Control closest = null;
            double closestDistance = double.MaxValue; //Nutzt die größte mögliche Distanz damit jeder andere Wert immer kleiner ist

            //Überprüft alle möglichen Snapp Ziele
            foreach (var target in snapTargets)
            {
                //Berechnet die Distantz zum Mittelpunkt des Ziels
                var dx = (target.Left + target.Width / 2) - (Control.Left + Control.Width / 2);
                var dy = (target.Top + target.Height / 2) - (Control.Top + Control.Height / 2);
                double distance = dx * dx + dy * dy; //Abstand wird quadriert um die Distanz herauszufinden (Satz des Pythagoras)

                //Überprüft ob die Karte in den Snap Radius ist und ob die Distanz kleiner ist als die vorherige
                if (distance <= snapRadius * snapRadius && distance < closestDistance)
                {
                    closestDistance = distance; //Neuer kleinster Wert
                    closest = target; //Ziel wird gespeichert
                }
            }

            //Wenn ein Ziel gefunden wurde wird die Karte in die Mitte von dessem gesetzt
            if (closest != null)
            {
                Control.Location = new Point(
                    closest.Left + (closest.Width - Control.Width) / 2,
                    closest.Top + (closest.Height - Control.Height) / 2
                );
                Control.BringToFront(); //Karte wird in den Vordergrund gebracht damit sie nicht hinter einem Stable verschwindet
            }
        };
        
        Control.MouseMove += (sender, e) =>
        {
            if (isDragging)
            {
                var location = Control.Location;
                location.Offset(e.Location.X - startLocation.X, e.Location.Y - startLocation.Y);
                Control.Location = location;
            }
        };
    }

    private double Distance(Point p1, Point p2)
    {
        int dx = p1.X - p2.X;
        int dy = p1.Y - p2.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }
}