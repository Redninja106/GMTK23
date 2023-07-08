using GMTK23.Scenes.GameplayScene;
using SimulationFramework.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23;
internal class GameScene
{
    public static GameScene Active => GMTKGame.Instance.ActiveScene;

    private readonly List<Entity> components = new();
    private readonly Queue<Entity> componentsToAdd = new();
    private readonly Queue<Entity> componentsToRemove = new();

    public Camera Camera { get; set; }
    public IEnumerable<Entity> Components => components;

    public GameScene(Camera camera)
    {
        this.Camera = camera;
        AddComponent(camera);
    }

    public void AddComponent(params Entity[] components)
    {
        foreach (var component in components)
        {
            AddComponent(component);
        }
    }

    public void AddComponent(Entity component)
    {
        componentsToAdd.Enqueue(component);
    }

    public void RemoveComponent(Entity component)
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
        Camera.ApplyTo(canvas);


        components.ForEach(c =>
        {
            canvas.PushState();
            c.Render(canvas);
            canvas.PopState();
        });
    }

    public World GetComponent()
    {
    }
}
