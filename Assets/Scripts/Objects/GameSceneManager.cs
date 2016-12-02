using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameSceneManager : NetworkBehaviour
{
    public BonusManager bonusManager;
    public ShellManager shellManager;
    public CoinManager  coinManager;
    public CoinHUD      coinHUD;

    void Start()
    {
        bonusManager.OnShellGot += shellManager.CreateShells;
        coinManager.OnCoinHit += CoinHit;
        if (isServer)
            NetworkServer.SpawnObjects();
    }

    private void CoinHit()
    {
        coinHUD.IncreaseCoins();
    }
}
