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
        Add(new SolidTile(1, Color.Gray));
        Add(new SolidTile(2, Color.Green));
        Add(new SolidTile(3, Color.CornflowerBlue));
        Add(new SolidTile(4, Color.Black));
        Add(new SolidTile(5, new Color(92, 78, 46)));
        Add(new SolidTile(6, new Color(61, 61, 61)));
        Add(new SolidTile(7, Color.White));
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
