using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23;
internal class GameStateManager : IGameComponent
{
    public RenderLayer RenderLayer { get; }

    private Ending? AchievedEnding = null;

    public void Render(ICanvas canvas)
    {
    }

    public void Update()
    {
    }

    public void AchieveEnding(Ending ending)
    {
        AchievedEnding = ending;
    }
}