using Silk.NET.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23.Cards;
internal class InteractableCard : Entity
{
    public Transform Transform { get; }
    public Transform TargetTransform { get; }

    public float Smoothing { get; set; } = 10;

    public void Render(ICanvas canvas)
    {
    }

    public void Update()
    {
        Transform.Position = Vector2.Lerp(Transform.Position, TargetTransform.Position, 1f - MathF.Exp(-Smoothing * Time.DeltaTime));
        Transform.Rotation = Angle.Lerp(Transform.Rotation, TargetTransform.Rotation, 1f - MathF.Exp(-Smoothing * Time.DeltaTime));
        Transform.Scale = Vector2.Lerp(Transform.Scale, TargetTransform.Scale, 1f - MathF.Exp(-Smoothing * Time.DeltaTime));
    }

    public bool ContainsPoint(Vector2 point, Vector2 localOffset = default)
    {
        var localBounds = new Rectangle(Vector2.Zero, new(CardRenderer.CardAspectRatio, 1), Alignment.BottomCenter);

        Vector2 pointLocalSpace = Transform.WorldToLocal(point);

        return localBounds.ContainsPoint(pointLocalSpace + localOffset);
    }
}
