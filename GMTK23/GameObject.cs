using SimulationFramework.Drawing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23;
internal abstract class GameObject
{
    public Transform Transform { get; } = new();

    public GameObject()
    {

    }

    public virtual void Initialize(GameScene scene)
    {

    }

    public virtual void Render(ICanvas canvas)
    {

    }

    public abstract void Update();
}
