using SimulationFramework.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23;
internal class GameScene
{
    private readonly List<GameObject> gameObjects = new();
    private readonly Queue<GameObject> gameObjectsToAdd = new();
    private readonly Queue<GameObject> gameObjectsToRemove = new();
    private bool isUpdating;

    public void Add(GameObject gameObject)
    {
        if (isUpdating)
        {
            gameObjectsToAdd.Enqueue(gameObject);
        }
        else
        {
            gameObjects.Add(gameObject);
        }
    }

    public void Remove(GameObject gameObject)
    {
        if (isUpdating)
        {
            gameObjectsToRemove.Enqueue(gameObject);
        }
        else
        {
            gameObjects.Remove(gameObject);
        }
    }

    public virtual void Update()
    {
        isUpdating = true;
        gameObjects.ForEach(g => g.Update());
        isUpdating = false;

        while (gameObjectsToAdd.Count > 0)
        {
            gameObjects.Add(gameObjectsToAdd.Dequeue());
        }

        while (gameObjectsToRemove.Count > 0)
        {
            gameObjects.Add(gameObjectsToRemove.Dequeue());
        }
    }

    public virtual void Render(ICanvas canvas)
    {
        gameObjects.ForEach(g => g.Render(canvas));
    }

    public TGameObject? Find<TGameObject>() where TGameObject : GameObject
    {
        return FindAll<TGameObject>().FirstOrDefault(); 
    }

    public TGameObject? Find<TGameObject>(Predicate<TGameObject> predicate) where TGameObject : GameObject
    {
        return FindAll(predicate).FirstOrDefault();
    }


    public IEnumerable<TGameObject> FindAll<TGameObject>() where TGameObject : GameObject
    {
        return FindAll<TGameObject>(x => true);
    }

    public IEnumerable<TGameObject> FindAll<TGameObject>(Predicate<TGameObject> predicate) where TGameObject : GameObject
    {
        return gameObjects.OfType<TGameObject>().Where(g => predicate(g));
    }
}
