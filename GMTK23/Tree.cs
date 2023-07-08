﻿using GMTK23.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23;
internal class Tree : IGameComponent, ISaveable
{
    public TileMap tileMap;
    public RenderLayer RenderLayer => RenderLayer.World;
    public Transform transform;

    public Tree(Transform transform, TileMap tileMap)
    {
        this.transform = transform;
        this.tileMap = tileMap;
    }

    public void Render(ICanvas canvas)
    {
        tileMap.Render(canvas);
    }

    public void Update()
    {
        tileMap.Transform.Match(this.transform);
        tileMap.Update();
    }

    public IEnumerable<string> Save()
    {
        return tileMap.Save();
    }

    public static IGameComponent Load(ArgReader reader)
    {
        var tileMap = (TileMap)TileMap.Load(reader);
        return new Tree(tileMap.Transform, tileMap);
    }
}
