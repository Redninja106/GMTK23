using Silk.NET.OpenAL;
using SimulationFramework.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.AudioExtensions.Windows;
public unsafe class WindowsAudioProvider : IAudioProvider
{
    public ALContext alc = ALContext.GetApi();
    public AL al = AL.GetApi();

    Device* device;
    Context* context;

    public float Volume
    {
        get
        {
            al.GetListenerProperty(ListenerFloat.Gain, out float gain);
            return gain;
        }
        set
        {
            al.SetListenerProperty(ListenerFloat.Gain, value);
        }
    }

    public void Dispose()
    {
        alc.DestroyContext(context);
        alc.CloseDevice(device);
    }

    public void Initialize(MessageDispatcher dispatcher)
    {
        device = alc.OpenDevice(null);

        if (device == null)
        {
            throw new Exception("unable to get openal device");
        }

        context = alc.CreateContext(device, null);
        alc.MakeContextCurrent(context);
    }

    public ISound LoadSound(ReadOnlySpan<byte> encodedData, AudioFileKind fileKind)
    {
        return new WindowsSound(this, encodedData, fileKind);
    }
}
