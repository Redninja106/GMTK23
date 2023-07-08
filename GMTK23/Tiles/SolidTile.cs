namespace GMTK23.Tiles;

class SolidTile : Tile
{
    public Color Color { get; }

    public SolidTile(Color color)
    {
        this.Color = color;
    }

    public override void Render(ICanvas canvas, Rectangle bounds)
    {
        canvas.Fill(Color);
        canvas.DrawRect(bounds);
    }
}