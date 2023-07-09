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
            Avatar.NextState();
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
            endTime = startTime + 15;
        }

        if (endTime != 0
            && Time.TotalTime > endTime)
        {
            Avatar.hasTorch = true;
            Avatar.NextState();
        }
    }
}

class MoveToFire : AvatarState
{
    public MoveToFire(Avatar avatar) : base(avatar) {
    }
    public override void Update()
    {
        var fire = Program.World.Find<LogFire>();
        Avatar.TargetPos = new Vector2(fire.transform.Position.X + 4, Avatar.Transform.Position.Y);
    }
}