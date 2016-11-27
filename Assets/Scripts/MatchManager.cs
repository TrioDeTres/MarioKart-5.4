using UnityEngine;
using UnityEngine.Networking;

public class MatchManager : NetworkBehaviour
{
    public Transform[] spawnPoints;
    public PlayerManager[] players;

    public void Start()
    {
        if (isServer)
        { 
            players = FindPlayers();
            ResetPlayerComponents(players);
            UpdatePlayerPosition(players, spawnPoints);
        }
    }

    private PlayerManager[] FindPlayers()
    {
        return FindObjectsOfType<PlayerManager>();
    }

    private void ResetPlayerComponents(PlayerManager[] players)
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].RpcResetPlayerComponents();
        }
    }

    private void UpdatePlayerPosition(PlayerManager[] players, Transform[] spawnPoints)
    {
        for (int i = 0; i < players.Length; i++)
        {
            PlayerManager player = players[i];
            player.RpcUpdatePlayerPosition(spawnPoints[player.id].transform.position);
        }
    }
}
