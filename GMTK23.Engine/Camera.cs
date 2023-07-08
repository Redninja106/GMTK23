﻿using SimulationFramework.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GMTK23.Engine;

public class Camera : Component
{
    public int DisplayWidth { get; private set; }
    public int DisplayHeight { get; private set; }

    private float verticalSize = 1.0f;

    public static Camera Main { get; private set; }
    public static Camera Active { get; set; }

    public float AspectRatio => DisplayWidth / (float)DisplayHeight;

    public bool HorizontalAnchor { get; set; } = false;

    public float VerticalSize
    {
        get => verticalSize;
        set => verticalSize = value;
    }

    public float HorizontalSize
    {
        get => verticalSize * AspectRatio;
        set => verticalSize = value / AspectRatio;
    }

    public Camera()
    {
        Main ??= this;
    }

    public override void Initialize(Entity parent)
    {
    }

    public override void Render(ICanvas canvas)
    {
    }

    public override void Update()
    {
    }

    public void SetDisplaySize(int width, int height)
    {
        DisplayWidth = width;
        DisplayHeight = height;
    }

    public void ApplyTo(ICanvas canvas)
    {
        // world to screen space
        canvas.Transform(CreateLocalToScreenMatrix());

        // world to local space
        canvas.Transform(ParentEntity.Transform.CreateMatrix(TransformSpace.World, TransformSpace.Local));
    }

    public Vector2 ScreenToWorld(Vector2 point)
    {
        return ParentEntity.Transform.LocalToWorld(ScreenToLocal(point));
    }

    public Vector2 WorldToScreen(Vector2 point)
    {
        return LocalToScreen(ParentEntity.Transform.WorldToLocal(point));
    }

    public Vector2 LocalToScreen(Vector2 point)
    {
        return Vector2.Transform(point, CreateLocalToScreenMatrix());
    }

    public Vector2 ScreenToLocal(Vector2 point)
    {
        return Vector2.Transform(point, CreateScreenToLocalMatrix());
    }

    private Matrix3x2 CreateLocalToScreenMatrix()
    {
        return
            Matrix3x2.CreateScale(1f, -1f) *
            Matrix3x2.CreateScale(1f / GetScreenScale()) *
            Matrix3x2.CreateTranslation(DisplayWidth * .5f, DisplayHeight * .5f);
    }

    private Matrix3x2 CreateScreenToLocalMatrix()
    {
        return
            Matrix3x2.CreateTranslation(-DisplayWidth * .5f, -DisplayHeight * .5f) *
            Matrix3x2.CreateScale(GetScreenScale()) *
            Matrix3x2.CreateScale(1f, -1f);
    }

    private float GetScreenScale()
    {
        return verticalSize / DisplayHeight; /* HorizontalAnchor ? 
            HorizontalSize / DisplayWidth : 
            verticalSize / DisplayHeight;*/
    }
}
