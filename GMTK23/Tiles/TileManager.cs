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
        Add(new SolidTile(Color.Green));
    }

    public void Add(Tile tile)
    {
        Tiles.Add(tile);
    }

    public Tile? FromID(int id)
    {
        if (id is 0)
            return null;    

        return Tiles.Single(t => t.ID == id);
    }
}
