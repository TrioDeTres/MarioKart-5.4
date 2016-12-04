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
        /*if (Input.GetKeyDown(KeyCode.A))
        { 
            foreach (PlayerManager __p in FindObjectsOfType<PlayerManager>())
            {
                Debug.Log(__p.skin);
                Debug.Log(__p.playerName);
            }
            Debug.Log("----");
            foreach (PlayerManager __p in players)
            {
                Debug.Log(__p.skin);
                Debug.Log(__p.playerName);
            }
        }
        if (isServer)
        {
            foreach (PlayerManager _p in players)
            {
                _p.laps = Random.Range(0, 3);
                _p.position = Random.Range(0, 4);
            }
        }*/
    }
    private void CoinHit()
    {
        coinHUD.IncreaseCoins();
    }
    
}
