using System.Drawing.Drawing2D;

namespace WebHouse_Client.Components;

public class Card
{
    public Panel Panel { get; }
    private readonly int cornerRadius; //Radius der Ecken
    private readonly int borderWidth; //Dicke des Rahmens
    private readonly Color color; //Hintergrundfarbe der Karte

    public Card(Size size, int borderWidth, int cornerRadius, Color color, int outlineWidth)
    {
        this.cornerRadius = cornerRadius;
        this.borderWidth = borderWidth;
        this.color = color;

        //Panel wird erstellt
        Panel = new BufferPanel()
        {
            Size = size,
            BackColor = Color.Transparent
        };

        //Karten werden erstellt
        Panel.Paint += (sender, e) =>
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (GraphicsPath path = RoundedRectangle(new Rectangle(0, 0, size.Width, size.Height), cornerRadius))
            using (SolidBrush brush = new SolidBrush(color))
            using (Pen pen = new Pen(Color.White, outlineWidth))
            using (Pen highlightPen = new Pen(Color.HotPink, outlineWidth + 1))
            {
                g.FillPath(brush, path);

                //Wenn eine Karte ausgewählt wird wird die Outline gemahlt
                if (DraggableControl.SelectedControl?.Control == Panel)
                {
                    g.DrawPath(highlightPen, path);
                }
                else
                {
                    g.DrawPath(pen, path);
                }
            }
        };
    }

    private GraphicsPath RoundedRectangle(Rectangle rect, int cornerRadius)
    {
        int diameter = cornerRadius * 2;
        GraphicsPath path = new GraphicsPath();
        path.AddArc(rect.Left, rect.Top, diameter, diameter, 180, 90);
        path.AddArc(rect.Right - diameter, rect.Top, diameter, diameter, 270, 90);
        path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
        path.AddArc(rect.Left, rect.Bottom - diameter, diameter, diameter, 90, 90);
        path.CloseFigure();
        return path;
    }
}