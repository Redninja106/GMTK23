using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23.Cards;

internal class PlayerHand : IGameComponent, ISaveable
{
    public List<InteractableCard> Cards { get; set; }
    public Transform Transform { get; } = new();

    public PlayerHand(Transform transform, float scale)
    {
        this.Transform = transform;
        this.scale = scale;

        Cards = new()
        {
            new InteractableCard(new CombustCard()),
            new InteractableCard(new DrenchCard()),
            new InteractableCard(new FallCard()),
        };
    }

    public float offset = .4f;
    public float radius = 2f;
    public float breadthStrength = 1.05f;
    public float breadthUpper = Angle.ToRadians(90f);
    public float breadthLower = Angle.ToRadians(10f);
    public float scale = 5f;

    public event Action<InteractableCard>? OnCardSelected;
    public bool SelectionEnabled { get; set; } = true;

    public InteractableCard? SelectedCard { get; private set; }
    public RenderLayer RenderLayer => RenderLayer.Cards;

    public void Render(ICanvas canvas)
    {
        foreach (var card in Cards)
        {
            card.Render(canvas);
        }
    }

    public void Update()
    {
        float breadth = breadthUpper - (breadthUpper - breadthLower) / MathF.Pow(breadthStrength, (Cards.Count - 2));
        float increment = (breadth / Cards.Count);
        float baseAngle = (increment - breadth) / 2f - (MathF.PI / 2f);

        var mousePosition = Program.Camera.ScreenToWorld(Mouse.Position);
        this.SelectedCard = GetCard(mousePosition);

        bool isSelecting = SelectionEnabled && SelectedCard is not null && !Cards.Any(c => c.isDragging);

        var selectedCardIndex = isSelecting ? Cards.IndexOf(SelectedCard!) : -1;

        for (int i = 0; i < Cards.Count; i++)
        {
            var card = Cards[i];

            if (!card.isDragging)
            {

                float angle = this.Transform.Rotation + baseAngle + i * increment;

                if (selectedCardIndex is not -1 && selectedCardIndex != (Cards.Count - 1))
                {
                    float diff = (increment / -2f) + Angle.ToRadians(12.5f / radius);

                    if (i <= selectedCardIndex)
                    {
                        angle -= diff;
                    }
                    else
                    {
                        angle += diff;
                    }
                }

                this.Transform.Scale = new(scale, scale);
                card.TargetTransform.Position = this.Transform.Position + scale * (Vector2.UnitY * (radius - offset) + Angle.ToVector(angle) * radius);
                card.TargetTransform.Rotation = angle + (MathF.PI / 2f);
                card.TargetTransform.Scale = this.Transform.Scale;

                if (SelectionEnabled && card == SelectedCard)
                {
                    card.TargetTransform.Scale *= 1.20f;
                    card.TargetTransform.Position += Angle.ToVector(angle) * .15f * scale;
                }
            }

            card.Update();
        }

        if (SelectedCard is not null && SelectionEnabled && Mouse.IsButtonPressed(MouseButton.Left))
        {
            this.OnCardSelected?.Invoke(SelectedCard);
        }
    }

    public InteractableCard? GetCard(Vector2 worldSpacePoint)
    {
        // cards are stored left to right, and a card is always on top of the card to its left.
        // we can assume that if we test the cards in reverse order, the first success will be the topmost card.

        for (int i = Cards.Count - 1; i >= 0; i--)
        {
            if (Cards[i].ContainsPoint(worldSpacePoint) ||
                Cards[i].ContainsPoint(worldSpacePoint, Vector2.UnitY * -1f))
                return Cards[i];
        }

        return null;
    }

    public IEnumerable<string> Save()
    {
        yield return Transform.ToString();
        yield return scale.ToString();
    }

    public static IGameComponent Load(ArgReader reader)
    {
        var transform = reader.NextTransform();
        var scale = reader.NextFloat();

        return new PlayerHand(transform, scale);
    }

    // public InteractableCard? GetCard(Card card)
    // {
    //     return Cards.FirstOrDefault(c => c.Card.ID == card.ID);
    // }
}