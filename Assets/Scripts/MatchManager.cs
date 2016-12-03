using System.Collections.Generic;
using System.Linq;
using Prototype.NetworkLobby;
using UnityEngine;
using UnityEngine.Networking;

public class MatchManager : NetworkBehaviour
{
    public Transform[] spawnPoints;
    public GameObject playerPrefab;

    public void Start()
    {
        if (isServer)
        {
            SpawnPlayers(NetworkServer.connections.ToArray());
        }
    }

    private void SpawnPlayers(NetworkConnection[] connections)
    {
        Dictionary<int, PlayerMetadata> playersMeta = LobbyManager.singleton.playerMetadata;

        for (int i = 0; i < connections.Length; i++)
        {
            NetworkConnection connection = connections[i];

            GameObject player = Instantiate(playerPrefab, spawnPoints[i].transform.position, Quaternion.identity) as GameObject;

            PlayerManager playerManager = player.GetComponent<PlayerManager>();

            PlayerMetadata playerMeta = playersMeta[connection.connectionId];

            playerManager.playername = playerMeta.playername;
            playerManager.skin = (int) playerMeta.skin;
            NetworkServer.ReplacePlayerForConnection(connection, player, 0);
        }
    }
}
