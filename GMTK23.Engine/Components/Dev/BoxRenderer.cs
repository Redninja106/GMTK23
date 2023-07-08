using GMTK23.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23.Engine.Components.Dev;
internal class BoxRenderer : Component
{
    public float Size = 1;

    public override void Initialize(Entity parent)
    {
    }

    public override void Update()
    {
    }

    public override void Render(ICanvas canvas)
    {
        canvas.DrawRect(0, 0, Size, Size, Alignment.Center);
    }
}
