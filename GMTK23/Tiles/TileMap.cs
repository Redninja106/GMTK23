using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23.Tiles;
internal class TileMap : Entity
{
    public int Width { get; }
    public int Height { get; }
    public Transform Transform { get; }

    private Tile?[] tiles;

    public TileMap(Transform transform, int width, int height)
    {
        this.Transform = transform;
        this.Width = width;
        this.Height = height;

        tiles = new Tile?[width * height];
    }

    public void Render(ICanvas canvas)
    {
        Transform.ApplyTo(canvas);
        // we render bottom up
        for (int y = Height - 1; y > 0; y--)
        {
            for (int x = 0; x < Width; x++)
            {
                var tile = this[x, y];

                if (tile is null)
                    continue;

                tile.Render(canvas, new(x, y, 1, 1));
            }
        }
    }

    public void Update()
    {

    }

    public ref Tile? this[int x, int y]
    {
        get
        {
            if (x < 0 || x > Width)
                throw new ArgumentOutOfRangeException(nameof(x));

            if (y < 0 || y > Height)
                throw new ArgumentOutOfRangeException(nameof(y));

            return ref tiles[y * Width + x];
        }
    }
}
