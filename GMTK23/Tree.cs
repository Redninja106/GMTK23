using GMTK23.Interactions;
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

    public Rectangle GetBounds()
    {
        return new Rectangle(transform.Position, new(tileMap.Width, tileMap.Height));
    }

    public void Fall()
    {
        this.transform.Rotation = Angle.ToRadians(-90);
        this.transform.Position = new(80, 49);

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
