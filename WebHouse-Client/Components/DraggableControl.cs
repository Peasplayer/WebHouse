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

        foreach (var target in SnapTargets)
        {
            target.MouseEnter += (s, e) =>
            {
                if (selectedCard != null)
                    target.BackColor = Color.LightGreen;
            };

            target.MouseLeave += (s, e) =>
            {
                target.BackColor = Color.LightGray;
            };
        }

        Control.Click += (sender, e) =>
        {
            if (selectedCard != null)
            {
                // Karte teleportieren, wenn ein Ziel geklickt wurde
                if (sender is Control clickedControl)
                {
                    foreach (var snapTarget in SnapTargets)
                    {
                        if (snapTarget.Bounds.Contains(clickedControl.PointToScreen(Point.Empty)))
                        {
                            selectedCard.Location = new Point(
                                snapTarget.Left + (snapTarget.Width - selectedCard.Width) / 2,
                                snapTarget.Top + (snapTarget.Height - selectedCard.Height) / 2
                            );
                            selectedCard.BackColor = Color.White;
                            selectedCard = null;
                            break;
                        }
                    }
                }
            }
            else
            {
                selectedCard = Control;
                selectedCard.BackColor = Color.LightBlue;
            }
        };
    }
}