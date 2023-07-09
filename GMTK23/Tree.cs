﻿using GMTK23.Interactions;
using GMTK23.Tiles;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23;
internal class Tree : IGameComponent, ISaveable, IFallable, IWettable, ICombustable
{
    public TileMap tileMap;
    public RenderLayer RenderLayer => tileMap.RenderLayer;
    public Transform transform;
    public ElementalState elementalState;
    bool isFallen;

    public Tree(Transform transform, TileMap tileMap)
    {
        this.transform = transform;
        this.tileMap = tileMap;
        elementalState = new(this);
    }

    public void Render(ICanvas canvas)
    {
        canvas.PushState();
        tileMap.Render(canvas);
        canvas.PopState();
        elementalState.Render(canvas);
    }

    public void Update()
    {
        tileMap.Transform.Match(this.transform);
        tileMap.Update();
        elementalState.Update();
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

    public Rectangle GetBounds()
    {
        if (isFallen)
        {
            return new Rectangle(transform.Position, new(tileMap.Height, tileMap.Width), Alignment.BottomLeft);
        }

        return new Rectangle(transform.Position, new(tileMap.Width, tileMap.Height));
    }

    public void Fall()
    {
        this.transform.Rotation = Angle.ToRadians(-90);
        this.transform.Position = new(80, 49);
        isFallen = true;
        // do a player hit test
        // if success kill player
        //      avatar.kill("tree")
    }

    public void Combust()
    {
        elementalState.Combust();
    }

    public void Drench()
    {
        elementalState.Drench();
    }
}
