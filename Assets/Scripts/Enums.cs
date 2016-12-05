using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums
{
    public static string YoshiSkinToString(YoshiSkin p_skin)
    {
        if (p_skin == YoshiSkin.GREEN)
            return "Green";
        else if (p_skin == YoshiSkin.BLACK)
            return "Black";
        else if (p_skin == YoshiSkin.DARK_BLUE)
            return "DarkBlue";
        else if (p_skin == YoshiSkin.RED)
            return "Red";
        else if (p_skin == YoshiSkin.YELLOW)
            return "Yellow";
        else if (p_skin == YoshiSkin.LIGHT_BLUE)
            return "LightBlue";
        else if (p_skin == YoshiSkin.PINK)
            return "Pink";
        else if (p_skin == YoshiSkin.ORANGE)
            return "Orange";
        else if (p_skin == YoshiSkin.WHITE)
            return "White";
        return "Green";
    }
    public static Color YoshiSkinToColor(YoshiSkin p_skin)
    {
        if (p_skin == YoshiSkin.GREEN)
            return new Color(24/255f, 139/255f, 26/255f);
        else if (p_skin == YoshiSkin.BLACK)
            return new Color(23/255f, 23/255f, 23/255f);
        else if (p_skin == YoshiSkin.DARK_BLUE)
            return new Color(30/255f, 35/255f, 114/255f);
        else if (p_skin == YoshiSkin.RED)
            return new Color(159/255f, 7/255f, 9/255f);
        else if (p_skin == YoshiSkin.YELLOW)
            return new Color(201/ 255f, 167/255f, 32/255f);
        else if (p_skin == YoshiSkin.LIGHT_BLUE)
            return new Color(15/255f, 121/255f, 184/255f);
        else if (p_skin == YoshiSkin.PINK)
            return new Color(182 / 255f, 75/ 255f, 114/255f);
        else if (p_skin == YoshiSkin.ORANGE)
            return new Color(159 / 255f, 88/ 255f, 13/255f);
        else if (p_skin == YoshiSkin.WHITE)
            return new Color(94/ 255f, 103/255f, 109/255f);
        return new Color(23 / 255f, 23 / 255f, 23 / 255f);
    }
}

public enum YoshiSkin
{
    GREEN,
    BLACK,
    DARK_BLUE,
    RED,
    YELLOW,
    LIGHT_BLUE,
    PINK,
    ORANGE,
    WHITE
}

public enum KartType
{
    STANDART,
    YOSHI_KART
}

public enum BonusState
{
    NOTHING,
    SHELL
}

public enum ComponentState
{
    CHAR_SELECT,
    MATCH
}

