using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23.Particles;
internal class ParticleEmitter : IPositionable
{
    public Transform Transform { get; } = new();

    public Vector2 Velocity;
    public float AngularVelocity;

    ParticleSystem system;

    public IParticleProvider ParticleProvider { get; set; }
    public float Rate { get; set; } // particles/second

    private float lastParticle;

    public ParticleEmitter(ParticleSystem system, IParticleProvider particleProvider)
    {
        lastParticle = Time.TotalTime;
        this.system = system;
        this.ParticleProvider = particleProvider;
    }

    public void Update()
    {
        if (ParticleProvider is null || Rate is 0)
        {
            lastParticle = Time.TotalTime;
            return;
        }

        float timeSinceParticle = Time.TotalTime - lastParticle;

        float freq = (1f / Rate);

        while (timeSinceParticle > freq)
        {
            Particle p = ParticleProvider.CreateParticle(this);
            system.Add(p);

            timeSinceParticle -= freq;
            lastParticle = Time.TotalTime;
        }
    }

    public void Burst(int count)
    {
        if (ParticleProvider is null)
            return;

        for (int i = 0; i < count; i++)
        {
            system.Add(ParticleProvider.CreateParticle(this));
        }
    }
}
