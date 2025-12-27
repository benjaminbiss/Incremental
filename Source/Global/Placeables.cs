using Godot;
using System.Collections.Generic;

public partial class Placeables : Node
{
    public enum E_PlatformTypes
    {
        NONE,
        OneXOne,
        OneXTwo,
        OneXThree,
        OneXFour,
        OneXFive,
        TwoXTwo,
        TwoXThree,
        ThreeXThree,
        Plus,
        Tee,
        Z,
        Z_Invert,
        L,
        L_Invert,
        END
    }

    public static Dictionary<E_PlatformTypes, Rect2> platformAtlasRegions = new Dictionary<E_PlatformTypes, Rect2>
    {
        {E_PlatformTypes.NONE, new Rect2(0, 0, 16, 16)},
        {E_PlatformTypes.OneXOne, new Rect2(16, 32, 16, 16)},
        {E_PlatformTypes.OneXTwo, new Rect2(16, 64, 32, 16)},
        {E_PlatformTypes.OneXThree, new Rect2(16, 96, 48, 16)},
        {E_PlatformTypes.OneXFour, new Rect2(16, 128, 64, 16)},
        {E_PlatformTypes.OneXFive, new Rect2(16, 160, 80, 16)},
        {E_PlatformTypes.TwoXTwo, new Rect2(112, 128, 32, 32)},
        {E_PlatformTypes.TwoXThree, new Rect2(112, 16, 48, 32)},
        {E_PlatformTypes.ThreeXThree, new Rect2(80, 64, 48, 48)},
        {E_PlatformTypes.Plus, new Rect2(144, 64, 48, 48)},
        {E_PlatformTypes.Tee, new Rect2(48, 16, 48, 32)},
        {E_PlatformTypes.Z, new Rect2(176, 16, 48, 32)},
        {E_PlatformTypes.Z_Invert, new Rect2(240, 16, 48, 32)},
        {E_PlatformTypes.L, new Rect2(208, 64, 32, 48)},
        {E_PlatformTypes.L_Invert, new Rect2(256, 64, 32, 48)},
    };

    public enum E_TowerTypes
    {
        NONE,
        MG,
        Sentry,
        END
    }

    public static Dictionary<E_TowerTypes, Rect2> towerAtlasRegions = new Dictionary<E_TowerTypes, Rect2>
    {
        {E_TowerTypes.NONE, new Rect2(0, 0, 32, 32)},
        {E_TowerTypes.MG, new Rect2(32, 0, 32, 32)},
        {E_TowerTypes.Sentry, new Rect2(64, 0, 32, 32)},
    };
}

