using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    public BonusManager bonusManager;
    public ShellManager shellManager;
    public CoinManager  coinManager;
    public CoinHUD      coinHUD;

    void Start()
    {
        bonusManager.OnShellGot += shellManager.CreateShells;
        coinManager.OnCoinHit += CoinHit;
    }

    private void CoinHit()
    {
        coinHUD.IncreaseCoins();
    }
}
