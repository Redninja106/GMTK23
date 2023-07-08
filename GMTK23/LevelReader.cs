using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23;
internal class LevelReader
{
    private static Dictionary<string, Func<string[], IGameComponent>> loaders = new();

    static LevelReader()
    {
        var types = Assembly.GetExecutingAssembly().GetTypes().Where(t =>
        {
            try
            {
                t.GetInterfaceMap(typeof(ISaveable));
                return true;
            }
            catch
            {
                return false;
            }
        });

        foreach (var type in types)
        {
            loaders.Add(type.Name, type.GetMethod("Load")!.CreateDelegate<Func<string[], IGameComponent>>());
            Console.WriteLine($"{type.Name} added");
        }
    }

    public IEnumerable<IGameComponent> GetComponents(string levelFile)
    {
        var lines = File.ReadAllLines(levelFile);

        foreach (var line in lines.Where(s => !string.IsNullOrEmpty(s)))
        {
            var parts = line.Split(' ').Where(s => !string.IsNullOrEmpty(s)).ToArray();

            if (loaders.TryGetValue(parts[0], out var factory))
            {
                yield return factory(parts[1..]);
            }
            else if (Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(t => t.Name == parts[0]) is not null)
            {
                var type = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(t => t.Name == parts[0]);
                yield return (IGameComponent)Activator.CreateInstance(type);
            }
            else
            {
                throw new Exception($"unknown component {parts[0]}");
            }

            //switch (parts[0].ToUpper())
            //{
            //    //case "RETICLE":
            //    //    yield return new Reticle();
            //    //    break;
            //    //case "TANK":
            //    //    yield return new Tank();
            //    //    break;
            //    //case "BOX":
            //    //    Vector2 boxPos = new(float.Parse(parts[1]), float.Parse(parts[2]));
            //    //    yield return new Box(new(boxPos, 0), .5f, .5f);
            //    //    break;
            //    //case "WALL":
            //    //    Vector2 from = new(float.Parse(parts[1]), float.Parse(parts[2]));
            //    //    Vector2 to = new(float.Parse(parts[3]), float.Parse(parts[4]));
            //    //    Vector2 diff = to - from;
            //    //    float wallWidth = diff.Length() + .5f;
            //    //    float wallHeight = .5f;
            //    //    float wallAngle = Angle.FromVector(diff);

            //    //    yield return new Wall(new(from + diff * .5f, wallAngle), wallWidth, wallHeight);
            //    //    break;
            //    //case "//":
            //    //    break;
            //    //case "LEVELBUILDER":
            //    //    yield return new LevelBuilder();
            //    //    break;
            //    //case "GAMEPLAYCONTROLLER":
            //    //    yield return new GameplayController();
            //    //    break;
            //    default:
            //        throw new Exception();
            //}
        }

        // return Enumerable.Empty<IGameComponent>();
    }
}
