using GMTK23.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23.MainMenu;
internal class FixedAvatarRenderer : IGameComponent, ISaveable
{
    public Transform Transform = new();
    public ITexture sprite;
    public Torch Torch;

    public FixedAvatarRenderer(Transform transform)
    {
        Transform = transform;
        sprite = Graphics.LoadTexture("Assets/dude.png");
        Torch = new(this.Transform, false, true);
    }

    public RenderLayer RenderLayer => RenderLayer.Avatar;

    public static IGameComponent Load(ArgReader reader)
    {
        return new FixedAvatarRenderer(reader.NextTransform());
    }

    public void Render(ICanvas canvas)
    {
        canvas.PushState();
        canvas.ApplyTransform(Transform);
        canvas.DrawTexture(sprite, 0, 0, 2, 3);
        canvas.PopState();
        Torch?.Render(canvas);
    }

    public IEnumerable<string> Save()
    {
        yield return Transform.ToString();
    }

    public void Update()
    {
        Torch?.Update();
    }
}
