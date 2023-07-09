using SimulationFramework.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GMTK23;
internal class LevelBuilder : IGameComponent
{
    public RenderLayer RenderLayer => RenderLayer.UI;

    bool active;

    public void Render(ICanvas canvas)
    {
        if (!active)
            return;

        canvas.PushState();
        canvas.ResetState();

        string desc = 
@"Level Editor Active:
1 - wall
2 - box
delete/backspace - remove objects";

        canvas.FontStyle(20, FontStyle.Normal);
        foreach (var line in desc.Split(Environment.NewLine))
        {
            canvas.DrawText(line, Vector2.Zero);
            canvas.Translate(0, 24);
        }

        canvas.PopState();

        Vector2 coords = new(MathF.Round(Program.MousePosition.X * 2) / 2f, MathF.Round(Program.MousePosition.Y * 2) / 2f);
        if (wallStartPos is not null)
        {
            Vector2 diff = coords - wallStartPos.Value;
            var transform = new Transform(wallStartPos.Value + diff * .5f, Angle.FromVector(diff));
            canvas.ApplyTransform(transform);
            canvas.Fill(Color.FromHSV(0, 0, .9f, .5f));
            canvas.DrawRect(0, 0, diff.Length() + .5f, .5f, Alignment.Center);
        }
    }

    Vector2? wallStartPos;

    public void Update()
    {
#if DEBUG
        if (Keyboard.IsKeyPressed(Key.F2))
            active = !active;

        if (Keyboard.IsKeyPressed(Key.F3))
            saveWindow = !saveWindow;
#endif
        LayoutSaveWindow();

        if (!active)
        {
            wallStartPos = null;
            return;
        }

        Vector2 coords = new(MathF.Round(Program.MousePosition.X * 2) / 2f, MathF.Round(Program.MousePosition.Y * 2) / 2f);

        bool hitTest = Program.World.Collision.TestPoint(Program.MousePosition, null, out var hitCollider);

        if (Keyboard.IsKeyPressed(Key.Delete) || Keyboard.IsKeyPressed(Key.Backspace))
        {
            if (hitTest && hitCollider is IGameComponent g)
            {
                Program.World.Remove(g);
            }
        }

        if (Keyboard.IsKeyPressed(Key.Key2) && !hitTest)
        {
        }

        if (Keyboard.IsKeyPressed(Key.Key1))
        {
            if (wallStartPos is null)
            {
                wallStartPos = coords;
            }
            else
            {
                Vector2 diff = coords - wallStartPos.Value;
                var transform = new Transform(wallStartPos.Value + diff * .5f, Angle.FromVector(diff));
                wallStartPos = null;
            }
        }

        if (Keyboard.IsKeyPressed(Key.Esc) && wallStartPos is not null)
        {
            wallStartPos = null;
        }
    }

    bool saveWindow = false;
    string currentLevelName = "";
    string[]? levels;

    private void LayoutSaveWindow()
    {
        if (saveWindow && ImGui.Begin("Save/Load Levels", ref saveWindow))
        {
            ImGui.InputText("Level File", ref currentLevelName, 128);

            if (!ImGui.IsItemFocused() && string.IsNullOrEmpty(currentLevelName))
            {
                currentLevelName = GetBestLevelFileName();
            }

            if (ImGui.Button("Save Current Level"))
            {
                if (!currentLevelName.EndsWith(".lvl"))
                    currentLevelName += ".lvl";

                SaveLevel($"Levels/{currentLevelName}");
            }

            if (ImGui.CollapsingHeader("Load Levels"))
            {
                if (levels is null || ImGui.Button("Refresh"))
                {
#if DEBUG
                    levels = Directory.GetFiles("../../../Levels/", "*.lvl", SearchOption.AllDirectories);
#else
                    levels = Directory.GetFiles("Levels/", "*.lvl", SearchOption.AllDirectories);
#endif
                }

                foreach (var level in levels)
                {
                    if (ImGui.Selectable(level))
                    {
                        Program.ReloadLevel(level);
                    }
                }
            }
        }
        ImGui.End();
    }

    void SaveLevel(string fileName)
    {
#if DEBUG
        if (Directory.Exists("../../../Levels"))
        {
            fileName = Path.Combine("../../../", fileName);
        }
#endif
        StringBuilder result = new();

        foreach (var component in Program.World.Components.Reverse())
        {
            if (component is ISaveable s)
            { 
                result.Append(component.GetType().Name + " ");
                result.Append(string.Join(" ", s.Save()));
            }
            else
            {
                result.Append(component.GetType().Name);
            }

            result.AppendLine();
        }

        File.WriteAllText(fileName, result.ToString());
    }

    private string GetBestLevelFileName()
    {
        return Path.GetFileName(Program.CurrentLevelPath);

        // int levelNumber = 0;
        // while (File.Exists($"Levels/test{levelNumber}.lvl"))
        //     levelNumber++;
        // 
        // return $"test{levelNumber}.lvl";
    }
}
