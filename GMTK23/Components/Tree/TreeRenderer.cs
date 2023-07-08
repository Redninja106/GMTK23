using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23.Components.Tree;
internal class TreeRenderer : Component
{
    public string ColorStr { get; set; }
    private Color? color;

    public override void Initialize(Entity parent)
    {
        
    }

    public override void Render(ICanvas canvas)
    {
        color ??= Color.Parse(ColorStr);
        canvas.Fill(color.Value);
        canvas.DrawRect(-1, 0, 2, 6);
    }

    public override void Update()
    {
    }
}
