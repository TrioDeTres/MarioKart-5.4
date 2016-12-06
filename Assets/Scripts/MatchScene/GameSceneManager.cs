using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;

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
    public List<PlayerManager> finishedPlayers;
    void Awake()
    {
        instance = this;
        finishedPlayers = new List<PlayerManager>();
    }

    void Start()
    {
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
    public float GetTimer()
    {
        if (player != null && player.trackCompleted)
            return player.trackCompletionTime;
        return matchTimer;
    }
    public int GetCoins()
    {
        if (player != null)
            return player.coins;
        return 0;
    }
    public void AddPlayerLap(PlayerManager p_player)
    {
        p_player.laps++;
        if (p_player.laps == 3)
        {
            p_player.hasControl = false;
            p_player.trackCompleted = true;
            p_player.trackCompletionTime = matchTimer;
            p_player.finishedPlace = finishedPlayers.Count;
            finishedPlayers.Add(p_player);
            int __activePlayers = 0;
            for (int i = 0; i < players.Count; i++)
                if (players[i] != null)
                    __activePlayers++;
            if (finishedPlayers.Count == __activePlayers)
            {
                Dictionary<int, PlayerMetadata> playersMeta = LobbyManager.singleton.playerMetadata;
                foreach (PlayerManager __player in players)
                {
                    if (__player == null)
                        continue;
                    __player.metaData.coinCount = __player.coins;
                    __player.metaData.trackTimer = __player.trackCompletionTime;
                    __player.metaData.trackTimerWithCoins = __player.trackCompletionTime - ((__player.finishedPlace + 1) * __player.coins * 0.1f);
                }
                List<float> __prog = new List<float>();
                foreach (PlayerManager __player in players)
                {
                    if (__player == null)
                        continue;
                    __prog.Add(__player.metaData.trackTimerWithCoins);
                }
                __prog.Sort();
                foreach (PlayerManager __player in players)
                {
                    if (__player == null)
                        continue;
                    __player.metaData.endPosition = __prog.IndexOf(__player.metaData.trackTimerWithCoins);
                }
                Debug.Log("Here");
                NetworkManager.singleton.ServerChangeScene("FinalScene");
            }
        }
    }
}
