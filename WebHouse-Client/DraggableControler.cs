namespace WebHouse_Client;

public class DraggableControler
{
    public Control Control { get; }
    private bool isDragging;
    private Point startLocation;
    private int ZIndex;
    private Control snapTarget;
    private int snapRadius;

    public DraggableControler(Control control, Control snapTarget, int snapRadius)
    {
        Control = control;
        this.snapTarget = snapTarget;
        this.snapRadius = snapRadius;

        Control.MouseDown += (sender, e) =>
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                startLocation = e.Location;
                ZIndex = Control.Parent.Controls.GetChildIndex(Control);
                Control.BringToFront();
            }
        };
        
        Control.MouseUp += (sender, e) =>
        {
            if (isDragging)
            {
                isDragging = false;
                Control.Parent.Controls.SetChildIndex(Control, ZIndex);
                
                if (snapTarget != null)
                {
                    var dx = (snapTarget.Left + snapTarget.Width / 2) - (Control.Left + Control.Width / 2);
                    var dy = (snapTarget.Top + snapTarget.Height / 2) - (Control.Top + Control.Height / 2);

                    if (dx * dx + dy * dy <= snapRadius * snapRadius)
                    {
                        Control.Location = new Point(
                            snapTarget.Left + (snapTarget.Width - Control.Width) / 2,
                            snapTarget.Top + (snapTarget.Height - Control.Height) / 2
                        );
                        Control.BringToFront();
                        return;
                    }
                }
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