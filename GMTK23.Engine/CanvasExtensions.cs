using SimulationFramework.Drawing;
using SimulationFramework.SkiaSharp;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23.Engine;
public static class CanvasExtensions
{
    public static void DrawSprite(this ICanvas canvas, Sprite sprite)
    {
        sprite.Render(canvas);
    }
}
