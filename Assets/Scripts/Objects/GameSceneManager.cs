using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameSceneManager : NetworkBehaviour
{
    public static GameSceneManager instance;
    public BonusManager bonusManager;
    public ShellManager shellManager;
    public CoinManager  coinManager;
    public CoinHUD      coinHUD;
    public PlayerManager player;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        coinManager.OnCoinHit += CoinHit;
        if (isServer)
            NetworkServer.SpawnObjects();
    }

    private void CoinHit()
    {
        coinHUD.IncreaseCoins();
    }
    
}
