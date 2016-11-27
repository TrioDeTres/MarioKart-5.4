using Prototype.NetworkLobby;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerDataHook : LobbyHook
{
    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        PlayerManager playerManager = gamePlayer.GetComponent<PlayerManager>();
        LobbyPlayer lp = lobbyPlayer.GetComponent<LobbyPlayer>();

        if (lp != null && playerManager != null) { 
            playerManager.playername = lp.playerName;
        }
    }
}
