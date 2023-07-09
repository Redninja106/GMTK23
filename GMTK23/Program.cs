using Silk.NET.Input.Glfw;
using Silk.NET.Windowing.Glfw;
using SimulationFramework;
using SimulationFramework.Desktop;
using SimulationFramework.Drawing;
using System.Diagnostics;
using System.Numerics;
using GMTK23;
using GMTK23.Particles;

new Program().Run(new DesktopPlatform());

partial class Program : Simulation
{
    public static World World { get; private set; }

    private static World? nextWorld = null;

    public static Camera Camera { get; private set; }
    public static Vector2 MousePosition { get; private set; }
    public static Inspector Inspector { get; private set; }
    public static SceneViewer SceneViewer { get; private set; }
    public static string CurrentLevelPath { get; private set; }
    public static bool NoIntro = false;

    public override void OnInitialize()
    {
        Camera = new(15);
        Inspector = new();
        SceneViewer = new();

        ReloadLevel("./Levels/mainmenu.lvl");
        
        // Window.Title = "TANKS!";
        Time.MaxDeltaTime = 1 / 30f;

        Window.Maximize();
        Window.Title = "Torch";
    }

    public override void OnRender(ICanvas canvas)
    {
        if (nextWorld is not null)
            World = nextWorld;

        if (Keyboard.IsKeyPressed(Key.Esc))
        {
            ReloadLevel("./Levels/mainmenu.lvl");
        }

        // float vol = Audio.Volume;
        // ImGui.DragFloat("volume", ref vol, 0.01f, 0, 1);
        // Audio.Volume = vol;

        canvas.ResetState();
        canvas.Clear(Color.FromHSV(0,0,0f));

        if (Window.IsMinimized)
        {
            return;
        }

        var waypoint = World.Find<CameraWaypoint>();
        if (waypoint is not null)
        {
            waypoint.Update();
        }

        Camera.Update(canvas.Width, canvas.Height);

        MousePosition = Vector2.Transform(Mouse.Position, Camera.LocalToScreen.InverseMatrix * Camera.WorldToLocal.InverseMatrix);
        
        SceneViewer.Layout();
        Inspector.Layout();
        World.Update();

        Camera.Update(canvas.Width, canvas.Height);

        canvas.Antialias(false);

        canvas.PushState();
        Camera.ApplyTo(canvas);
        World.Render(canvas);
        canvas.PopState();
    }

    public static void ReloadLevel(string path)
    {
        nextWorld = new();
        var reader = new LevelReader();
        foreach (var component in reader.GetComponents(path))
        {
            nextWorld.Add(component);
        }
        CurrentLevelPath = path;
    }

}