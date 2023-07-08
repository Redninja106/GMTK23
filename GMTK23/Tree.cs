using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23;
internal class Tree : GameObject
{
    public override void Render(ICanvas canvas)
    {
        canvas.DrawRect(-1, 0, 2, 6);
    }

    public override void Update()
    {
    }
}
