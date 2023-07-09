using GMTK23.Particles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23.Interactions;
internal class ElementalState
{
    public bool IsBurning { get; private set; }
    public bool IsWet { get; private set; }

    public ParticleSystem particleSystem;
    public Rectangle bounds;

    public ElementalState(IInteractable interactable)
    {
        bounds = interactable.GetBounds();
        particleSystem = new(null);
    }

    public void Combust()
    {
        if (IsWet)
            return;

        IsBurning = true;
    }

    public void Drench()
    {
        if (IsBurning)
            IsBurning = false;

        IsWet = true;

    }

    public void Update()
    {
        particleSystem.Update();
    }

    public void Render(ICanvas canvas)
    {
        particleSystem.Render(canvas);
    }
}

class DripParticleProvider : IParticleProvider
{
    public Particle CreateParticle(ParticleEmitter emitter)
    {
        return new()
        {
        };
    }
}