using GMTK23.Cards;
using GMTK23.Interactions;
using GMTK23.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23;
internal class Boulder : IGameComponent, ISaveable, IInteractable, IFallable
{
    public TileMap tileMap;
    public RenderLayer RenderLayer => tileMap.RenderLayer;
    public Transform transform;

    public Boulder(Transform transform, TileMap tileMap)
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
        return new Boulder(tileMap.Transform, tileMap);
    }

    public Rectangle GetBounds()
    {
        return new Rectangle(transform.Position, new(tileMap.Width, tileMap.Height));
    }

    public void Fall()
    {
        this.transform.Position = new(35, 33);

        Avatar av = Program.World.Find<Avatar>();
        if (this.GetBounds().Intersects(av.GetBounds()))
        {
            av.Kill();
        }
    }
}

