using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.Intrinsics;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23;
internal struct Transform
{
    public Vector2 Position;
    public float Rotation;
    public Vector2 Scale;

    public Transform(float x, float y, float rotation) : this(new(x, y), rotation)
    {
    }

    public Transform(Vector2 position, float rotation)
    {
        Position = position;
        Rotation = rotation;
        Scale = Vector2.One;
    }

    public Vector2 Forward
    { 
        get => Vector2.UnitX.Rotated(Rotation); 
        set => Rotation = MathF.Atan2(value.Y, value.X); 
    }
    public Vector2 Backward
    {
        get => (-Vector2.UnitX).Rotated(Rotation);
    }
    public Vector2 Up
    {
        get => Vector2.UnitY.Rotated(Rotation);
    }
    public Vector2 Down
    {
        get => (-Vector2.UnitY).Rotated(Rotation);
    }

    public Transform Translated(Vector2 vector, bool local = true)
    {
        if (local)
            vector = vector.Rotated(this.Rotation);

        return this with { Position = this.Position + vector };
    }

    public Transform Rotated(float rotation)
    {
        return this with { Rotation = this.Rotation + rotation };
    }

    public Vector2 WorldToLocal(Vector2 point)
    {
        var matrix = Matrix3x2.CreateScale(Vector2.One / this.Scale);
        matrix = matrix.Append(Matrix3x2.CreateRotation(-this.Rotation));
        matrix = matrix.Append(Matrix3x2.CreateTranslation(-this.Position));
        return Vector2.Transform(point, matrix);
    }

    public Vector2 LocalToWorld(Vector2 point)
    {
        var matrix = Matrix3x2.CreateTranslation(this.Position);
        matrix = matrix.Append(Matrix3x2.CreateRotation(this.Rotation));
        matrix = matrix.Append(Matrix3x2.CreateScale(this.Scale));
        return Vector2.Transform(point, matrix);
    }


    public override string ToString()
    {
        return $"({Position.X},{Position.Y},{Rotation})";
    }

    public static Transform Parse(string str)
    {
        var parts = str.Trim('(', ')').Split(',');
        float x = float.Parse(parts[0]);
        float y = float.Parse(parts[1]);
        float r = float.Parse(parts[2]);
        return new(x, y, r);
    }
}
