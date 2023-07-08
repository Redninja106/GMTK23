using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23.Engine.Components.Dev;
internal class BoxSpawner : Component
{
    public override void Initialize(Entity parent)
    {
    }

    public override void Update()
    {
        if (Mouse.IsButtonPressed(MouseButton.Middle))
        {
            var box = Entity.Create("./Components/Dev/box.arch", Scene.Active);
            box.Transform.Position = Camera.Active.ScreenToWorld(Mouse.Position);
        }
    }
}
