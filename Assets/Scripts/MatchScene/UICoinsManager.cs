using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UICoinsManager : MonoBehaviour
{
    public Text coinsLabel;
    public int currentCoins = -1;

	// Update is called once per frame
	void Update ()
    {
        int __coins = GameSceneManager.instance.GetCoins();
        if (__coins == currentCoins)
            return;
        coinsLabel.text = "X " + __coins.ToString();
        currentCoins = __coins;
	}
}
