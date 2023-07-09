using GMTK23.Cards;
using GMTK23.Interactions;
using GMTK23.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23;
internal class Cloud : IGameComponent, ISaveable, IInteractable, IFallable
{
    public TileMap tileMap;
    public RenderLayer RenderLayer => tileMap.RenderLayer;
    public Transform transform;
    public Vector2 returnPos;
    public Vector2 targetPos;
    public float fallSpeed = 8;

    public Cloud(Transform transform, TileMap tileMap)
    {
        this.transform = transform;
        this.targetPos = transform.Position;
        this.tileMap = tileMap;
    }

    public void Render(ICanvas canvas)
    {
        tileMap.Render(canvas);
    }

    public void setTargetPos(Vector2 targetPos)
    {
        this.targetPos = targetPos;
    }

    public void Update()
    {
        tileMap.Transform.Match(this.transform);
        tileMap.Update();
        Vector2 v = GMTK23.Extensions.VectorExtensions.StepTowards(transform.Position, targetPos, fallSpeed * Time.DeltaTime);
        transform.Position = v;

        if (this.GetBounds().Intersects(Program.World.Find<Avatar>().GetBounds()))
        {
            Avatar av = Program.World.Find<Avatar>();
            av.Transform = new Transform(transform.Position.X+6,transform.Position.Y,0);
            av.setTargetPos(new Vector2(returnPos.X + 6, returnPos.Y));
            av.WalkSpeed = fallSpeed;
            this.setTargetPos(returnPos);
        }
    }

    public IEnumerable<string> Save()
    {
        return tileMap.Save();
    }

    public static IGameComponent Load(ArgReader reader)
    {
        var tileMap = (TileMap)TileMap.Load(reader);
        return new Cloud(tileMap.Transform, tileMap);
    }

    public Rectangle GetBounds()
    {
        return new Rectangle(transform.Position, new(tileMap.Width, tileMap.Height));
    }

    public void Fall()
    {
        returnPos = transform.Position;
        this.targetPos = new(transform.Position.X, 36);
    }
}

