using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23.Tiles;
internal class TileMapEditor
{
    public TileMap TileMap { get; }

    public TileMapEditor(TileMap tileMap)
    {
        this.TileMap = tileMap;
    }

    public void Update()
    {
        if (ImGui.Begin("Tiles"))
        {
        }
        ImGui.End();

        Vector2 localMousePos = TileMap.Transform.WorldToLocal(Vector2.Zero);
        int gridCellX = (int)localMousePos.X;
        int gridCellY = (int)localMousePos.Y;

        if (Mouse.IsButtonDown(MouseButton.Left))
        {
            TileMap[gridCellX, gridCellY] = TileManager.Instance.Tiles.First();
        }
    }

    public void Render(ICanvas canvas)
    {
        var mousePos = Vector2.Zero;
        Vector2 localMousePos = TileMap.Transform.WorldToLocal(mousePos);
        int gridCellX = (int)localMousePos.X;
        int gridCellY = (int)localMousePos.Y;

        TileMap.Transform.ApplyTo(canvas);

        for (int y = 0; y < TileMap.Height; y++)
        {
            for (int x = 0; x < TileMap.Width; x++)
            {
                if (x == gridCellX && y == gridCellY)
                {
                    canvas.Fill(Color.Red);
                }
                else
                {
                    canvas.Stroke(Color.Gray);
                }

                canvas.DrawRect(x, y, 1, 1);
            }
        }
    }
}
