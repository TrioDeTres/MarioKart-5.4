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

