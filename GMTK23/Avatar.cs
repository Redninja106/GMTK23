using GMTK23.Extensions;
using GMTK23.Interactions;
using GMTK23.Particles;
using GMTK23.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23;

internal class Avatar : IGameComponent, ISaveable, IFallable, ICombustable, IWettable
{
    public Transform Transform { get; set; }
    public Vector2 TargetPos { get; set; }
    public float WalkSpeed { get; set; } = 7.5f;
    public ITexture sprite;
    public AvatarState? AvatarState { get; private set; }
    public bool IsAtTarget => this.Transform.Position == TargetPos;
    private ParticleSystem? bloodParticleSystem = null;
    private float timeSinceDeath = 0;
    private ElementalState elementalState;
    private float timeOnFire;
    public bool HasFallen { get; private set; }
    public Torch? torch;
    private Ending? killedEnding;

    public void SetState(AvatarState state)
    {
        this.AvatarState = state;
    }

    public void Kill(Ending ending)
    {
        AvatarState = null;
        this.killedEnding = ending;
        TargetPos = Transform.Position;
    }

    public Avatar(Transform transform)
    {
        Transform = transform;
        TargetPos = transform.Position;
        sprite = Graphics.LoadTexture("./Assets/dude.png");
        
        elementalState = new(this);
        SetState(new MoveToTreeState(this));
    }

    public RenderLayer RenderLayer => RenderLayer.Avatar;

    public void Render(ICanvas canvas)
    {
        canvas.PushState();
        canvas.ApplyTransform(Transform);
        canvas.DrawTexture(sprite, 0, 0, 2, 3);
        canvas.PopState();
        torch?.Render(canvas);
        bloodParticleSystem?.Render(canvas);
        elementalState.Render(canvas);
    }

    public void Update()
    {
        Vector2 v = GMTK23.Extensions.VectorExtensions.StepTowards(Transform.Position, TargetPos, WalkSpeed * Time.DeltaTime);
        Transform.Position = v;

        torch?.Update();

        if (elementalState.IsBurning)
        {
            timeOnFire += Time.DeltaTime;
        }

        if (timeOnFire > 2)
        {
            this.Fall(Ending.QuickAndPainful);
        }

        Boulder b = Program.World.Find<Boulder>();
        if (b.GetBounds().Intersects(this.GetBounds())
            && AvatarState is not (null or Idle))
        {
            if (b.transform.Position.X < this.Transform.Position.X)
            {
                SetState(new Idle(this, Ending.ShouldveRespawn));
            }
            else
            {
                SetState(new Idle(this, null));
            }

            TargetPos = Transform.Position;
        }

        if (AvatarState != null)
        {
            AvatarState.Update();
        }
        else
        {
            timeSinceDeath += Time.DeltaTime;

            if (timeSinceDeath > 1 && bloodParticleSystem is null)
            {
                bloodParticleSystem = new(new BloodParticleProvider(this));
                bloodParticleSystem.Emitter.Burst(50);
                bloodParticleSystem.Emitter.Rate = 10f;
            }
        }

        bloodParticleSystem?.Update();
        elementalState.Update();

        if (timeSinceDeath > 3)
        {
            GameStateManager.AchieveEnding(this.killedEnding);
        }
    }

    public void setTargetPos(Vector2 targetPos)
    {
        this.TargetPos = targetPos;
    }

    public IEnumerable<string> Save()
    {
        yield return Transform.ToString();
    }

    public static IGameComponent Load(ArgReader reader)
    {
        Transform transform = reader.NextTransform();
        Avatar av = new Avatar(transform);
        return av;
    }

    public void Fall()
    {
        Fall(Ending.ItsLikeBrawl);
    }

    public void Fall(Ending ending)
    {
        if (AvatarState == null)
            return;

        this.Transform.Rotation = Angle.ToRadians(90);
        this.TargetPos = new(Transform.Position.X + 3, Transform.Position.Y + 1);
        this.Transform.Position = new(Transform.Position.X + 3, Transform.Position.Y + 1);
        this.TargetPos = this.Transform.Position;
        HasFallen = true;
        AvatarState = null;
        this.killedEnding = ending;
    }

    public Rectangle GetBounds()
    {
        if (HasFallen)
        {
            return new Rectangle(Transform.Position, new(3, 2), Alignment.TopRight);
        }

        return new Rectangle(Transform.Position, new(2, 3));
    }

    public void Drench()
    {
        this.elementalState.Drench();

        if (this.torch is not null)
        {
            if (this.torch.elementalState.IsBurning)
            {
            }

            this.torch.elementalState.Drench();
        }
    }

    public void Combust()
    {
        this.elementalState.Combust();
    }

    class BloodParticleProvider : IParticleProvider
    {
        public IInteractable interactable;
        private Random rng = new();

        public BloodParticleProvider(IInteractable interactable)
        {
            this.interactable = interactable;
        }

        public Particle CreateParticle(ParticleEmitter emitter)
        {
            var bounds = interactable.GetBounds();
            return new()
            {
                transform = new(
                    bounds.X + rng.NextSingle() * bounds.Width,
                    bounds.Y + rng.NextSingle() * bounds.Height,
                    rng.NextSingle() * MathF.Tau
                    ),
                acceleration = Vector2.UnitY * 14,
                angularVelocity = rng.NextSingle() * MathF.Tau,
                color = Color.Red,
                lifetime = 1f + rng.NextSingle(),
                size = .1f + rng.NextSingle() * .1f,
                velocity = new(rng.NextSingle() * 2 - 1, -8)
            };
        }
    }
}

class Torch : IInteractable
{
    public ElementalState elementalState;
    private Avatar avatar;

    public Torch(Avatar avatar, bool startsWet, bool startsFlaming)
    {
        this.avatar = avatar;
        elementalState = new(this, startsFlaming, startsWet);
    }

    public RenderLayer RenderLayer => RenderLayer.Avatar;

    public Rectangle GetBounds()
    {
        return new(avatar.Transform.Position + new Vector2(3, .5f), new(.1f, .1f), Alignment.Center);
    }

    public void Render(ICanvas canvas)
    {
        canvas.PushState();
        canvas.ApplyTransform(avatar.Transform);
        canvas.Fill(new Color(92, 78, 46));
        canvas.StrokeWidth(.25f);
        canvas.DrawLine(2, 2, 3, .5f);
        canvas.PopState();
        elementalState.Render(canvas);
    }

    public void Update()
    {
        elementalState.Update();
    }
}