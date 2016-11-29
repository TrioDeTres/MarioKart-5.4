using System.Collections;
using System.Collections.Generic;
using Prototype.NetworkLobby;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CharacterSelectManager : NetworkBehaviour
{
    public static CharacterSelectManager instance;

    [SyncVar]
    public int                          numberOfTotalPlayers;

    [SyncVar(hook = "UpdateServerMessage")]
    public int                          matchCountdown;

    public float                        prematchCountdown;

    public List<PlayerCharacterSelect>  players;
    public List<YoshiPortrait>          portraits;
    public List<Image>                  playerStatusImage;
    public List<Transform>              playerSpots;
    public List<Text>                   playerStatusLabel;
    public RectTransform                youLabel;
    public GameObject                   serverMessageLabel;
    public Text                         serverMessageText;
    public PlayerCharacterSelect        localPlayer;
    public Canvas                       canvas;
    public int                          numberOfPlayers;

    private bool isChangingScene;

    public void Start()
    {
        instance = this;
        players = new List<PlayerCharacterSelect>();

        PlayerCharacterSelectPoolUtil pool = PlayerCharacterSelectPoolUtil.Instance;

        pool.OnPlayerRegistered += AddPlayer;
        pool.OnLocalPlayerRegistered += AddLocal;

        players.AddRange(pool.GetPlayers());
        localPlayer = pool.FindLocalPlayer(players);

        numberOfPlayers = players.Count;

        if (isServer)
        {
            prematchCountdown = 5.0f;
            numberOfTotalPlayers = NetworkServer.connections.Count;
            UpdatePlayerId(players);
        }
    }

    [Server]
    private void UpdatePlayerId(List<PlayerCharacterSelect> players)
    {
        for (int i = 0; i < players.Count; i++)
        {
            PlayerCharacterSelect player = players[i];

            player.id = FindPlayerIdByStartPosition(player.transform.position);
        }
    }

    [Server]
    private int FindPlayerIdByStartPosition(Vector3 playerPosition)
    {
        float lastDistance = float.MaxValue;

        int id = 0;

        for (int i = 0; i < playerSpots.Count; i++)
        {
            float currentDistance = Vector3.Distance(playerPosition, playerSpots[i].position);

            if (lastDistance > currentDistance)
            {
                id = i;
                lastDistance = currentDistance;
            }
        }

        return id;
    }

    public void Update()
    {
        if (numberOfPlayers == numberOfTotalPlayers)
        {
            UpdateUI();

            if (Input.GetKeyDown(KeyCode.Return) && localPlayer != null)
            {
                LockPlayerDecision(true);
            }
        }

        // all players ready then change scene
        if (isServer && IsEveryPlayerReady(players) && !isChangingScene)
        {
            SavePlayerMetadata(players);
            StartCoroutine(ServerCountdownCoroutine());
        }
    }

    public void SavePlayerMetadata(List<PlayerCharacterSelect> players)
    {
        for (int i = 0; i < players.Count; i++)
        {
            PlayerCharacterSelect player = players[i];
            int connectionId = player.connectionToClient.connectionId;
            PlayerMetadata meta = new PlayerMetadata(connectionId, player.playername, player.selectedSkin);
            LobbyManager.singleton.playerMetadata.Add(connectionId, meta);
        }
    }

    private bool IsEveryPlayerReady(List<PlayerCharacterSelect> players)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].status == (int) PlayerCharacterSelect.PlayerStatus.CHOOSING)
            {
                return false;
            }
        }

        return true;
    }

    public void UpdateServerMessage(int countdown)
    {
        if (!serverMessageLabel.activeSelf)
        {
            serverMessageLabel.SetActive(true);
        }

        serverMessageText.text = "Match will start in " + countdown;
    }

    private IEnumerator ServerCountdownCoroutine()
    {
        isChangingScene = true;

        float remainingTime = prematchCountdown;
        int floorTime = Mathf.FloorToInt(remainingTime);

        while (remainingTime > 0)
        {
            yield return null;

            remainingTime -= Time.deltaTime;
            int newFloorTime = Mathf.FloorToInt(remainingTime);

            if (newFloorTime != floorTime && newFloorTime >= 0)
            {
                matchCountdown = newFloorTime;
            }
        }

        DestroyAllPlayers(players);
        NetworkManager.singleton.ServerChangeScene("Match");
    }

    public void DestroyAllPlayers(List<PlayerCharacterSelect> players)
    {
        for (int i = 0; i < players.Count; i++)
        {
            NetworkServer.Destroy(players[i].gameObject);
        }
    }

    public
        void AddPlayer(PlayerCharacterSelect player)
    {
        players.Add(player);
        numberOfPlayers = players.Count;
    }

    public void AddLocal(PlayerCharacterSelect player)
    {
        localPlayer = player;
    }

    //Change player locked status and update the ui
    public void LockPlayerDecision(bool p_lock)
    {
        if (p_lock)
        {
            localPlayer.CmdUpdatePlayerStatus((int) PlayerCharacterSelect.PlayerStatus.LOCKED);
        }
        else
        {
            localPlayer.CmdUpdatePlayerStatus((int) PlayerCharacterSelect.PlayerStatus.CHOOSING);
        }
    }

    //Update All UI
    private void UpdateUI()
    {
        UpdatePlayerStatus();
        UpdatePortrait();
        UpdateYouLabel();
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
            {
                portraits[(int) players[i].selectedSkin].EnablePlayerLockedBar(players[i].playername);
            }
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
                if (player.status == (int) PlayerCharacterSelect.PlayerStatus.CHOOSING)
                {
                    playerStatusImage[player.id].color = new Color(0.55f, 1f, 1f);
                    playerStatusLabel[player.id].text = "Choosing...";
                }
                // Ready: Light Green
                else
                {
                    playerStatusImage[player.id].color = new Color(0.25f, 1f, 0.55f);
                    playerStatusLabel[player.id].text = "Ready!";
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

