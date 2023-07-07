using SimulationFramework;
using SimulationFramework.AudioExtensions.Windows;
using SimulationFramework.Drawing;
using System.Numerics;

namespace GMTK23;

class GMTKGame : Simulation
{
    public static GMTKGame Instance { get; private set; } = null!;

    public GameScene ActiveScene { get; private set; }
    private GameScene? nextScene;

    public GMTKGame(GameScene gameScene)
    {
        if (Instance is not null)
        {
            throw new Exception("Game already started!");
        }

        Instance = this;
        this.ActiveScene = gameScene;
    }

    public static void SwitchScenes(GameScene gameScene)
    {
        Instance.nextScene = gameScene;
    }

    public override void OnInitialize()
    {
        Application.RegisterComponent(new WindowsAudioProvider());
    }

    public override void OnRender(ICanvas canvas)
    {
        if (nextScene is not null)
        {
            ActiveScene = nextScene;
        }

        canvas.ResetState();
        canvas.Clear(Color.FromHSV(0, 0, .1f));

        ActiveScene.Update();
        ActiveScene.Render(canvas);
    }
}