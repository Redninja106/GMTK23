using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23.Particles;
internal class RandomAreaParticleProvider : IParticleProvider
{
    Random rng = new();
    public float AreaWidth;
    public float AreaHeight;

    public Particle CreateParticle(ParticleEmitter emitter)
    {
        return new()
        {
            transform = new(
                emitter.Transform.Position.X + rng.NextSingle() * AreaWidth,
                emitter.Transform.Position.Y + rng.NextSingle() * AreaHeight,
                emitter.Transform.Rotation + rng.NextSingle() * MathF.Tau
                ),
            angularVelocity = rng.NextSingle(),
            velocity = new(rng.NextSingle() * 2 - 1, rng.NextSingle() * 2 - 1),
            color = Color.FromHSV(rng.NextSingle(), 1, 1),
            size = rng.NextSingle() * .1f + .1f,
            lifetime = rng.NextSingle() * 2 + 2,
        };
    }
}
