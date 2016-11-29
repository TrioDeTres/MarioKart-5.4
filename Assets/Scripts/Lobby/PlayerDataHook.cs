using Prototype.NetworkLobby;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerDataHook : LobbyHook
{
    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        PlayerCharacterSelect playerCharacterSelect = gamePlayer.GetComponent<PlayerCharacterSelect>();
        LobbyPlayer lp = lobbyPlayer.GetComponent<LobbyPlayer>();

        playerCharacterSelect.playername = lp.playerName;
    }
}
