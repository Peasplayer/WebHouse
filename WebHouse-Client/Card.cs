using System.Drawing.Drawing2D;
using System.Windows.Forms.VisualStyles;

namespace WebHouse_Client;

public class Card
{
    public Panel cardPanel { get; }
    private readonly int cornerRadius;
    private readonly int borderWidth;
    private readonly Color color;

    public Card(Size size, int borderWidth, int cornerRadius, Color color, int outlineWidth)
    {
        this.cornerRadius = cornerRadius;
        this.borderWidth = borderWidth;
        this.color = color;

        cardPanel = new BufferPanel()
        {
            Size = size,
            BackColor = Color.Transparent
        };

        cardPanel.Paint += (sender, e) =>
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            using (GraphicsPath path = RoundedRect(new Rectangle(0, 0, size.Width, size.Height), cornerRadius))
            using (SolidBrush brush = new SolidBrush(color))
            using (Pen pen = new Pen(Color.White, outlineWidth))
            {
                g.FillPath(brush, path);
                g.DrawPath(pen, path);
            }
        };
    }

    private GraphicsPath RoundedRect(Rectangle rect, int cornerRadius)
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