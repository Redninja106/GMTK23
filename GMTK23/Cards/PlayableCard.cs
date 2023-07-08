using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public abstract void Interact(IInteractable interableObject);
}
