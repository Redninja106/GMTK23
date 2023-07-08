using SimulationFramework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23;
internal static class SceneViewer
{
    private static bool open;

    public static void Layout()
    {
        if (Keyboard.IsKeyPressed(Key.F2))
            open = !open;

        if (open && ImGui.Begin("Scene Viewer", ref open))
        {
            var scene = GMTKGame.Instance.ActiveScene;

            foreach (var component in scene.Components)
            {
                ImGui.Text(component.ToString());
                ImGui.SameLine();
                if (ImGui.SmallButton("select"))
                {
                    Inspector.Inspect(component);
                }
            }
        }
        ImGui.End();
    }
}
