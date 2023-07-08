using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23.Extensions;
internal static class VectorExtensions
{
    public static Vector2 Rotated(this Vector2 vector, float rotation)
    {
        var (s, c) = MathF.SinCos(rotation);
        return new(vector.X * c - vector.Y * s, vector.X * s + vector.Y * c);
    }
}
