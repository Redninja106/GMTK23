﻿using SimulationFramework.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23.Extensions;
internal static class CanvasExtensions
{
    public static void ApplyTransform(this ICanvas canvas, Transform transform)
    {
        canvas.Translate(transform.Position);
        canvas.Rotate(transform.Rotation);
        canvas.Scale(transform.Scale);
    }
}