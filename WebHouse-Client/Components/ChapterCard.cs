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

        Card = new Card(new Size(135, 200), 5, 10, Color.Black, 2);
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
        PointF textPosition = new PointF((Cardpanel.Width - textSize.Width) / 2, 30);
        g.DrawString(title, font, Brushes.White, textPosition);
    }

    private void DrawArrow(Graphics g)
    {
        int shaftHeight = 30;
        int shaftWidth = 80;         //Breite des Schafts
        int arrowHeight = 50;        //Höhe des gesamten Pfeils
        int arrowHeadWidth = 30;     //Breite der Spitze
        int startX = 0;
        int centerY = (Cardpanel.Height - arrowHeight) / 2 + 25;

        Rectangle shaftRect = new Rectangle(startX, centerY, shaftWidth, shaftHeight);

        using var arrowBackground = new SolidBrush(Color.White);
        using var arrowFrame = new Pen(Color.Black, 2);
        using var numberFont = new Font("Arial", 14, FontStyle.Bold);
        using var numberColor = new SolidBrush(Color.Black);

        var path = new System.Drawing.Drawing2D.GraphicsPath();

        path.StartFigure();
        path.AddLine(shaftRect.Right - 1, centerY + shaftHeight / 2 + arrowHeight / 2, shaftRect.Right + arrowHeadWidth, centerY + shaftHeight / 2); // Diagonale zur Spitze
        path.AddLine(shaftRect.Right + arrowHeadWidth, centerY + shaftHeight / 2, shaftRect.Right - 1, centerY + shaftHeight / 2 - arrowHeight / 2); // Diagonale zur Spitze
        path.AddLine(shaftRect.Right - 1, centerY + shaftHeight / 2 - arrowHeight / 2, shaftRect.Right - 1, centerY + shaftHeight / 2 + arrowHeight / 2);
        
        /*path.AddLine(shaftRect.Left, shaftRect.Top, shaftRect.Right, shaftRect.Top); 
        path.AddLine(shaftRect.Right, shaftRect.Top, shaftRect.Right + arrowHeadWidth, shaftRect.Top + arrowHeight / 2); 
        path.AddLine(shaftRect.Right + arrowHeadWidth, shaftRect.Top + arrowHeight / 2, shaftRect.Right, shaftRect.Bottom); 
        path.AddLine(shaftRect.Right, shaftRect.Bottom, shaftRect.Left, shaftRect.Bottom); 
        */path.CloseFigure();
        
        g.FillPath(arrowBackground, path);
        //g.DrawPath(arrowFrame, path);
        g.FillRectangle(arrowBackground, shaftRect);

        var text = number.ToString();
        var textSize = g.MeasureString(text, numberFont);
        float textX = shaftRect.Left + (shaftRect.Width - textSize.Width) / 2;
        float textY = shaftRect.Top + (shaftRect.Height - textSize.Height) / 2;
        g.DrawString(text, numberFont, numberColor, textX, textY);
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
