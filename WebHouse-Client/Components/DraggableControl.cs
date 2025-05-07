namespace WebHouse_Client.Components;

public class DraggableControl
{
    public const int SnapRadius = 200; //Radius in dem die Karte an einen Stapel angeheftet wird

    private Control Control { get; }
    private bool _isDragging;
    private Point _startLocation;
    private Control? _highlightTarget; //Der Stapel der farblich hervorgehoben wir da eine Karte in seiner nähe ist
    private Color _originalColor;

    public DraggableControl(Control control, List<Control> snapTargets)
    {
        Control = control;

        //Wird ausgeführt wenn die Maustaste gedrückt wird
        Control.MouseDown += (sender, e) =>
        {
            if (e.Button == MouseButtons.Left)
            {
                _isDragging = true;
                _startLocation = e.Location;
                Control.BringToFront(); //Karte wird in den Vordergrund gebracht damit sie nicht hinter einem Stable verschwindet
                
                //Entfernt die ausgewählte Karten aus dem Dictionary in CardManager
                foreach(var group in CardManager.CardGroup)
                {
                    if (group.Value.Contains(Control))
                    {
                        group.Value.Remove(Control); //Karte wird aus dem Stapel entfernt
                        break;
                    }
                }
            }
        };
        
        //Wird ausgeführt wenn die Maustaste losgelassen wird
        Control.MouseUp += (sender, e) =>
        {
            if (!_isDragging)
            {
                return; //Wenn die Karte nicht bewegt wird wird nichts gemacht
            } 
            _isDragging = false;

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
                if (distance <= SnapRadius * SnapRadius && distance < closestDistance)
                {
                    closestDistance = distance; //Neuer kleinster Wert
                    closest = target; //Ziel wird gespeichert
                }
            }

            //Wenn ein Ziel gefunden wurde wird die Karte in die Mitte von dessem gesetzt
            if (closest != null)
            {
                if (!CardManager.CardGroup.ContainsKey(closest))
                    CardManager.CardGroup[closest] = new List<Control>();

                var group = CardManager.CardGroup[closest];
                group.Add(Control);

                // Position berechnen (horizontal nebeneinander)
                int index = group.Count - 1;
                int spacing = 10; // Abstand zwischen den Karten
                int startX = closest.Left + (closest.Width - (Control.Width * group.Count + spacing * (group.Count - 1))) / 2;

                Control.Location = new Point(
                    startX + index * (Control.Width + spacing),
                    closest.Top + (closest.Height - Control.Height) / 2
                );

                Control.BringToFront();//Karte wird in den Vordergrund gebracht damit sie nicht hinter einem Stable verschwindet
            }
            if (_highlightTarget != null)
            {
                _highlightTarget.BackColor = _originalColor;
                _highlightTarget = null;
            }
        };
        
        //Wir aufgerufen wen eine Karte bewegt wird
        Control.MouseMove += (sender, e) =>
        {
            if (_isDragging)
            {
                //Karte bewegen
                var location = Control.Location;
                location.Offset(e.Location.X - _startLocation.X, e.Location.Y - _startLocation.Y);
                Control.Location = location;

                Control nearestTarget = null;
                
                double nearestDistance = double.MaxValue;

                //Suche den nächsten Stapel der im Snapradius ist
                foreach (var target in snapTargets)
                {
                    var dx = (target.Left + target.Width / 2) - (Control.Left + Control.Width / 2);
                    var dy = (target.Top + target.Height / 2) - (Control.Top + Control.Height / 2);
                    double distance = Math.Sqrt(dx * dx + dy * dy);

                    if (distance <= SnapRadius && distance < nearestDistance)
                    {
                        nearestDistance = distance;
                        nearestTarget = target;
                        
                    }
                }

                //Wenn sich das Snap Ziel geändert hat
                if (nearestTarget != _highlightTarget)
                {
                    //Der alte Stapel wird zurückgefärbt
                    if (_highlightTarget != null)
                    {
                        _highlightTarget.BackColor = _originalColor;
                    }

                    //Stapel wird gelb gefärbt
                    if (nearestTarget != null)
                    {
                        _originalColor = nearestTarget.BackColor;  
                        nearestTarget.BackColor = Color.Yellow; 
                    }
                    _highlightTarget = nearestTarget;
                }
            }
        };
    }
}