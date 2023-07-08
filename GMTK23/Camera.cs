using SimulationFramework;
using SimulationFramework.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using GMTK23.Extensions;

namespace GMTK23;

internal class Camera
{
    public readonly MatrixBuilder WorldToLocal = new();
    public readonly MatrixBuilder LocalToScreen = new();

    public Transform Transform { get; } = new();

    public float AspectRatio { get; private set; }

    public float VerticalSize { get; set; }
    public float HorizontalSize { get => VerticalSize * AspectRatio; set => VerticalSize = value / AspectRatio; }

    public int DisplayWidth { get; private set; }
    public int DisplayHeight { get; private set; }

    public Camera(float verticalSize)
    {
        VerticalSize = verticalSize;
    }

    public void Update(int displayWidth, int displayHeight)
    {
        this.Transform.Rotation = Angle.Normalize(this.Transform.Rotation);

        this.DisplayWidth = displayWidth;
        this.DisplayHeight = displayHeight;

        AspectRatio = displayWidth / (float)displayHeight;

        LocalToScreen
            .Reset()
            .Translate(displayWidth / 2f, displayHeight / 2f)
            .Scale(displayHeight / VerticalSize);

        WorldToLocal
            .Reset()
            .Rotate(-Transform.Rotation)
            .Translate(-Transform.Position);
    }

    public void ApplyTo(ICanvas canvas)
    {
        canvas.Transform(LocalToScreen.Matrix);
        canvas.Transform(WorldToLocal.Matrix);
    }

    public Vector2 ScreenToWorld(Vector2 point)
    {
        point = Vector2.Transform(point, LocalToScreen.InverseMatrix);
        point = Vector2.Transform(point, WorldToLocal.InverseMatrix);
        return point;
    }
}