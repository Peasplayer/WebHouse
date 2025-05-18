namespace WebHouse_Client.Components;

public class DraggableControl
{
    public Control Control { get; }

    private static DraggableControl? selected = null; //Ausgewählte Karte
    private static bool FormCklickHandler = false; //Verhindert das mehrmals der ClickHandler für die Form hinzugefügt wird
    private static bool NextClick  = false; //Unterdrückt den clickt auf die ChapterCard nachdem eine EscapeCard an sie gelegt wurde
    public static DraggableControl? SelectedControl => selected; //Macht die ausgewählte Karte für andere Klassen nutzbar

    public DraggableControl(Control control)
    {
        Control = control;

        //Wird ausgeführt wenn eine Karte angecklickt wird
        Control.MouseClick += (sender, e) =>
        {
            if (NextClick)
            {
                NextClick = false;
                return;
            }

            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            if (selected == this)
            {
                Deselect(); //Wenn die Karte schon ausgewählt ist wird sie abgewählt
            }
            else
            {
                selected?.Deselect(); //Wenn eine andere Karte ausgewählt ist wird sie abgewählt
                selected = this; 
                Control.Invalidate(); //Kartenrahmen zeichnen
            }
        };
        
        //Sorgt dafür das die Klick händerl nur einemal hinzugefügt wird und verhindert so Probleme
        Control.HandleCreated += (sender, e) =>
        {
            var form = Control.FindForm();
            if (!FormCklickHandler && form != null)
            {
                FormCklickHandler = true;
                form.MouseClick += GlobalFormClick;
            }
        };
    }

    private void Deselect()
    {
        Control.Invalidate(); //Alten Rahmen löschen
        selected = null;
    }
    public static void ClearSelection()
    {
        selected?.Control.Invalidate();
        selected = null;
    }
    
    //wird aufgerufen wenn auf die Form gecklickt wird und Teleportiert die Karte an diese Stelle
    private static void GlobalFormClick(object? sender, MouseEventArgs e)
    {
        if (selected == null) return;
        if (sender is not Form form) return;

        var newLocation = e.Location;
        newLocation.Offset(-selected.Control.Width / 2, -selected.Control.Height / 2); //Zentriert die Karte auf dem Klick. Ohne dieses wird die linke obere Ecke an den Cursor gesetzt

        selected.Control.Location = newLocation; //Bewegt die Karte an ihre neue Position
        selected.Deselect();
    }

    public static void NoNextClick()
    {
        NextClick = true;
    }
}