using SimulationFramework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23;
internal static class Inspector
{
    private static object? selectedObject = null;
    private static bool open;

    public static void Inspect(object obj)
    {
        selectedObject = obj;
    }

    public static void Layout()
    {
        if (Keyboard.IsKeyPressed(Key.F1))
            open = !open;

        if (open && ImGui.Begin("Inspector", ref open))
        {
            switch (selectedObject)
            {
                case IInspectable inspectable:
                    inspectable.Layout();
                    break;
                case not null:
                    ImGui.Text($"Inspecting '{selectedObject}' of type '{selectedObject.GetType()}'.");
                    ImGui.Text("To implement a custom inspector menu for this object, implement IInspectable.");
                    break;
                default:
                    ImGui.Text("Not inspecting an object.");
                    break;
            }
        }
        ImGui.End();
    }
}
