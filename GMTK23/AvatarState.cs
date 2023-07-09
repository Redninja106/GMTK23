using GMTK23;

abstract class AvatarState
{
    public Avatar Avatar { get; private set; }

    public AvatarState(Avatar avatar)
    {
        this.Avatar = avatar;
    }

    public abstract void Update();
}

class MoveToTreeState : AvatarState
{
    public MoveToTreeState(Avatar avatar) : base(avatar)
    {
    }

    public override void Update()
    {
        var tree = Program.World.Find<Tree>();
        Avatar.TargetPos = new Vector2(tree.transform.Position.X - 1, Avatar.Transform.Position.Y);

        if (Avatar.IsAtTarget)
        {
            Avatar.SetState(new MineTreeState(this.Avatar));
        }
    }
}

class MineTreeState : AvatarState
{
    public float startTime;
    public float endTime;

    public MineTreeState(Avatar avatar) : base(avatar)
    {
    }

    public override void Update()
    {
        if (startTime == 0)
        {
            startTime = Time.TotalTime;
            endTime = startTime + 5;
        }

        var tree = Program.World.Find<Tree>();

        if (endTime != 0
            && Time.TotalTime > endTime)
        {
            Avatar.torch = new(Avatar, tree!.elementalState.IsWet, tree!.elementalState.IsBurning); 

            if (Avatar.torch.elementalState.IsBurning)
            {
                Avatar.SetState(new Idle(this.Avatar));
            }
            else
            {
                Avatar.SetState(new MoveToFire(this.Avatar));
            }
        }
    }
}

class MoveToFire : AvatarState
{
    public MoveToFire(Avatar avatar) : base(avatar) 
    {
    }

    public override void Update()
    {
        var fire = Program.World.Find<LogFire>();
        Avatar.TargetPos = new Vector2(fire.transform.Position.X + 4, Avatar.Transform.Position.Y);

        if (Avatar.IsAtTarget)
        {
            Avatar.SetState(new LightTorch(this.Avatar));
        }
    }
}

class LightTorch : AvatarState
{
    float activeTime = 0;

    public LightTorch(Avatar avatar) : base(avatar)
    {
    }

    public override void Update()
    {
        activeTime += Time.DeltaTime;

        if (activeTime > 2)
        {
            Avatar.torch!.elementalState.Combust();
        }
    }
}

class Idle : AvatarState
{
    public Idle(Avatar avatar) : base(avatar) { } 
    
    public override void Update() { }
}