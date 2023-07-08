namespace GMTK23.Tiles;

abstract class Tile
{
    public int ID { get; }

    public Tile(int id)
    {
        this.ID = id;
    }

    public abstract void Render(ICanvas canvas, Rectangle bounds);
}