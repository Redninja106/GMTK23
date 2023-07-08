using GMTK23.Scenes.GameplayScene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23.Avatar;
internal class AvatarCharacter : Entity
{
    public Transform Transform { get; }
    
    private World World;

    public AvatarCharacter(Transform transform)
    {
        Transform = transform;
    }

    public void Render(ICanvas canvas)
    {
        Transform.ApplyTo(canvas);
        canvas.DrawRect(0, 0, 1, 2);
    }

    public void Update()
    {
        World ??= GameScene.Active.GetComponent();
    }
}
