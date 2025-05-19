using System.Drawing.Drawing2D;

namespace WebHouse_Client.Components;

public class Card
{
    public Panel Panel { get; }
    public bool Highlighted { get; private set; } //Ob die Karte hervorgehoben werden soll
    private readonly int _cornerRadius; //Radius der Ecken
    private readonly int _borderWidth; //Dicke des Rahmens
    private readonly Color _color; //Hintergrundfarbe der Karte
    private readonly Action<Graphics>? _additionalPaint;

    public Card(int borderWidth, int cornerRadius, Color color, int outlineWidth, Action<Graphics>? additionalPaint = null)
    {
        _cornerRadius = cornerRadius;
        _borderWidth = borderWidth;
        _color = color;
        _additionalPaint = additionalPaint;

        //Panel wird erstellt
        Panel = new BufferPanel()
        {
            BackColor = Color.Transparent
        };

        //Karten werden erstellt
        Panel.Paint += DrawCard;
    }
    
    private void DrawCard(object? sender, PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        using (GraphicsPath path = RoundedRectangle(new Rectangle(0, 0, Panel.Size.Width, Panel.Size.Height), _cornerRadius))
        using (SolidBrush brush = new SolidBrush(_color))
        using (Pen pen = new Pen(Color.White, _borderWidth))
        using (Pen highlightPen = new Pen(Color.HotPink, _borderWidth + 1))
        {
            g.FillPath(brush, path); //Hintergrund der Karte
            if (_additionalPaint != null)
            {
                _additionalPaint(g); //Zusätzliche Zeichnungen
            }
            
            g.DrawPath(Highlighted ? highlightPen : pen, path); //Rahmen der Karte zeichnen
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
    
    public void SetHighlighted(bool highlighted)
    {
        Highlighted = highlighted;
        Panel.Invalidate(); //Panel neu zeichnen
    }
}