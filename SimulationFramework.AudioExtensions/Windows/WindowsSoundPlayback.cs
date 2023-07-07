using Silk.NET.OpenAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.AudioExtensions.Windows;
internal unsafe class WindowsSoundPlayback : SoundPlayback, IDisposable
{
    uint source;
    WindowsAudioProvider provider;

    public override bool IsStopped
    {
        get
        {
            provider.al.GetSourceProperty(source, GetSourceInteger.SourceState, out int sourceState);
            return (SourceState)sourceState == SourceState.Stopped;
        }
    }

    public override bool IsPaused
    {
        get
        {
            provider.al.GetSourceProperty(source, GetSourceInteger.SourceState, out int sourceState);
            return (SourceState)sourceState == SourceState.Paused;
        }
    }

    public WindowsSoundPlayback(WindowsAudioProvider provider, WindowsSound sound)
    {
        this.provider = provider;
        source = provider.al.GenSource();
        fixed (uint* bufferPtr = &sound.buffer)
        {
            provider.al.SourceQueueBuffers(source, 1, bufferPtr);
        }
        provider.al.SourcePlay(source);
    }


    public override void Dispose()
    {
        provider.al.DeleteSource(source);
    }

    public override void Pause()
    {
        provider.al.SourcePause(source);
    }

    public override void Resume()
    {
        provider.al.SourcePlay(source);
    }

    public override void Stop()
    {
        provider.al.SourceStop(source);
    }
}
