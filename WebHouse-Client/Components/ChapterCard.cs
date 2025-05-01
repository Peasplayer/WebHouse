using System.Drawing.Drawing2D;
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
        DrawNeededColors(g);
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
        path.AddLine(shaftRect.Right - 1, centerY + shaftHeight / 2 + arrowHeight / 2, shaftRect.Right + arrowHeadWidth, centerY + shaftHeight / 2); //Diagonale zur Spitze
        path.AddLine(shaftRect.Right + arrowHeadWidth, centerY + shaftHeight / 2, shaftRect.Right - 1, centerY + shaftHeight / 2 - arrowHeight / 2); //Diagonale zur Spitze
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
    
    private void DrawNeededColors(Graphics g)
    {
        int dotWidth = 20;
        int dotHeight = 28;
        int cornerRadius = 6;
        int space = 5;
        int totalWidth = requirements.Length * dotWidth + (requirements.Length - 1) * space;
        int startX = (Cardpanel.Width - totalWidth) / 2;
        int y = Cardpanel.Height - 35;

        for (int i = 0; i < requirements.Length; i++)
        {
            Color color = requirements[i];
            using var brush = new SolidBrush(color);
            Rectangle rect = new Rectangle(startX + i * (dotWidth + space), y, dotWidth, dotHeight);
            using var path = RoundedRectangle(rect, cornerRadius);
            g.FillPath(brush, path);
        }
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
