using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameSceneManager : NetworkBehaviour
{
    public static GameSceneManager instance;
    public List<PlayerManager> players = new List<PlayerManager>();
    public BonusManager bonusManager;
    public ShellManager shellManager;
    public CoinManager  coinManager;
    public CoinHUD      coinHUD;
    public PlayerManager player;
    [SyncVar]
    public float matchTimer = 0f;
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
    void Update()
    {
        if (isClient)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                player.CmdPlayerShoot();
            }
        }
        if (isServer)
            matchTimer += Time.deltaTime;
    }
    private void CoinHit()
    {
        coinHUD.IncreaseCoins();
    }
    public float GetTimer()
    {
        if (player != null && player.trackCompleted)
            return player.trackCompletionTime;
        return matchTimer;
    }
}
