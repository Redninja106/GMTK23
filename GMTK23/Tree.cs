using GMTK23.Interactions;
using GMTK23.Particles;
using GMTK23.Tiles;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace GMTK23;
internal class Tree : IGameComponent, ISaveable, IFallable, IWettable, ICombustable
{
    public TileMap tileMap;
    public RenderLayer RenderLayer => tileMap.RenderLayer;
    public Transform transform;
    public ElementalState elementalState;
    bool isFallen;
    private ParticleSystem woodChipSystem;

    public Tree(Transform transform, TileMap tileMap)
    {
        this.transform = transform;
        this.tileMap = tileMap;
        elementalState = new(this);
        woodChipSystem = new(new WoodChipParticleProvider(this.transform.Position + new Vector2(4, 16)));
    }

    public void BeginBreaking()
    {
        woodChipSystem.Emitter.Rate = 10;
    }

    public void EndBreaking()
    {
        woodChipSystem.Emitter.Rate = 0;
    }

    public void Render(ICanvas canvas)
    {
        canvas.PushState();
        tileMap.Render(canvas);
        canvas.PopState();
        elementalState.Render(canvas);
        woodChipSystem.Render(canvas);
    }

    public void Update()
    {
        tileMap.Transform.Match(this.transform);
        tileMap.Update();
        elementalState.Update();
        woodChipSystem.Update();
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
        this.transform.Position = new(81, 49);
        isFallen = true;

        var avatar = Program.World.Find<Avatar>();

        if (this.GetBounds().Intersects(avatar.GetBounds()))
        {
            avatar.Fall(Ending.NoOneHeard);
        }
    }

    public void Combust()
    {
        elementalState.Combust();
    }

    public void Drench()
    {
        elementalState.Drench();
    }

    class WoodChipParticleProvider : IParticleProvider
    {
        public Vector2 point;
        Random rng = new();

        public WoodChipParticleProvider(Vector2 point)
        {
            this.point = point;
        }

        public Particle CreateParticle(ParticleEmitter emitter)
        {
            return new()
            {
                transform = new(
                    point.X, point.Y, rng.NextSingle() * MathF.Tau
                    ),
                lifetime = 1f,
                angularVelocity = 5 * (rng.NextSingle() * MathF.Tau - MathF.PI),
                velocity = 5 * Angle.ToVector(Angle.ToRadians(180 + rng.NextSingle() * 90)),
                acceleration = Vector2.UnitY * 8,
                color = new Color(92, 78, 46),
                size = .1f + rng.NextSingle() * .2f,
            };
        }
    }
}
