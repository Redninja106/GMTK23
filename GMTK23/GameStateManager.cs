using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23;
internal class GameStateManager : IGameComponent
{
    public RenderLayer RenderLayer => RenderLayer.UI;

    private Ending? achievedEnding = null;

    public void Render(ICanvas canvas)
    {
        if (achievedEnding is not null) 
        {
            var center = Program.Camera.Transform.Position;

            canvas.DrawText(achievedEnding.Title, center, Alignment.BottomCenter);
            canvas.DrawText(achievedEnding.Description, center, Alignment.TopCenter);
        }
    }

    public void Update()
    {
        if (Keyboard.IsKeyPressed(Key.F))
        {
            AchieveEnding(Ending.CelestialKidnapping);
        }
    }

    public void AchieveEnding(Ending ending)
    {
        achievedEnding = ending;
    }
}