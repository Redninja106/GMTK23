using GMTK23.Extensions;
using GMTK23.Interactions;
using GMTK23.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23;

internal class Avatar : IGameComponent, ISaveable, IFallable
{
    public Transform Transform { get; set; }
    public Vector2 TargetPos { get; set; }
    public float WalkSpeed { get; set; } = 5;

    public Avatar(Transform transform)
    {
        Transform = transform;
        TargetPos = transform.Position;
    }

    public RenderLayer RenderLayer => RenderLayer.Avatar;

    public void Render(ICanvas canvas)
    {
        canvas.ApplyTransform(Transform);
        canvas.DrawRect(0, 0, 2, 3);
    }

    public void Update()
    {
        Vector2 v = GMTK23.Extensions.VectorExtensions.StepTowards(Transform.Position, TargetPos, WalkSpeed * Time.DeltaTime);
        Transform.Position = v;
    }

    public void setTargetPos(Vector2 targetPos)
    {
        this.TargetPos = targetPos;
    }

    public IEnumerable<string> Save()
    {
        yield return Transform.ToString();
    }

    public static IGameComponent Load(ArgReader reader)
    {
        Transform transform = reader.NextTransform();
        Avatar av = new Avatar(transform);
        return av;
    }

    public void Fall()
    {
        this.Transform.Rotation = Angle.ToRadians(90);
        this.TargetPos = new(Transform.Position.X + 3, Transform.Position.Y + 1);
        this.Transform.Position = new(Transform.Position.X + 3, Transform.Position.Y + 1);
    }

    public Rectangle GetBounds()
    {
        return new Rectangle(Transform.Position, new(2, 3));
    }
}
