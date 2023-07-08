using System.Runtime.CompilerServices;

namespace GMTK23.Tiles;

class SolidTile : Tile
{
    public Color Color { get; }
    private string? desc = null;

    public SolidTile(Color color, [CallerArgumentExpression(nameof(color))] string? expr = null)
    {
        this.Color = color;
        desc = expr;
    }

    public override void Render(ICanvas canvas, Rectangle bounds)
    {
        canvas.Fill(Color);
        canvas.DrawRect(bounds);
    }

    public override string ToString()
    {
        return desc ?? base.ToString();
    }
}