using SimulationFramework.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23;
internal class Entity
{
    private readonly List<Entity> children = new();
    private readonly Queue<Entity> childrenToAdd = new();
    private readonly Queue<Entity> childrenToRemove = new();
    private bool isUpdating;
    
    public Entity Parent { get; init; }
    public Transform Transform { get; } = new();


    public virtual void Render(ICanvas canvas)
    {
        foreach (var child in children)
        {
            canvas.PushState();
            child.Render(canvas);
            canvas.PopState();
        }
    }

    public virtual void Update()
    {
        isUpdating = true;
        foreach (var child in children)
        {
            child.Update();
        }
        isUpdating = false;

        while (childrenToRemove.Count > 0)
        {
            children.Remove(childrenToRemove.Dequeue());
        }

        while (childrenToAdd.Count > 0)
        {
            children.Add(childrenToAdd.Dequeue());
        }
    }

    public void AddChild(Entity entity)
    {
        if (isUpdating)
        { 
            childrenToAdd.Enqueue(entity);
            return;
        }

        children.Add(entity);
    }

    public void RemoveChild(Entity entity)
    {
        if (isUpdating)
        {
            childrenToRemove.Enqueue(entity);
            return;
        }

        children.Add(entity);
    }

    public void Destroy()
    {
    }
}
