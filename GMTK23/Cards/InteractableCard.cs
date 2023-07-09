using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23.Cards;
internal class InteractableCard : IGameComponent
{
    public Transform Transform { get; } = new();
    public Transform TargetTransform { get; } = new();

    public bool isDragging;
    private Vector2 dragOffset;

    public float Smoothing { get; set; } = 8;
    public RenderLayer RenderLayer => RenderLayer.Cards;
    public Color color = Color.FromHSV(Random.Shared.NextSingle(), 1f, 1f);

    public InteractableCard()
    {
    }

    public void Render(ICanvas canvas)
    {
        canvas.ApplyTransform(Transform);
        canvas.Fill(color);
        canvas.DrawRect(0, 0, .7f, 1, Alignment.Center);
    }

    public void Update()
    {
        Transform.Position = Vector2.Lerp(Transform.Position, TargetTransform.Position, 1f - MathF.Exp(-Smoothing * Time.DeltaTime));
        Transform.Rotation = Angle.Lerp(Transform.Rotation, TargetTransform.Rotation, 1f - MathF.Exp(-Smoothing * Time.DeltaTime));
        Transform.Scale = Vector2.Lerp(Transform.Scale, TargetTransform.Scale, 1f - MathF.Exp(-Smoothing * Time.DeltaTime));

        var mousePos = Program.Camera.ScreenToWorld(Mouse.Position);
        var hand = Program.World.Find<PlayerHand>();

        if (isDragging)
        {
            if (Mouse.IsButtonReleased(MouseButton.Left))
            {
                var interactables = Program.World.FindAll<IInteractable>();

                foreach (var interactable in interactables)
                {
                    var bounds = interactable.GetBounds();

                    if (bounds.ContainsPoint(Transform.LocalToWorld(Vector2.Zero)))
                    {
                        
                    }
                }

                isDragging = false;
            }

            TargetTransform.Position = mousePos;
            TargetTransform.Rotation = 0;
        }
        else
        {
            if (Mouse.IsButtonPressed(MouseButton.Left) && this == hand!.GetCard(mousePos))
            {
                isDragging = true;
                dragOffset = this.Transform.Position - mousePos;
            }
        }
    }

    public bool ContainsPoint(Vector2 point, Vector2 localOffset = default)
    {
        var localBounds = new Rectangle(Vector2.Zero, new(.7f, 1f), Alignment.BottomCenter);

        Vector2 pointLocalSpace = Transform.WorldToLocal(point);

        return localBounds.ContainsPoint(pointLocalSpace + localOffset);
    }
}
