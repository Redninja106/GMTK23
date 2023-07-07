using SimulationFramework.AudioExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.AudioExtensions;

/// <summary>
/// Represents a sound. Can be played using <see cref="SoundPlayer"/>.
/// </summary>
public interface ISound
{
    int SampleRate { get; }
    float Duration { get; }
    bool IsStereo { get; }
    bool Is16Bit { get; }

    SoundPlayback Play();
}