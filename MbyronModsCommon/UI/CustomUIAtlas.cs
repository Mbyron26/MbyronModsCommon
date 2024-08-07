﻿namespace MbyronModsCommon.UI;
using ColossalFramework.UI;
using System.Collections.Generic;
using UnityEngine;

public static class CustomUIAtlas {
    private static UITextureAtlas inGameAtlas;
    private static UITextureAtlas mbyronModsAtlas;

    public static Dictionary<string, RectOffset> SpriteParams { get; private set; } = new();
    public static string CustomBackground => nameof(CustomBackground);
    public static string Rectangle => nameof(Rectangle);
    public static string TransparencySprite => nameof(TransparencySprite);
    public static string LineBottom => nameof(LineBottom);
    public static string RoundedRectangle1 => nameof(RoundedRectangle1);
    public static string RoundedRectangle2 => nameof(RoundedRectangle2);
    public static string RoundedRectangle3 => nameof(RoundedRectangle3);
    public static string RoundedRectangle4 => nameof(RoundedRectangle4);
    public static string RoundedRectangle5 => nameof(RoundedRectangle5);
    public static string Circle => nameof(Circle);
    public static string ToggleOnFg => nameof(ToggleOnFg);
    public static string ToggleOffFg => nameof(ToggleOffFg);
    public static string CheckBoxOn => nameof(CheckBoxOn);
    public static string CheckBoxOnFg => nameof(CheckBoxOnFg);
    public static string CheckBoxOffBg => nameof(CheckBoxOffBg);
    public static string GradientSlider => nameof(GradientSlider);
    public static string EmptySprite => nameof(EmptySprite);
    public static string ArrowDown => nameof(ArrowDown);
    public static string CloseButton => nameof(CloseButton);
    public static string ResetButton => nameof(ResetButton);
    public static string Copy => nameof(Copy);
    public static string Paste => nameof(Paste);
    public static string Clean => nameof(Clean);
    public static string Close => nameof(Close);
    public static string ToolButton => nameof(ToolButton);

    static CustomUIAtlas() {
        SpriteParams[CustomBackground] = new RectOffset(12, 12, 12, 12);
        SpriteParams[RoundedRectangle1] = new RectOffset(4, 4, 4, 4);
        SpriteParams[RoundedRectangle2] = new RectOffset(6, 6, 6, 6);
        SpriteParams[RoundedRectangle3] = new RectOffset(8, 8, 8, 8);
        SpriteParams[RoundedRectangle4] = new RectOffset(10, 10, 10, 10);
        SpriteParams[RoundedRectangle5] = new RectOffset(12, 12, 12, 12);
        SpriteParams[Rectangle] = new RectOffset(1, 1, 1, 1);
        SpriteParams[Circle] = new RectOffset();
        SpriteParams[ToggleOnFg] = new RectOffset();
        SpriteParams[ToggleOffFg] = new RectOffset(12, 12, 12, 12);
        SpriteParams[CheckBoxOffBg] = new RectOffset(7, 7, 7, 7);
        SpriteParams[CheckBoxOn] = new RectOffset();
        SpriteParams[CheckBoxOnFg] = new RectOffset(6, 6, 6, 6);
        SpriteParams[LineBottom] = new RectOffset(1, 1, 0, 0);
        SpriteParams[GradientSlider] = new RectOffset(8, 8, 8, 8);
        SpriteParams[EmptySprite] = new RectOffset(1, 1, 0, 0);
        SpriteParams[TransparencySprite] = new RectOffset();
        SpriteParams[ArrowDown] = new RectOffset(4, 4, 4, 4);
        SpriteParams[CloseButton] = new RectOffset(4, 4, 4, 4);
        SpriteParams[ResetButton] = new RectOffset(4, 4, 4, 4);
        SpriteParams[Copy] = new RectOffset(1, 1, 1, 1);
        SpriteParams[Paste] = new RectOffset(1, 1, 1, 1);
        SpriteParams[Clean] = new RectOffset(1, 1, 1, 1);
        SpriteParams[Close] = new RectOffset(1, 1, 1, 1);
        SpriteParams[ToolButton] = new RectOffset(1, 1, 1, 1);
    }

    public static UITextureAtlas MbyronModsAtlas {
        get {
            if (mbyronModsAtlas is null) {
                var atlas = UIUtils.GetAtlas(nameof(MbyronModsAtlas));
                if (atlas is not null) {
                    mbyronModsAtlas = atlas;
                }
                else {
                    mbyronModsAtlas = UIUtils.CreateTextureAtlas(nameof(MbyronModsAtlas), $"{AssemblyUtils.CurrentAssemblyName}.UI.Resources.", SpriteParams);
                    MbyronModsCommon.Logger.GetLogger(AssemblyUtils.CurrentAssemblyName).Info("Initialized MbyronModsAtlas");
                }
            }
            return mbyronModsAtlas;
        }
    }
    public static UITextureAtlas InGameAtlas {
        get {
            if (inGameAtlas is null) {
                inGameAtlas = UIUtils.GetAtlas("Ingame");
                inGameAtlas ??= UIUtils.GetDefaultAtlas();
            }
            return inGameAtlas;
        }
    }

}