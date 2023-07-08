﻿using SimulationFramework;
using SimulationFramework.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23.Engine;

public class Sprite
{
    public readonly ITexture texture;
    public Vector2 Size { get; set; }

    public Sprite(string path, Vector2 size)
    {
        Size = size;

        texture = Assets.GetSpriteTexture(path);
    }

    public void Render(ICanvas canvas)
    {
        canvas.DrawTexture(texture, new Rectangle(new(0f, 0f), Size, Alignment.Center));
    }
}