﻿using SimulationFramework.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23;
internal interface IGameComponent
{
    void Render(ICanvas canvas);
    void Update();
}
