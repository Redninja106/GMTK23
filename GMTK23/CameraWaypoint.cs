using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23;
internal class CameraWaypoint : IGameComponent, ISaveable
{
    public bool Active { get; set; }
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
        float lerpFactor = MathUtils.TimescaledLerpFactor(Smoothing, Time.DeltaTime);
        Program.Camera.Transform.LerpTowards(TargetTransform, lerpFactor);
        Program.Camera.VerticalSize = MathHelper.Lerp(Program.Camera.VerticalSize, TargetSize, lerpFactor); 
    }

    public IEnumerable<string> Save()
    {
        yield return TargetTransform.ToString();
    }

    public static IGameComponent Load(ArgReader reader)
    {
        var transform = reader.NextTransform();
        float targetSize = reader.NextFloat();
        float smoothing = reader.NextFloat();
        return new CameraWaypoint(transform, targetSize, smoothing);
    }
}
