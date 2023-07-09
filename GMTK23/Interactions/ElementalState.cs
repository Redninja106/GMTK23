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

    private IElementalParticleProvider particleProvider;
    public ParticleSystem particleSystem;
    public IInteractable interactable;

    public ElementalState(IInteractable interactable, bool initiallyBurning = false, bool initiallyWet = false)
    {
        this.interactable = interactable;
        particleSystem = new(null);

        if (initiallyWet)
        {
            Drench();
        }
        else if (initiallyBurning) 
        {
            Combust();
        }
    }

    public void Combust()
    {
        if (IsWet)
            return;

        // fireSoundPlayback = fireSound!.Play();

        IsBurning = true;
        var bounds = this.interactable.GetBounds();
        particleSystem.Emitter.Rate = MathF.Max(10, bounds.Width * bounds.Height);
        particleProvider = new BurningParticleProvider(bounds);
        particleSystem.Emitter.ParticleProvider = this.particleProvider;
    }

    public void Drench()
    {
        if (IsBurning)
            IsBurning = false;

        IsWet = true;
        var bounds = this.interactable.GetBounds();
        particleSystem.Emitter.Rate = MathF.Max(10, .25f * bounds.Width * bounds.Height);
        this.particleProvider = new DripParticleProvider(this.interactable.GetBounds());
        particleSystem.Emitter.ParticleProvider = this.particleProvider;
    }

    public void Update()
    {
        if (particleProvider is not null)
            particleProvider.Bounds = this.interactable.GetBounds();

        particleSystem.Update();
    }

    public void Render(ICanvas canvas)
    {
        particleSystem.Render(canvas);
    }
}

interface IElementalParticleProvider : IParticleProvider
{ 
    Rectangle Bounds { get; set; }
}


internal class BurningParticleProvider : IElementalParticleProvider
{
    public Rectangle Bounds { get; set; }
    Random rng = new();

    public BurningParticleProvider(Rectangle bounds)
    {
        this.Bounds = bounds;
    }

    public Particle CreateParticle(ParticleEmitter emitter)
    {
        if (rng.NextSingle() > .5f)
        {
            return new()
            {
                color = Color.Lerp(Color.Red, Color.Yellow, rng.NextSingle()),
                transform = new Transform(
                       Bounds.X + rng.NextSingle() * Bounds.Width,
                       Bounds.Y + rng.NextSingle() * Bounds.Height,
                       rng.NextSingle() * MathF.Tau
                    ),
                angularVelocity = rng.NextSingle(),
                lifetime = rng.NextSingle() + 1,
                drag = 0,
                velocity = new(rng.NextSingle() * 2 - 1, - 3f),
                size = .3f + rng.NextSingle() * .3f,
            };
        }
        else
        {
            return new()
            {
                color = Color.Lerp(new Color(100,100,100), new Color(140,140,140), rng.NextSingle()),
                transform = new Transform(
                           Bounds.X + rng.NextSingle() * Bounds.Width,
                           Bounds.Y + rng.NextSingle() * Bounds.Height,
                           rng.NextSingle() * MathF.Tau
                        ),
                angularVelocity = rng.NextSingle(),
                lifetime = rng.NextSingle() + 2,
                drag = 0,
                velocity = new(1f + rng.NextSingle(), -8),
                size = .6f + rng.NextSingle() * .6f,
            };
        }
    }
}

class DripParticleProvider : IElementalParticleProvider
{
    public Rectangle Bounds { get; set; }
    Random rng = new();

    public DripParticleProvider(Rectangle bounds)
    {
        this.Bounds = bounds;
    }

    public Particle CreateParticle(ParticleEmitter emitter)
    {
        return new()
        {
            color = Color.Blue,
            transform = new Transform(
                   Bounds.X + rng.NextSingle() * Bounds.Width,
                   Bounds.Y + rng.NextSingle() * Bounds.Height,
                   rng.NextSingle() * MathF.Tau
                ),
            angularVelocity = rng.NextSingle(),
            lifetime = rng.NextSingle() + 1,
            drag = 0,
            acceleration = Vector2.UnitY * 10,
            size = .1f + rng.NextSingle() * .1f,
        };
    }
}