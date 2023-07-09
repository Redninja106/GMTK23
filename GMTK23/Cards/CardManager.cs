using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23.Cards;
internal class CardManager : IGameComponent
{
    public RenderLayer RenderLayer => 0;

    public void Render(ICanvas canvas)
    {
    }

    public void Update()
    {
        var hand = Program.World.Find<PlayerHand>();
    }
}
