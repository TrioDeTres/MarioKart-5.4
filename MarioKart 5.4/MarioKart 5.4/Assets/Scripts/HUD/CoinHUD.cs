using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinHUD : MonoBehaviour
{
    public static int coins = 0;
    public Text text;
    
    public void IncreaseCoins ()
    {
        coins++;
        text.text = "X  " + coins;
    }
}
