using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23;
internal interface ISaveable
{
    IEnumerable<string> Save();
    static abstract IGameComponent Load(string[] args);
}
