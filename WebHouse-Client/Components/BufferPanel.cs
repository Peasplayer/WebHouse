namespace WebHouse_Client.Components;

public class BufferPanel : Panel
{
    //Alle Zeichnungen finden in einem Puffer staat damit die Karten nicht Flackern wen sie bewegt werden
    public BufferPanel()
    {
        this.DoubleBuffered = true;
        this.ResizeRedraw = true;
        this.SetStyle(ControlStyles.OptimizedDoubleBuffer |
                      ControlStyles.AllPaintingInWmPaint |
                      ControlStyles.UserPaint, true);
        this.UpdateStyles();
    }
}