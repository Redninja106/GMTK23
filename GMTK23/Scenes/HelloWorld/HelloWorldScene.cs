using SimulationFramework.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23.Scenes.HelloWorld;
internal class HelloWorldScene : GameScene
{
    public override void Render(ICanvas canvas)
    {
        canvas.Translate(canvas.Width / 2f, canvas.Height / 2f);
        canvas.DrawText("Hello world!", Vector2.Zero, Alignment.Center);

        base.Render(canvas);
    }

    public override void Update()
    {
        base.Update();
    }
}
