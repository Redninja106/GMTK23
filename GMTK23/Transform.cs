using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23;

/// <summary>
/// A position/rotation/scale in world space.
/// </summary>
public class Transform
{
    private Vector2 position;
    private float rotation;
    private Vector2 scale;

    public ref Vector2 Position => ref position;
    public ref float Rotation => ref rotation;
    public ref Vector2 Scale => ref scale;

    public Transform() : this(Vector2.Zero, 0f, Vector2.One)
    {

    }

    public Transform(Vector2 position, float rotation, Vector2 scale)
    {
        this.position = position;
        this.rotation = rotation;
        this.scale = scale;
    }

    public Transform(float x, float y, float rotation, float xScale = 1, float yScale = 1) : this(new(x, y), rotation, new(xScale, yScale))
    {
    }


    public Matrix3x2 GetWorldToLocalMatrix()
    {
        // if this is changed the GetLocalToWorldMatrix() method should also be changed
        Matrix3x2 result = Matrix3x2.Identity;
        result = result.Append(Matrix3x2.CreateTranslation(position));
        result = result.Append(Matrix3x2.CreateRotation(rotation));
        result = result.Append(Matrix3x2.CreateScale(scale));
        return result;
    }

    public Matrix3x2 GetLocalToWorldMatrix()
    {
        // if this is changed the GetWorldToLocalMatrix() method should also be changed
        Matrix3x2 result = Matrix3x2.Identity;
        result = result.Append(Matrix3x2.CreateRotation(-rotation));
        result = result.Append(Matrix3x2.CreateTranslation(-position));
        result = result.Append(Matrix3x2.CreateScale(Vector2.One / scale));
        return result;
    }

    public void ApplyTo(ICanvas canvas)
    {
        canvas.Transform(GetWorldToLocalMatrix());
    }

    public Vector2 WorldToLocal(Vector2 point)
    {
        return Vector2.Transform(point, GetWorldToLocalMatrix());
    }

    public Vector2 LocalToWorld(Vector2 point)
    {
        return Vector2.Transform(point, GetLocalToWorldMatrix());
    }
}