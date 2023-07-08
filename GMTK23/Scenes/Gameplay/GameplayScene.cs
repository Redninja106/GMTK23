using GMTK23.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23.Scenes.GameplayScene;
internal class GameplayScene : GameScene
{
    public GameplayScene() : base(new Camera(20))
    {
        AddComponent(
            new World(new(0, -5, 0)),
            new Avatar(new(0, -4, 0)),
            new Tree(new(10, -5, 0))
            );
    }
}

class World : Entity
{
    private readonly List<WorldSegment> segments = new()
    {
        new WorldSegment(0, 0),
        new WorldSegment(0, 0),
        new WorldSegment(0, 0),
        new WorldSegment(0, 0),
    };
    public Transform Transform { get; }

    public World(Transform transform)
    {
        this.Transform = transform;
    }

    public void Render(ICanvas canvas)
    {
        Transform.ApplyTo(canvas);
        foreach (var s in segments)
        {
            canvas.PushState();
            s.Render(canvas);
            canvas.PopState();
            canvas.Translate(s.Width, 0);
        }
    }

    public void Update()
    {
    }
}

class WorldSegment
{
    public float LeftHeight { get; } = 1;
    public float RightHeight { get; } = 1;
    public float Width { get; } = 5;

    public WorldSegment(float leftHeight, float rightHeight)
    {
        this.LeftHeight = leftHeight;
        this.RightHeight = rightHeight;
    }

    public void Render(ICanvas canvas)
    {
        canvas.Fill(Color.Brown);
        Span<Vector2> poly = stackalloc Vector2[4];
        poly[0] = new(0, LeftHeight);
        poly[1] = new(Width, RightHeight);
        poly[2] = new(Width, -10);
        poly[3] = new(0, -10);

        canvas.DrawPolygon(poly);
    }
} 