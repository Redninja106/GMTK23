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
    private float elapsedTime = 0;
    private float timeSinceEnding;

    public void Render(ICanvas canvas)
    {
        if (achievedEnding is not null) 
        {
            var center = Program.Camera.Transform.Position;

            canvas.PushState();
            canvas.FontStyle(5f, FontStyle.Bold);
            canvas.Fill(new Color(0, 0, 0, 100));
            canvas.DrawRect(center, new(128, 40), Alignment.Center);

            canvas.Fill(new Color(225, 225, 225));
            
            canvas.DrawText(achievedEnding.Title, center, Alignment.BottomCenter);
            canvas.Translate(0, 2.5f);

            canvas.FontStyle(3f, FontStyle.Italic);
            canvas.DrawText(achievedEnding.Description, center, Alignment.TopCenter);
            canvas.PopState();
        }

        canvas.Fill(new Color(0, 0, 0, 1f - MathF.Min(1, elapsedTime)));
        canvas.DrawRect(Program.Camera.Transform.Position, new(Program.Camera.HorizontalSize, Program.Camera.VerticalSize), Alignment.Center);

        if (timeSinceEnding > 5)
        {
            float fadeoutTime = 1;

            canvas.Fill(new Color(0, 0, 0, MathF.Min(1, (timeSinceEnding - 5) / fadeoutTime)));
            canvas.DrawRect(Program.Camera.Transform.Position, new(Program.Camera.HorizontalSize, Program.Camera.VerticalSize), Alignment.Center);
            if (timeSinceEnding > fadeoutTime + 5)
            {
                Program.ReloadLevel("./Levels/mainmenu.lvl");
            }
        }
    }

    public void Update()
    {
        if (Keyboard.IsKeyPressed(Key.R))
        {
            Program.ReloadLevel(Program.CurrentLevelPath);
        }

        elapsedTime += Time.DeltaTime;

        if (achievedEnding is not null)
        {
            timeSinceEnding += Time.DeltaTime;
        }
    }

    public static void AchieveEnding(Ending ending)
    {
        var manager = Program.World.Find<GameStateManager>();

        if (manager.achievedEnding is not null)
            return;
        
        manager!.achievedEnding = ending;
        
        var visManager = Program.World.Find<EndingVisibilityManager>();
        visManager?.OnEndingAchieved(ending, "abcdef");
    }
}