﻿using ImGuiNET;
using SimulationFramework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23.Engine.Editor;
public abstract class DebugWindow
{
    public static readonly List<DebugWindow> Windows = new();

    private bool isOpen;

    public bool IsOpen
    {
        get => isOpen;
        protected set => isOpen = value;
    }

    public virtual Key? KeyBind => null;
    public virtual ImGuiWindowFlags WindowFlags => ImGuiWindowFlags.None;
    public virtual string Title => GetType().Name;

    protected abstract void OnLayout();

    static DebugWindow()
    {

    }



    public static void LayoutAll()
    {
        foreach (var debugWindow in Windows)
        {
            debugWindow.Layout();
        }
    }

    public DebugWindow()
    {

    }

    public void Show()
    {
        IsOpen = true;
    }

    public void Hide()
    {
        IsOpen = false;
    }

    public void Layout()
    {
        if (KeyBind is not null && Keyboard.IsKeyPressed(KeyBind.Value))
        {
            IsOpen = !IsOpen;
        }

        ImGui.SetNextWindowSize(new(400, 300), ImGuiCond.FirstUseEver);
        if (isOpen && ImGui.Begin(Title, ref isOpen, WindowFlags))
        {
            OnLayout();
        }

        ImGui.End();
    }

    public static T RegisterWindow<T>(T debugWindow) where T : DebugWindow
    {
        Windows.Add(debugWindow);
        return debugWindow;
    }
}
