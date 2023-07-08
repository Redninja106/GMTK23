using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23.Tiles;

/// <summary>
/// Manages all loaded tiles.
/// </summary>
internal class TileManager
{
    public static TileManager Instance { get; } = new();

    public List<Tile> Tiles { get; } = new();

    private TileManager()
    {
        Add(new SolidTile(Color.Red));
        Add(new SolidTile(Color.Blue));
    }

    public void Add(Tile tile)
    {
        Tiles.Add(tile);
    }
}
