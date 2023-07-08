namespace GMTK23.Tiles;

class SpriteTile : Tile
{
    public ITexture texture;
    public Rectangle sourceBounds;

    public SpriteTile(int id, ITexture texture, Rectangle sourceBounds) : base(id)
    {
        this.texture = texture;
        this.sourceBounds = sourceBounds;
    }

    public override void Render(ICanvas canvas, Rectangle destination)
    {
        canvas.DrawTexture(texture, sourceBounds, destination);
    }
}
