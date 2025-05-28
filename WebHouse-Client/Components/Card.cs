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
    
    //Zeichnet eine Karte mit abgerundeten Ecken die hervirgehoben werden kann
    private void DrawCard(object? sender, PaintEventArgs e) 
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        int cornerRadius = Panel.Width / 20;     //Berechnet den Radius der Ecken in Abhängigkeit von der Panel Größe damit siech diese an die größe des Bildschirms anpassen
        int borderWidth = Panel.Width / 100;     //Berechnet die breite des Rahmens in Abhängigkeit von der Panel Größe damit siech diese an die größe des Bildschirms anpassen

        using (GraphicsPath path = RoundedRectangle(new Rectangle(0, 0, Panel.Size.Width, Panel.Size.Height), cornerRadius)) //Karten werden wie das Rechteck mit abgerundeten Ecken gezeichnet
        using (SolidBrush brush = new SolidBrush(_color))
        using (Pen pen = new Pen(Color.White, borderWidth))
        using (Pen highlightPen = new Pen(Color.HotPink, borderWidth + 1)) //Hebt die Karte durch einen Farblichen rahmen hervor
        
        {
            //Karte füllen
            g.FillPath(brush, path);

            _additionalPaint?.Invoke(g); //Zeichnet zusätzliche Elemente auf die Karte fals es welche gibt

            //Zeichnet den Rahmen der Karte
            using (GraphicsPath borderPath = Outline(new Rectangle(0, 0, Panel.Size.Width, Panel.Size.Height), cornerRadius, borderWidth))
            {
                g.DrawPath(Highlighted ? highlightPen : pen, borderPath);
            }
        }
    }

    //Berechnet eine Rechteck mit abgerundeten Ecken. 
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

    //Zeichnet den Rahmen der Karte
    private GraphicsPath Outline(Rectangle rect, int cornerRadius, int outlineWidth)
    {
        int diameter = cornerRadius * 2;
        GraphicsPath path = new GraphicsPath();
        path.AddArc(rect.Left + outlineWidth / 2, rect.Top + outlineWidth / 2, diameter, diameter, 180, 90);
        path.AddArc(rect.Right - diameter - outlineWidth / 2, rect.Top + outlineWidth / 2, diameter, diameter, 270, 90);
        path.AddArc(rect.Right - diameter - outlineWidth / 2, rect.Bottom - diameter - outlineWidth / 2, diameter, diameter, 0, 90);
        path.AddArc(rect.Left + outlineWidth / 2, rect.Bottom - diameter - outlineWidth / 2, diameter, diameter, 90, 90);
        path.CloseFigure();
        return path;
    }
    //Wird ausgeführt den die Karte hervorgehoben werden soll
    public void SetHighlighted(bool highlighted)
    {
        Highlighted = highlighted;
        Panel.Invalidate(); //Panel neu zeichnen
    }
}