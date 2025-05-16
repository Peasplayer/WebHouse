using WebHouse_Client.Components;

namespace WinFormsTest;

public class DraggableControl
{
    public Control Control { get; }

    private static DraggableControl? _selectedControl = null;
    private static bool _isFormClickHandlerAttached = false;

    public DraggableControl(Control control)
    {
        Control = control;

        Control.MouseClick += (sender, e) =>
        {
            if (e.Button != MouseButtons.Left) return;

            if (_selectedControl == this)
            {
                Deselect();
            }
            else
            {
                _selectedControl?.Deselect();
                _selectedControl = this;
            }
        };

        //Versuche erst später das Form zu finden
        Control.HandleCreated += (sender, e) =>
        {
            var form = Control.FindForm();
            if (!_isFormClickHandlerAttached && form != null)
            {
                _isFormClickHandlerAttached = true;
                form.MouseClick += GlobalFormClick;
            }
        };
    }

    private void Deselect()
    {
        _selectedControl = null;
    }

    private static void GlobalFormClick(object? sender, MouseEventArgs e)
    {
        if (_selectedControl == null) return;
        if (sender is not Form form) return;

        var newLocation = e.Location;
        newLocation.Offset(-_selectedControl.Control.Width / 2, -_selectedControl.Control.Height / 2);

        _selectedControl.Control.Location = newLocation;
        _selectedControl.Deselect();
    }
}