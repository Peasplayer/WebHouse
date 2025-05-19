namespace WebHouse_Client.Components;

public class BufferPictureBox : PictureBox
{
    public BufferPictureBox()
    {
        this.DoubleBuffered = true;
        this.ResizeRedraw = true;
        this.SetStyle(ControlStyles.OptimizedDoubleBuffer |
                      ControlStyles.AllPaintingInWmPaint |
                      ControlStyles.UserPaint, true);
        this.UpdateStyles();
    }
}