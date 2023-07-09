using SimulationFramework.Drawing;
using System.Numerics;

namespace GMTK23.Particles;

struct Particle
{
    public Transform transform;
    public Vector2 velocity;
    public float angularVelocity;
    public float size;
    public Color color;
    public float age;
    public float lifetime;
    public float drag;
    public Vector2 acceleration;

    internal void Render(ICanvas canvas)
    {
        if (Vector2.DistanceSquared(Program.Camera.Transform.Position, transform.Position) > Program.Camera.HorizontalSize * Program.Camera.HorizontalSize)
            return;

        canvas.PushState();
        canvas.Fill(color);
        canvas.ApplyTransform(transform);
        canvas.DrawRect(0, 0, size, size, Alignment.Center);
        canvas.PopState();
    }

    internal bool Update()
    {
        if (!Program.World.Collision.TestPoint(transform.Position))
        {
            velocity += acceleration * Time.DeltaTime;
            transform.Position += velocity * Time.DeltaTime;
            // velocity *= MathF.Pow(drag, Time.DeltaTime);
        }

        transform.Rotation += angularVelocity * Time.DeltaTime;
        age += Time.DeltaTime;

        return age < lifetime;
    }
}