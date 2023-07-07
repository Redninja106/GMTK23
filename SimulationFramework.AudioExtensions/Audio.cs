namespace SimulationFramework.AudioExtensions;

public static class Audio
{
    private static IAudioProvider Provider => Application.GetComponent<IAudioProvider>();

    public static float Volume 
    { 
        get => Provider.Volume; 
        set 
        {
            if (value < 0)
                throw new InvalidOperationException("Volume cannot be less than zero!");

            Provider.Volume = value; 
        }
    }

    public static ISound LoadSound(string file, AudioFileKind? fileKind = null)
    {
        if (fileKind is null)
        {
            if (file.EndsWith(".wav", StringComparison.OrdinalIgnoreCase))
            {
                fileKind = AudioFileKind.Wav;
            }
            else if (file.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase))
            {
                fileKind = AudioFileKind.Mp3;
            }
            else if (file.EndsWith(".ogg", StringComparison.OrdinalIgnoreCase))
            {
                fileKind = AudioFileKind.Ogg;
            }
            else
            {
                throw new ArgumentException("File type cannot be determined from file name.");
            }
        }

        var data = File.ReadAllBytes(file);
        return LoadSound(data, fileKind.Value);
    }

    public static ISound LoadSound(ReadOnlySpan<byte> encodedData, AudioFileKind fileKind)
    {
        return Provider.LoadSound(encodedData, fileKind);
    }
}