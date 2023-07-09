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

internal class Avatar : IGameComponent, ISaveable, IFallable
{
    public Transform Transform { get; set; }
    public Vector2 TargetPos { get; set; }
    public float WalkSpeed { get; set; } = 5;
    public ITexture sprite;
    public bool hasTorch = false;
    public AvatarState? AvatarState { get; private set; }
    public bool IsAtTarget => this.Transform.Position == TargetPos;
    private int currentStateIndex = -1;
    private AvatarState?[] states;
    private ParticleSystem? bloodParticleSystem = null;
    private float timeSinceDeath = 0;

    public void NextState()
    {
        currentStateIndex++;
        AvatarState = states[currentStateIndex];
    }

    public Avatar(Transform transform)
    {
        Transform = transform;
        TargetPos = transform.Position;
        sprite = Graphics.LoadTexture("./Assets/dude.png");
        states = new AvatarState?[]
        {
            new MoveToTreeState(this),
            new MineTreeState(this),
            new MoveToFire(this),
            null
        };
        NextState();
    }

    public RenderLayer RenderLayer => RenderLayer.Avatar;

    public void Render(ICanvas canvas)
    {
        canvas.PushState();
        canvas.ApplyTransform(Transform);
        canvas.DrawTexture(sprite, 0, 0, 2, 3);
        if (hasTorch)
        {
            canvas.Fill(new Color(92, 78, 46));
            canvas.StrokeWidth(.25f);
            canvas.DrawLine(2,2,3,.5f);
        }
        canvas.PopState();
        bloodParticleSystem?.Render(canvas);
    }

    public void Update()
    {
        Vector2 v = GMTK23.Extensions.VectorExtensions.StepTowards(Transform.Position, TargetPos, WalkSpeed * Time.DeltaTime);
        Transform.Position = v;
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
                bloodParticleSystem.Emitter.Burst(100);
            }
        }
        bloodParticleSystem?.Update();
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
        this.Transform.Rotation = Angle.ToRadians(90);
        this.TargetPos = new(Transform.Position.X + 3, Transform.Position.Y + 1);
        this.Transform.Position = new(Transform.Position.X + 3, Transform.Position.Y + 1);
        this.TargetPos = this.Transform.Position;
        AvatarState = null;
    }

    public Rectangle GetBounds()
    {
        if (AvatarState is null)
        {
            return new Rectangle(Transform.Position, new(3, 2), Alignment.TopRight);
        }

        return new Rectangle(Transform.Position, new(2, 3));
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