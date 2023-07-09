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

    public PlayableCard(ITexture? texture, string name, string description)
    {
        Texture = texture;
        Name = name;
        Description = description;
    }

    public abstract void Interact(IInteractable interactable);
}

class FallCard : PlayableCard
{
    public FallCard() : base(null, "Fall", "")
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
    public CombustCard() : base(null, "Combust", "")
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
    public DrenchCard() : base(null, "Drench", "")
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