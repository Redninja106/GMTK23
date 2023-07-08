using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23;
internal class Avatar : GameObject
{
    public override void Render(ICanvas canvas)
    {
        canvas.DrawRect(-.5f, 0, 1, 2);
    }

    public override void Update()
    {
    }
}
