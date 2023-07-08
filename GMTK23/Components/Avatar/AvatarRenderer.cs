using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23.Components.Avatar;
internal class AvatarRenderer : Component
{
    public override void Initialize(Entity parent)
    {
    }

    public override void Update()
    {
    }

    public override void Render(ICanvas canvas)
    {
        canvas.DrawRect(-.5f, 0, 1, 2);
        base.Render(canvas);
    }
}
