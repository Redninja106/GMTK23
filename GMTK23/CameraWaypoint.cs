using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23;
internal class CameraWaypoint : IGameComponent, ISaveable
{
    public bool Active { get; set; } = true;
    public RenderLayer RenderLayer { get; }
    public Transform TargetTransform { get; set; }
    public float TargetSize { get; set; }
    public float Smoothing { get; set; }

    public CameraWaypoint(Transform targetTransform, float targetSize, float smoothing)
    {
        TargetTransform = targetTransform;
        TargetSize = targetSize;
        Smoothing = smoothing;
    }

    public void Render(ICanvas canvas)
    {
    }

    public void Update()
    {
        if (!Active)
            return;

        float edgeDist = MathF.Min(0, TargetTransform.Position.X - Program.Camera.HorizontalSize / 2f);
        float lerpFactor = MathUtils.TimescaledLerpFactor(Smoothing, Time.DeltaTime);
        Program.Camera.Transform.LerpTowards(TargetTransform.Translated(Vector2.UnitX * -edgeDist), lerpFactor);
        Program.Camera.VerticalSize = MathHelper.Lerp(Program.Camera.VerticalSize, TargetSize, lerpFactor);

    }

    public IEnumerable<string> Save()
    {
        yield return TargetTransform.ToString();
        yield return TargetSize.ToString();
        yield return Smoothing.ToString();
    }

    public static IGameComponent Load(ArgReader reader)
    {
        var transform = reader.NextTransform();
        float targetSize = reader.NextFloat();
        float smoothing = reader.NextFloat();
        return new CameraWaypoint(transform, targetSize, smoothing);
    }
}
