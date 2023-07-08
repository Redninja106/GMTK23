using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23.Components.Player;
internal class CameraController : Component
{
    Camera camera;
    bool isDragging;
    Vector2 lastMousePos;

    public override void Initialize(Entity parent)
    {
        camera = GetSibling<Camera>() ?? throw new Exception();
        lastMousePos = Mouse.Position;
    }


    public override void Update()
    {
        if (!isDragging && Mouse.IsButtonPressed(MouseButton.Left))
        {
            isDragging = true;
        }

        if (isDragging && Mouse.IsButtonReleased(MouseButton.Left))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            var diff = camera.ScreenToWorld(Mouse.Position);
            this.ParentTransform.Position.X += diff.X;
        }

        lastMousePos = camera.ScreenToWorld(Mouse.Position);
    }
}
