using GMTK23.Cards;
using GMTK23.Interactions;
using GMTK23.Particles;
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
    private ParticleSystem? caveSmoke;
    private SmokeParticleProvider? particleProvider;
    private float timeSinceFall;
    private bool hasFallen;

    public Boulder(Transform transform, TileMap tileMap)
    {
        this.transform = transform;
        this.tileMap = tileMap;
    }

    public void Render(ICanvas canvas)
    {
        canvas.PushState();
        tileMap.Render(canvas);
        canvas.PopState();

        caveSmoke?.Render(canvas);

        if (particleProvider is not null)
        {
            float progress = timeSinceFall / 3;
            progress = MathF.Min(progress, 1);
            caveSmoke!.Emitter.Rate = progress * 150;
            particleProvider.Bounds.Height = progress * 21;
        }
    }

    public void Update()
    {
        tileMap.Transform.Match(this.transform);
        tileMap.Update();
        caveSmoke?.Update();

        if (hasFallen)
            timeSinceFall += Time.DeltaTime;

        Avatar av = Program.World.Find<Avatar>();
        if (av.Transform.Position.X < this.transform.Position.X && timeSinceFall > 3f)
        {
            av.Fall();
        }
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
        if (hasFallen)
            return;

        this.transform.Position = new(35, 33);
        hasFallen = true;

        particleProvider = new SmokeParticleProvider();
        caveSmoke = new(particleProvider);

        Avatar av = Program.World.Find<Avatar>();
        if (this.GetBounds().Intersects(av.GetBounds()))
        {
            av.Kill();
        }

    }

    class SmokeParticleProvider : IParticleProvider
    {
        public Rectangle Bounds = new(2, 23, 29, 0);
        Random rng = new();

        public Particle CreateParticle(ParticleEmitter emitter)
        {
            return new()
            {
                color = Color.Lerp(new Color(100, 100, 100), new Color(140, 140, 140), rng.NextSingle()),
                transform = new Transform(
                                       Bounds.X + rng.NextSingle() * Bounds.Width,
                                       Bounds.Y + rng.NextSingle() * Bounds.Height,
                                       rng.NextSingle() * MathF.Tau
                                    ),
                angularVelocity = rng.NextSingle(),
                lifetime = rng.NextSingle() + 2,
                drag = 0,
                velocity = new(rng.NextSingle() * 2 - 1, rng.NextSingle() * 2 - 1),
                size = .6f + rng.NextSingle() * .6f,
            };
        }
    }
}

