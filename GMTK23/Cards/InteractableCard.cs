using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMTK23.Interactions;

namespace GMTK23.Cards;
internal class InteractableCard : IGameComponent
{
    public Transform Transform { get; } = new();
    public Transform TargetTransform { get; } = new();
    public PlayableCard PlayableCard;

    public bool isDragging;
    private Vector2 dragOffset;

    public float Smoothing { get; set; } = 15;
    public RenderLayer RenderLayer => RenderLayer.Cards;

    public InteractableCard(PlayableCard playableCard)
    {
        this.PlayableCard = playableCard;
    }

    public void Render(ICanvas canvas)
    {
        const float cardWidth = .7f;
        const float cardHeight = 1f;
        const float borderSize = .01f;

        canvas.PushState();
        canvas.Antialias(true);
        canvas.ApplyTransform(Transform);

        canvas.Fill(new Color(0, 0, 0, 127));
        canvas.DrawRect(0, 0, cardWidth + borderSize * 2, cardHeight + borderSize * 2, Alignment.Center);

        canvas.Fill(PlayableCard.Color);
        canvas.DrawRect(0, 0, cardWidth, cardHeight, Alignment.Center);

        canvas.Fill(new Color(0x50, 0x50, 0x50));

        canvas.FontStyle(.15f, FontStyle.Normal);
        canvas.DrawText(PlayableCard.Name, 0, -(cardHeight / 2f - .05f), Alignment.TopCenter);
        canvas.PopState();
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
                        PlayableCard.Interact(interactable);
                    }
                }

                isDragging = false;
            }

            TargetTransform.Position = mousePos;
            TargetTransform.Rotation = 0;
            TargetTransform.Scale = new(7.5f, 7.5f);
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
