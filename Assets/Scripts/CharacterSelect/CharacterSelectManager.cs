using System.Collections.Generic;
using System.Linq;
using Prototype.NetworkLobby;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectManager : NetworkBehaviour
{
    public List<PlayerCharacterSelect>  players;
    public List<YoshiPortrait>          portraits;
    public List<Image>                  playerStatusImage;
    public List<Transform>              playerSpots;
    public List<Text>                   playerStatusLabel;
    public RectTransform                youLabel;
    public PlayerCharacterSelect        localPlayer;
    public Canvas                       canvas;

    private bool changingScene;

    public int playerStatusImageIndex;

    public void FindPlayers()
    {
        players = FindObjectsOfType<PlayerCharacterSelect>().ToList();

        for (int i = 0; i < players.Count; i++)
        {
            PlayerCharacterSelect player = players[i];

            player.SetID(FindPlayerStartSpot(player.transform));

            if (player.isLocal)
            {
                localPlayer = player;
            }
        }
    }

    private int FindPlayerStartSpot(Transform playerTransform)
    {
        int spotId = 0;

        float lastDistance = 999.999f;

        for (int i = 0; i < playerSpots.Count; i++)
        {
            float distance = Vector3.Distance(playerTransform.position, playerSpots[i].position);

            if (lastDistance > distance)
            {
                spotId = i;
                lastDistance = distance;
            }
        }

        return spotId;
    }
   
    public void Update()
    {
        if (players.Count < 2)
        {
            FindPlayers();
        }

        UpdateUI();

        if (Input.GetKeyDown(KeyCode.Return))
        {
            LockPlayerDecision(true);
        }

        if (isServer && !changingScene) { 
            ChangeSceneWhenReady();
        }
    }

    [Server]
    public void ChangeSceneWhenReady()
    {
        bool changeScene = players.TrueForAll(p => p.status == PlayerCharacterSelect.PlayerStatus.LOCKED);

        if (changeScene)
        {
            SceneManager.LoadScene("Match");
            NetworkManager.singleton.ServerChangeScene("Match");
            changingScene = true;
        }
    }
    
    //Change player locked status and update the ui
    public void LockPlayerDecision(bool p_lock)
    {
        if (p_lock)
        {
            localPlayer.CmdUpdatePlayerStatus((int)PlayerCharacterSelect.PlayerStatus.LOCKED);
            localPlayer.CmdUpdatePlayerName();
        }
            
        else
            localPlayer.CmdUpdatePlayerStatus((int)PlayerCharacterSelect.PlayerStatus.CHOOSING);

        UpdateUI();
    }

    //Update All UI
    private void UpdateUI()
    {
        if (localPlayer != null) { 
            UpdatePlayerStatus();
            UpdatePortrait();
            UpdateYouLabel();
        }
    }

    //Change the position of the You Label
    private void UpdateYouLabel()
    {
        youLabel.anchoredPosition = playerStatusImage[localPlayer.id].rectTransform.anchoredPosition + Vector2.up * 32.0f;
    }

    private void UpdatePortrait()
    {
        //Convert the active player skin to and ID
        int __skinID = (int) localPlayer.selectedSkin;

        for (int i = 0; i < portraits.Count; i++)
        {
            //Enable the white/red animation on the portrait of the selected skin
            portraits[i].SetAnimatorSelected(__skinID == i ? true : false);
            //Disable all portraits Locked Bar
            portraits[i].DisablePlayerLockedBar();
        }
        for (int i = 0; i < players.Count; i++)
        {
            //Enable Locked Bar for the skins selected by locked players
            if (players[i].status == PlayerCharacterSelect.PlayerStatus.LOCKED)
                portraits[(int)players[i].selectedSkin].EnablePlayerLockedBar(players[i].playerManager.playername);
        }
    }

    private void UpdatePlayerStatus()
    {
        for(int i = 0; i < 4; i ++)
        {
            if (i < players.Count)
            {
                playerStatusImage[i].gameObject.SetActive(true);
                playerStatusImage[i].gameObject.SetActive(true);

                PlayerCharacterSelect player = players[i];

                // Choosing: Light Blue
                if (player.status == PlayerCharacterSelect.PlayerStatus.CHOOSING)
                {
                    playerStatusImage[player.id].color = new Color(0.55f, 1f, 1f);
                    playerStatusLabel[player.id].text = "CHOOSING...";
                }
                // Ready: Light Green
                else
                {
                    playerStatusImage[player.id].color = new Color(0.25f, 1f, 0.55f);
                    playerStatusLabel[player.id].text = "READY!";
                }
            }
            //Disable if ID higher than player count
            else
            {
                playerStatusImage[i].gameObject.SetActive(false);
                playerStatusLabel[i].gameObject.SetActive(false);
            }
        }
    }

    public void SkinSelected(int p_skinIndex)
    {
        //Check if it's not already selected
        if ((int)localPlayer.selectedSkin == p_skinIndex)
            return;
        //Check if any player already selected skin
        for(int i = 0; i < players.Count; i ++)
        {
            if (players[i].status == PlayerCharacterSelect.PlayerStatus.LOCKED && (int)players[i].selectedSkin == p_skinIndex)
                //play error sound
                return;
        }
        //play select sound

        //Unlock player select and update skin and UI
        LockPlayerDecision(false);
        localPlayer.CmdUpdatePlayerSkin(p_skinIndex);
        UpdateUI();
    }
}

