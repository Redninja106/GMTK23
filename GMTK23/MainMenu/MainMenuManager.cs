using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23.MainMenu;
internal class MainMenuManager : IGameComponent
{
    public RenderLayer RenderLayer => RenderLayer.UI;
    private List<Button> buttons;
    public EndingsMenu? endings = null;
    public float elapsedTime = 0;

    public MainMenuManager()
    {
        buttons = new()
        {
            new(new(0, .75f, 2f, .5f, Alignment.Center), "Play", () => Program.ReloadLevel("./Levels/newgame.lvl")),
            new(new(0, 1.5f, 2f, .5f, Alignment.Center), "Endings", () => endings = new(this)),
        };
    }

    public void Render(ICanvas canvas)
    {
        if (endings is not null)
        {
            endings.Render(canvas);
            return;
        }

        canvas.PushState();
        var center = Program.World.Find<CameraWaypoint>()!.TargetTransform.Position;
        canvas.Translate(center);

        canvas.Fill(new Color(.9f, .9f, .9f));
        canvas.FontStyle(2.0f, FontStyle.Bold);
        canvas.DrawText("Torch", 0, -1, Alignment.BottomCenter);

        buttons.ForEach(b => b.Render(canvas));
        canvas.PopState();

        canvas.Fill(new Color(0, 0, 0, 1f - MathF.Min(1, elapsedTime)));
        canvas.DrawRect(Program.Camera.Transform.Position, new(Program.Camera.HorizontalSize, Program.Camera.VerticalSize), Alignment.Center);
    }

    public void Update()
    {
        if (endings is not null)
        {
            endings.Update();
            return;
        }

        elapsedTime += Time.DeltaTime;

        var center = Program.World.Find<CameraWaypoint>()!.TargetTransform.Position;
        buttons.ForEach(b => b.Update(center));
    }
}

class Button
{
    public Rectangle bounds;
    public string desc;
    public Action click;
    private bool hover;

    public Button(Rectangle bounds, string desc, Action click)
    {
        this.bounds = bounds;
        this.desc = desc;
        this.click = click;
    }

    public void Render(ICanvas canvas)
    {
        canvas.Fill(new Color(0, 0, 0, (byte)(hover ? 200 : 100)));
        canvas.DrawRect(bounds);

        canvas.Fill(new Color(.75f, .75f, .75f));
        canvas.FontStyle(.30f, FontStyle.Normal);
        canvas.DrawText(desc, bounds.GetAlignedPoint(Alignment.Center), Alignment.Center);
    }

    public void Update(Vector2 offset)
    {
        hover = bounds.ContainsPoint(Program.MousePosition - offset);

        if (hover && Mouse.IsButtonReleased(MouseButton.Left))
        {
            click();
        }
    }
}

class EndingsMenu
{
    private float scroll = 2;
    private float targetScroll = 2;
    private Button backButton;

    public EndingsMenu(MainMenuManager mainMenuManager)
    {
        backButton = new(new(-8, .5f, 2f, .5f, Alignment.Center), "back", () => mainMenuManager.endings = null);
    }

    public void Render(ICanvas canvas)
    {
        var top = Program.World.Find<CameraWaypoint>()!.TargetTransform.Position - new Vector2(0, Program.Camera.VerticalSize/2f);
        canvas.Translate(top);

        backButton.Render(canvas);
        var visibiltyManager = Program.World.Find<EndingVisibilityManager>();
        canvas.DrawText($"Endings: {visibiltyManager.GetAchievedCount()}/12", 8, .5f, Alignment.Center);

        canvas.Translate(0, scroll);

        foreach (var ending in Ending.AllEndings)
        {
            DrawEnding(canvas, ending);
            canvas.Translate(0, 3f);
        }
    }

    public void Update()
    {
        var top = Program.World.Find<CameraWaypoint>()!.TargetTransform.Position - new Vector2(0, Program.Camera.VerticalSize / 2f);
        backButton.Update(top);

        targetScroll += Mouse.ScrollWheelDelta;
        targetScroll = Math.Clamp(targetScroll, -30.5f, 2);

        scroll = MathHelper.Lerp(scroll, targetScroll, MathUtils.TimescaledLerpFactor(15, Time.DeltaTime));
    }

    public void DrawEnding(ICanvas canvas, Ending ending)
    {
        var bounds = new Rectangle(0, 0, 9, 2.5f, Alignment.Center);

        var visibiltyManager = Program.World.Find<EndingVisibilityManager>();
        bool visible = visibiltyManager!.IsVisible(ending);

        canvas.PushState();

        canvas.Fill(new Color(0, 0, 0, 150));
        canvas.DrawRect(bounds);

        canvas.Fill(new Color(.75f, .75f, .75f));
        canvas.FontStyle(.5f, FontStyle.Bold);
        canvas.DrawText(visible ? ending.Title : "???", bounds.Position + Vector2.One * .25f);

        canvas.PushState();
        canvas.Translate(0, .6f);

        canvas.Fill(new Color(.75f, .75f, .75f));
        canvas.FontStyle(.25f, FontStyle.Italic);
        canvas.DrawText(visible ? ending.Description : "???", bounds.Position + Vector2.One * .25f);
        canvas.PopState();

        // if (visible)
        // {
        //     canvas.Fill(new Color(.75f, .75f, .75f));
        //     canvas.FontStyle(.15f, FontStyle.Italic);
        // 
        //     canvas.DrawText($"First achieved with {visibiltyManager.GetAchievedUsername(ending)}", bounds.GetAlignedPoint(Alignment.BottomLeft) + new Vector2(.25f, -.25f), Alignment.BottomLeft);
        // }

        canvas.PopState();
    }
}