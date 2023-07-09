using GMTK23.Cards;
using GMTK23.Interactions;
using GMTK23.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23;
internal class LogFire : IGameComponent, ISaveable, IWettable
{
    public TileMap tileMap;
    public RenderLayer RenderLayer => tileMap.RenderLayer;
    public Transform transform;
    private ElementalState elementalState;

    public LogFire(Transform transform, TileMap tileMap)
    {
        this.transform = transform;
        this.tileMap = tileMap;
        elementalState = new(this, initiallyBurning: true);
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
        return new LogFire(tileMap.Transform, tileMap);
    }

    public Rectangle GetBounds()
    {
        return new Rectangle(transform.Position, new(tileMap.Width, tileMap.Height));
    }

    public void Drench()
    {
        this.elementalState.Drench();
    }
}

