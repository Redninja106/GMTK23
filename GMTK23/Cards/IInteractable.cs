﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23.Cards;
internal interface IInteractable : IGameComponent
{
    Rectangle GetBounds();
}