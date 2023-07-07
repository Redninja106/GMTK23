using SimulationFramework;
using SimulationFramework.AudioExtensions.Windows;
using SimulationFramework.Drawing;
using System.Numerics;

namespace GMTK23
{
    class GMTKGame : Simulation
    {
        public static GMTKGame Instance { get; } = new();

        public override void OnInitialize()
        {
            Application.RegisterComponent(new WindowsAudioProvider());
        }

        public override void OnRender(ICanvas canvas)
        {
            canvas.ResetState();
            canvas.Clear(Color.FromHSV(0, 0, .1f));

            canvas.Translate(canvas.Width / 2f, canvas.Height / 2f);
            canvas.DrawText("Hello world!", Vector2.Zero, Alignment.Center);
        }
    }
}