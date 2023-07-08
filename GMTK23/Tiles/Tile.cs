namespace GMTK23.Tiles;

abstract class Tile
{
    private static int nextID = 1;

    public int ID { get; } = nextID++;
    public abstract void Render(ICanvas canvas, Rectangle bounds);
}