using NAudio.Vorbis;
using NAudio.Wave;
using NVorbis;
using Silk.NET.OpenAL;
using SimulationFramework.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework.AudioExtensions.Windows;
internal class WindowsSound : ISound
{
    private readonly WindowsAudioProvider provider;
    internal readonly List<WindowsSoundPlayback> activePlaybacks;
    internal readonly uint buffer;

    public int SampleRate { get; }
    public float Duration { get; }
    public bool IsStereo { get; }
    public bool Is16Bit { get; }

    public unsafe WindowsSound(WindowsAudioProvider provider, ReadOnlySpan<byte> encodedData, AudioFileKind fileKind)
    {
        this.provider = provider;
        activePlaybacks = new();

        fixed (byte* encodedDataPtr = encodedData)
        {
            var stream = new UnmanagedMemoryStream(encodedDataPtr, encodedData.Length);
            WaveStream reader = fileKind switch
            {
                AudioFileKind.Wav => new WaveFileReader(stream),
                AudioFileKind.Mp3 => new Mp3FileReader(stream),
                AudioFileKind.Ogg => new VorbisWaveReader(stream),
                _ => throw new NotSupportedException("Unsupported audio file kind."),
            };

            if (reader.WaveFormat.BitsPerSample is 32)
            {
                // WaveFormat format = new(reader.WaveFormat.SampleRate, 16, reader.WaveFormat.Channels);

                reader = new Wave32To16Stream(reader);
            }

            byte[] data = new byte[reader.Length];
            reader.Read(data.AsSpan());

            buffer = provider.al.GenBuffer();
            fixed (byte* dataPtr = &data[0])
            {
                provider.al.BufferData(buffer, GetBufferFormat(reader.WaveFormat), dataPtr, data.Length, reader.WaveFormat.SampleRate);
            }

            var format = reader.WaveFormat;

            SampleRate = format.SampleRate;
            IsStereo = format.Channels == 2;
            Is16Bit = format.BitsPerSample == 16;

            int sampleSize = Is16Bit ? 2 : 1;
            int sampleCount = data.Length / sampleSize;

            float duration = sampleCount / SampleRate;

            if (IsStereo)
                duration *= .5f;

            Duration = duration;
        }
    }

    private static BufferFormat GetBufferFormat(WaveFormat format)
    {
        if (format.Channels is 1)
        {
            if (format.BitsPerSample == 16)
            {
                return BufferFormat.Mono16;
            }
            else if (format.BitsPerSample == 8)
            {
                return BufferFormat.Mono8;
            }
        }
        else if (format.Channels is 2)
        {
            if (format.BitsPerSample == 16)
            {
                return BufferFormat.Stereo16;
            }
            else if (format.BitsPerSample == 8)
            {
                return BufferFormat.Stereo8;
            }
        }

        throw new Exception("Audio format not supported!");
    }

    public SoundPlayback Play()
    {
        foreach (var playback in activePlaybacks)
        {
            if (playback.IsStopped)
            {
                // activePlaybacks.Remove(playback);
            }
        }

        var newPlayback = new WindowsSoundPlayback(provider, this);
        activePlaybacks.Add(newPlayback);
        return newPlayback;
    }
}
