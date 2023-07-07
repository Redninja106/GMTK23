using SimulationFramework.Components;
using SimulationFramework.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.AudioExtensions;
internal interface IAudioProvider : ISimulationComponent
{
    float Volume { get; set; }

    ISound LoadSound(ReadOnlySpan<byte> encodedData, AudioFileKind fileKind);
}
