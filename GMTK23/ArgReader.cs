using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23;
internal class ArgReader
{
    public string[] Args { get; }
    public int Position { get; set; }

    public int CountRemaining => Args.Length - Position;

    public ArgReader(string[] args)
    {
        this.Args = args;
    }

    public string Next()
    {
        return Args[Position++];
    }

    public float NextFloat()
    {
        return float.Parse(Next());
    }

    public int NextInt()
    {
        return int.Parse(Next());
    }

    public Transform NextTransform()
    {
        return Transform.Parse(Next());
    }
}
