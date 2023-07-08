using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMTK23;
using GMTK23.Extensions;

namespace GMTK23.Tiles;
internal class TileMapEditor : IGameComponent, ISaveable
{
    public TileMap TileMap { get; private set; }
    public Tile SelectedTile { get; private set; }

    public bool IsGridVisible { get; set; } = true;

    public RenderLayer RenderLayer => RenderLayer.Interactables;

    public TileMapEditor()
    {
    }

    public void Update()
    {
        TileMap ??= Program.World.Components.OfType<TileMap>().First();

        if (ImGui.Begin("Tiles"))
        {
            foreach (var tile in TileManager.Instance.Tiles)
            {
                if (ImGui.Button($"{tile} (id {tile.ID})"))
                {
                    SelectedTile = tile;
                }
            }
        }
        ImGui.End();

        var mousePos = Program.Camera.ScreenToWorld(Mouse.Position);
        Vector2 localMousePos = TileMap.Transform.WorldToLocal(mousePos);
        int gridCellX = (int)localMousePos.X;
        int gridCellY = (int)localMousePos.Y;

        if (gridCellX < 0 || gridCellX >= this.TileMap.Width)
            return;
        if (gridCellY < 0 || gridCellY >= this.TileMap.Height)
            return;

        if (Mouse.IsButtonDown(MouseButton.Left))
        {
            TileMap[gridCellX, gridCellY] = SelectedTile;
        }
        if (Mouse.IsButtonDown(MouseButton.Right))
        {
            TileMap[gridCellX, gridCellY] = null;
        }
    }

    public void Render(ICanvas canvas)
    {
        TileMap ??= Program.World.Components.OfType<TileMap>().First();

        if (!IsGridVisible)
            return;

        var mousePos = Program.Camera.ScreenToWorld(Mouse.Position);
        Vector2 localMousePos = TileMap.Transform.WorldToLocal(mousePos);
        int gridCellX = (int)localMousePos.X;
        int gridCellY = (int)localMousePos.Y;

        canvas.ApplyTransform(TileMap.Transform);

        for (int y = 0; y < TileMap.Height; y++)
        {
            for (int x = 0; x < TileMap.Width; x++)
            {
                if (x == gridCellX && y == gridCellY)
                {
                    canvas.Stroke(Color.Red);
                }
                else
                {
                    canvas.Stroke(Color.Gray);
                }

                canvas.DrawRect(x, y, 1, 1);
            }
        }
    }

    public IEnumerable<string> Save()
    {
        yield break;
    }

    public static IGameComponent Load(string[] args)
    {
        return new TileMapEditor();
    }
}