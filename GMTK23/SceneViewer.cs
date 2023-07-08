using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace GMTK23;
internal class SceneViewer
{
    public bool Open;

    public void Layout()
    {
        if (Keyboard.IsKeyPressed(Key.F4))
        {
            Open = !Open;
        }

        if (Open && ImGui.Begin("Scene Viewer", ref Open))
        {
            foreach (var component in Program.World.Components)
            {
                ImGui.PushID(component.ToString());
                ImGui.Text(component.ToString());
                ImGui.SameLine();
                if (ImGui.SmallButton("select"))
                {
                    Program.Inspector.Select(component);
                }
                ImGui.PopID();
            }
        }
        ImGui.End();
    }
}
