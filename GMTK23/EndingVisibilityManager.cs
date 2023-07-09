using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23;
internal class EndingVisibilityManager : IGameComponent
{
    private Dictionary<Ending, string> userEndings = new();

    public EndingVisibilityManager()
    {
        if (File.Exists("./endings.txt"))
        {
            var lines = File.ReadAllLines("./endings.txt");

            for (int i = 1; i < lines.Length; i += 2)
            {
                var endingTitle = lines[i - 1];
                var username = lines[i];

                var ending = Ending.AllEndings.FirstOrDefault(e => e.Title == endingTitle);

                if (ending is null)
                    continue;

                userEndings.Add(ending, username);
            }
        }
    }

    public RenderLayer RenderLayer { get; }

    public void Render(ICanvas canvas)
    {
    }

    public void Update()
    {
    }

    public int GetAchievedCount()
    {
        return userEndings.Count;
    }

    public bool IsVisible(Ending ending)
    {
        return userEndings.ContainsKey(ending);
    }

    public string GetAchievedUsername(Ending ending)
    {
        return userEndings[ending];
    }

    public void OnEndingAchieved(Ending ending, string username)
    {
        if (userEndings.ContainsKey(ending))
            return;

        userEndings.Add(ending, username);
        Save();
    }

    private void Save()
    {
        StringBuilder builder = new();
        foreach (var (ending, username) in userEndings)
        {
            builder.AppendLine(ending.Title);
            builder.AppendLine(username);
        }
        File.WriteAllText("./endings.txt", builder.ToString());
    }
}
