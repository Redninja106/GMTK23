using SimulationFramework.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23;
internal class GameScene
{
    private readonly List<IGameComponent> components = new();
    private readonly Queue<IGameComponent> componentsToAdd = new();
    private readonly Queue<IGameComponent> componentsToRemove = new();

    public void AddComponent(IGameComponent component)
    {
        componentsToAdd.Enqueue(component);
    }

    public void RemoveComponent(IGameComponent component)
    {
        componentsToAdd.Enqueue(component);
    }

    public virtual void Update()
    {
        while (componentsToAdd.Count > 0)
        {
            components.Add(componentsToAdd.Dequeue());
        }

        components.ForEach(c => c.Update());

        while (componentsToRemove.Count > 0)
        {
            components.Add(componentsToRemove.Dequeue());
        }
    }

    public virtual void Render(ICanvas canvas)
    {
        components.ForEach(c => c.Render(canvas));
    }
}
