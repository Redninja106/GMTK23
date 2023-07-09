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
        var tree = Program.World.Find<Tree>();
        
        if (startTime == 0)
        {
            startTime = Time.TotalTime;
            endTime = startTime + 5;
            tree!.BeginBreaking();
        }

        if (endTime != 0
            && Time.TotalTime > endTime)
        {

            tree!.EndBreaking();
            Avatar.torch = new(Avatar.Transform, tree!.elementalState.IsWet, tree!.elementalState.IsBurning); 

            if (!Avatar.torch.elementalState.IsBurning)
            {
                Avatar.SetState(new MoveToFire(this.Avatar));
            }
            else
            {
                Avatar.SetState(new Idle(Avatar, Ending.ThatWasEasy));
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
        Avatar.TargetPos = new Vector2(fire!.transform.Position.X + 4, Avatar.Transform.Position.Y);

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
            var fire = Program.World.Find<LogFire>();
            if (fire!.elementalState.IsBurning)
            {
                if (Avatar.torch!.elementalState.IsWet)
                {
                    GameStateManager.AchieveEnding(Ending.StickySituation);
                }
                else
                {
                    Avatar.torch!.elementalState.Combust();
                    GameStateManager.AchieveEnding(Ending.RoleRevised);
                }
            }
            else
            {
                GameStateManager.AchieveEnding(Ending.DampenedDreams);
            }
        }
    }
}

class Idle : AvatarState
{
    private Ending? ending;
    private float time;

    public Idle(Avatar avatar, Ending? ending) : base(avatar) 
    {
        this.ending = ending;
    } 
    
    public override void Update() 
    {
        if (ending is not null)
        {
            time += Time.DeltaTime;

            if (time > 2)
            {
                GameStateManager.AchieveEnding(ending);
            }
        }
    }
}