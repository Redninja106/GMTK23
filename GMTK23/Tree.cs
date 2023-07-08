using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23;
internal class Tree : Entity
{
    Color color = Color.Parse("#a58151");

    public Transform Transform { get; }

    public Tree(Transform transform)
    {
        this.Transform = transform;
    }

    public void Render(ICanvas canvas)
    {
        Transform.ApplyTo(canvas);
        canvas.Fill(color);
        canvas.DrawRect(-1, 0, 2, 6);
    }

    public void Update()
    {

    }
}
