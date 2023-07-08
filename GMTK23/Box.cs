using GMTK23;
using SimulationFramework.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23;
internal class Box : IGameComponent, ICollider, ISaveable
{
    public bool IsStatic => true;

    public Transform Transform { get; private set; }
    float width, height;
    public Color color = Color.Parse("#c7986b");

    public Box(float x, float y, float r, float width, float height) : this(new(x, y, r), width, height)
    {
        this.width = width;
        this.height = height;
    }

    public Box(Transform transform, float width, float height)
    {
        this.Transform = transform;
        this.width = width;
        this.height = height;
    }

    public void Render(ICanvas canvas)
    {
        canvas.ApplyTransform(Transform);
        canvas.Fill(color);
        canvas.DrawRect(0, 0, width, height, Alignment.Center);
    }

    public void Update()
    {
    }

    public Rectangle GetBounds()
    {
        return new(0, 0, width, height, Alignment.Center);
    }

    public void OnCollision(ICollider other, Vector2 mtv)
    {
        Destroy();
    }

    public void Destroy()
    {
        Program.World.Remove(this);
    }

    public static IGameComponent Load(ArgReader reader)
    {
        var x = reader.NextFloat();
        var y = reader.NextFloat();
        var w = reader.NextFloat();
        var h = reader.NextFloat();

        return new Box(x, y, 0, w, h);
    }

    public IEnumerable<string> Save()
    {
        yield return Transform.Position.X.ToString();
        yield return Transform.Position.Y.ToString();
        yield return width.ToString();
        yield return height.ToString();
    }

    public void Initalize()
    {
    }

    public RenderLayer RenderLayer => RenderLayer.World;
}
