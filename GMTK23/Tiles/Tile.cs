namespace GMTK23.Tiles;

abstract class Tile
{
    public abstract void Render(ICanvas canvas, Rectangle bounds);
}