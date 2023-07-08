using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23.Tiles;
internal class TileMap : IGameComponent, ISaveable
{
    public int Width { get; }
    public int Height { get; }

    private Tile?[] tiles;

    public RenderLayer RenderLayer => RenderLayer.Interactables;

    public Transform Transform;

    public TileMap(int width, int height)
    {
        this.Transform = new();
        this.Width = width;
        this.Height = height;

        tiles = new Tile?[width * height];
    }

    [Button]
    public void Edit()
    {
        var mapEditor = Program.World.Find<TileMapEditor>();

        if (mapEditor is null)
        {
            Console.WriteLine("No TileMapEditor is present!");
            return;
        }

        mapEditor.Select(this);
    }

    public void Render(ICanvas canvas)
    {
        canvas.ApplyTransform(Transform);
        // we render bottom up
        for (int y = Height - 1; y >= 0; y--)
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

    public IEnumerable<string> Save()
    {
        yield return Transform.ToString()!;
        yield return Width.ToString();
        yield return Height.ToString();

        foreach (var tile in tiles)
        {
            yield return (tile?.ID ?? 0).ToString();
        }
    }

    public static IGameComponent Load(ArgReader reader)
    {
        var transform = reader.NextTransform();
        var width = reader.NextInt();
        var height = reader.NextInt();

        var result = new TileMap(width, height);
        result.Transform = transform;

        if (reader.CountRemaining > 0) 
        {
            for (int i = 0; i < width * height; i++)
            {
                int id = reader.NextInt();
                result.tiles[i] = TileManager.Instance.FromID(id);
            }
        }

        return result;
    }

    public void Initalize()
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
