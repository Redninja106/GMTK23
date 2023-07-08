using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23;

internal class Camera : GameObject
{
    public Transform Transform { get; } = new();

    public float AspectRatio => DisplayWidth / (float)DisplayHeight;

    /// <summary>
    /// The height of the camera's visible area, in world space.
    /// </summary>
    public float ViewportHeight { get; set; }

    /// <summary>
    /// The width of the camera's visible area, in world space.
    /// </summary>
    public float ViewportWidth 
    { 
        get => ViewportHeight * AspectRatio; 
        set => ViewportHeight = value / AspectRatio;
    }

    /// <summary>
    /// The width, in pixels, of the camera's output (usually the window).
    /// </summary>
    public int DisplayWidth { get; set; }

    /// <summary>
    /// The width, in pixels, of the camera's output (usually the window).
    /// </summary>
    public int DisplayHeight { get; set; }

    public Camera(float viewportHeight)
    {
        this.ViewportHeight = viewportHeight;
    }

    public override void Update()
    {
        DisplayWidth = Window.Width;
        DisplayHeight = Window.Height;
    }

    public void ApplyTo(ICanvas canvas)
    {
        canvas.Transform(GetWorldToScreenMatrix());
    }

    private MatrixBuilder GetWorldToScreenMatrixBuilder()
    {
        return new MatrixBuilder()
            .Translate(DisplayWidth / 2f, DisplayHeight / 2f)
            .Scale(DisplayHeight / ViewportHeight)
            .Scale(1, -1)
            .Multiply(Transform.GetLocalToWorldMatrix());
    }

    public Matrix3x2 GetScreenToWorldMatrix()
    {
        return GetWorldToScreenMatrixBuilder().InverseMatrix;
    }

    public Matrix3x2 GetWorldToScreenMatrix()
    {
        return GetWorldToScreenMatrixBuilder().Matrix;
    }

    public Vector2 ScreenToWorld(Vector2 point)
    {
        return Vector2.Transform(point, GetScreenToWorldMatrix());
    }

    public Vector2 WorldToScreen(Vector2 point)
    {
        return Vector2.Transform(point, GetWorldToScreenMatrix());
    }
}
