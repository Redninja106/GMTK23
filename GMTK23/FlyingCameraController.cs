using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23;
internal class FlyingCameraController : IGameComponent
{
    public RenderLayer RenderLayer { get; }
    float zoom;

    public void Render(ICanvas canvas)
    {
    }

    public void Update()
    {
        Vector2 delta = Vector2.Zero;

        if (Keyboard.IsKeyDown(Key.W))
            delta -= Program.Camera.Transform.Up;
        if (Keyboard.IsKeyDown(Key.S))
            delta -= Program.Camera.Transform.Down;
        if (Keyboard.IsKeyDown(Key.A))
            delta -= Program.Camera.Transform.Forward;
        if (Keyboard.IsKeyDown(Key.D))
            delta -= Program.Camera.Transform.Backward;

        float speed = 1;
        if (Keyboard.IsKeyDown(Key.LeftShift))
            speed = 5;

        Program.Camera.Transform.Position += speed * delta * 5 * Time.DeltaTime;

        zoom += Mouse.ScrollWheelDelta;

        Program.Camera.VerticalSize = MathF.Pow(1.1f, -zoom);
    }
}
