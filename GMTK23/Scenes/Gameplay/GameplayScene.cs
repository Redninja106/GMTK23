using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23.Scenes.Gameplay;
internal class GameplayScene : GameScene
{
    public GameplayScene()
    {
        var avatar = new Avatar();
        avatar.Transform.Position = new Vector2()
        AddComponent();
        Add(new Avatar());
    }
}
