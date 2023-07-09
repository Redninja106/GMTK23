using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMTK23.Interactions;

namespace GMTK23.Cards;
internal abstract class PlayableCard
{
    public ITexture? Texture = null;
    public string Name { get; set; }
    public string Description { get; set; }
    public Color Color { get; set; }

    public PlayableCard(ITexture? texture, string name, string description, Color color)
    {
        Texture = texture;
        Name = name;
        Description = description;
        this.Color = color;
    }

    public abstract void Interact(IInteractable interactable);
}

class FallCard : PlayableCard
{
    public FallCard() : base(null, "Fall", "", new Color(0xaa, 0x95, 0x68))
    {
    }

    public override void Interact(IInteractable interactable)
    {
        if (interactable is IFallable fallable)
        {
            fallable.Fall();
        }    
    }
}

class CombustCard : PlayableCard
{
    public CombustCard() : base(null, "Combust", "", new Color(0xdb, 0x66, 0x18))
    {
    }

    public override void Interact(IInteractable interactable)
    {
        if (interactable is ICombustable combustable)
        {
            combustable.Combust();
        }
    }
}

class DrenchCard : PlayableCard
{
    public DrenchCard() : base(null, "Drench", "", new Color(0x1b, 0x99, 0xe2))
    {
    }

    public override void Interact(IInteractable interactable)
    {
        if (interactable is IWettable wettable)
        {
            wettable.Drench();
        }
    }
}