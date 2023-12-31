﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23.NewGame;
internal class NewGameManager : IGameComponent
{
    public RenderLayer RenderLayer { get; }
    private float elapsedTime;
    private float fadeOutTime = -1;

    public void Render(ICanvas canvas)
    {
        canvas.Clear(Color.Black);
        canvas.Translate(Program.Camera.Transform.Position);

        canvas.FontStyle(.5f, FontStyle.Normal);
        canvas.DrawText("The player is trying to make a torch", 0, -.1f, Alignment.BottomCenter);
        canvas.DrawText("what happens next is up to you", 0, .1f, Alignment.TopCenter);

        canvas.Fill(new Color(0, 0, 0, 1f - MathF.Min(1, elapsedTime)));
        canvas.DrawRect(Vector2.Zero, new(Program.Camera.HorizontalSize, Program.Camera.VerticalSize), Alignment.Center);

        if (fadeOutTime > 0)
        {
            canvas.Fill(new Color(0, 0, 0, MathF.Min(1, fadeOutTime)));
            canvas.DrawRect(Vector2.Zero, new(Program.Camera.HorizontalSize, Program.Camera.VerticalSize), Alignment.Center);
            if (fadeOutTime > 1)
            {
                Program.ReloadLevel("./Levels/cave.lvl");
                Program.NoIntro = true;
            }
        }
    }

    public void Update()
    {
        if (Program.NoIntro)
        {
            Program.ReloadLevel("./Levels/cave.lvl");
        }

        elapsedTime += Time.DeltaTime;

        if (fadeOutTime != -1)
        {
            fadeOutTime += Time.DeltaTime;
        }
        else 
        {
            if (elapsedTime > 4 )
            {
                fadeOutTime = 0;
            }

            if (Mouse.IsButtonPressed(MouseButton.Left))
            {
                fadeOutTime = 0;
            }
        }

    }
}
