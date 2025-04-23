using WebHouse_Client.Components;

namespace WebHouse_Client;

public class ChapterCard
{
    public Card Card { get; }
    public Panel Cardpanel => Card.cardPanel;

    private string title;
    private int number;
    private Color[] requirements;

    public ChapterCard(string title, int number, Color[] requirements)
    {
        this.title = title;
        this.number = number;
        this.requirements = requirements;

        Card = new Card(new Size(125, 200), 5, 10, Color.Black, 2);
        Cardpanel.Paint += DrawChapterCard;
    }

    private void DrawChapterCard(object? sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        DrawTitle(g);
        DrawArrow(g);
        DrawDots(g);
    }

    private void DrawTitle(Graphics g)
    {
        Font font = new Font("Arial", 12, FontStyle.Bold);
        SizeF textSize = g.MeasureString(title, font);
        PointF textPosition = new PointF((Cardpanel.Width - textSize.Width) / 2, 10);
        g.DrawString(title, font, Brushes.White, textPosition);
    }

    private void DrawArrow(Graphics g)
    {
        Rectangle rect = new Rectangle(15, 70, 95, 30);
        int arrowHeadSize = 20;

        using var arrowBackground = new SolidBrush(Color.White);
        using var arrowFrame = new Pen(Color.Black, 2);
        using var numberFont = new Font("Arial", 14, FontStyle.Bold);
        using var numberColor = new SolidBrush(Color.Black);

        var path = new System.Drawing.Drawing2D.GraphicsPath();
        path.AddLine(rect.Left, rect.Top, rect.Right - arrowHeadSize, rect.Top);
        path.AddLine(rect.Right - arrowHeadSize, rect.Top, rect.Right, rect.Top + rect.Height / 2);
        path.AddLine(rect.Right, rect.Top + rect.Height / 2, rect.Right - arrowHeadSize, rect.Bottom);
        path.AddLine(rect.Right - arrowHeadSize, rect.Bottom, rect.Left, rect.Bottom);
        path.CloseFigure();

        g.FillPath(arrowBackground, path);
        g.DrawPath(arrowFrame, path);

        var text = number.ToString();
        var textSize = g.MeasureString(text, numberFont);
        g.DrawString(text, numberFont, numberColor,
            rect.Left + (rect.Width - arrowHeadSize - textSize.Width) / 2,
            rect.Top + (rect.Height - textSize.Height) / 2);
    }
    
    private void DrawDots(Graphics g)
    {
        int circleSize = 20;
        int space = 5;
        int totalWidth = requirements.Length * circleSize + (requirements.Length - 1) * space;
        int startX = (125 - totalWidth) / 2;
        int y = 200 - 35;

        foreach (var (color, i) in requirements.Select((c, i) => (c, i)))
        {
            using var brush = new SolidBrush(color);
            g.FillEllipse(brush, startX + i * (circleSize + space), y, circleSize, circleSize);
        }
    }
}
