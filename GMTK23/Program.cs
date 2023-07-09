using Silk.NET.Input.Glfw;
using Silk.NET.Windowing.Glfw;
using SimulationFramework;
using SimulationFramework.AudioExtensions;
using SimulationFramework.Desktop;
using SimulationFramework.Drawing;
using System.Diagnostics;
using System.Numerics;
using GMTK23;
using GMTK23.Particles;
using SimulationFramework.AudioExtensions.Windows;

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

    public override void OnInitialize()
    {
        Application.RegisterComponent<WindowsAudioProvider>(new());

        Camera = new(15);
        Inspector = new();
        SceneViewer = new();

        ReloadLevel("./Levels/cave.lvl");
        
        // Window.Title = "TANKS!";
        Time.MaxDeltaTime = 1 / 30f;
    }

    public override void OnRender(ICanvas canvas)
    {
        if (nextWorld is not null)
            World = nextWorld;

        if (Keyboard.IsKeyPressed(Key.F11))
        {
            if (Window.IsFullscreen)
            {
                Window.ExitFullscreen();
            }
            else
            {
                Window.EnterFullscreen();
            }
        }

        // float vol = Audio.Volume;
        // ImGui.DragFloat("volume", ref vol, 0.01f, 0, 1);
        // Audio.Volume = vol;

        canvas.ResetState();
        canvas.Clear(Color.FromHSV(0,0,.1f));

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

        canvas.DrawText(Performance.Framerate.ToString("f0"), new(Camera.DisplayWidth, 0), Alignment.TopRight);
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