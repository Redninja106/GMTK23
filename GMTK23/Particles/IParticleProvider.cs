using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23.Particles;
internal interface IParticleProvider
{
    Particle CreateParticle(ParticleEmitter emitter);
}
