using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23.Engine;

internal class DependsOnAttribute<T> : Attribute where T : Component
{
}
