using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;
using UnityEngine.Networking.Match;
using System.Collections;
using System.Linq;


namespace Prototype.NetworkLobby
{
    public class LobbyManager : NetworkLobbyManager 
    {
        static short MsgKicked = MsgType.Highest + 1;

        static public LobbyManager s_Singleton;


        [Header("Unity UI Lobby")]
        [Tooltip("Time in second between all players ready & match start")]
        public float prematchCountdown = 5.0f;

        [Space]
        [Header("UI Reference")]
        public LobbyTopPanel topPanel;

        public RectTransform mainMenuPanel;
        public RectTransform lobbyPanel;

        public LobbyInfoPanel infoPanel;
        public LobbyCountdownPanel countdownPanel;
        public GameObject addPlayerButton;

        protected RectTransform currentPanel;

        public Button backButton;

        public Text statusInfo;
        public Text hostInfo;

        //Client numPlayers from NetworkManager is always 0, so we count (throught connect/destroy in LobbyPlayer) the number
        //of players, so that even client know how many player there is.
        [HideInInspector]
        public int _playerNumber = 0;

        //used to disconnect a client properly when exiting the matchmaker
        [HideInInspector]
        public bool _isMatchmaking = false;

        protected bool _disconnectServer = false;
        
        protected ulong _currentMatchID;

        protected LobbyHook _lobbyHooks;

        public InputField messageInput;

        void Start()
        {
            s_Singleton = this;
            _lobbyHooks = GetComponent<LobbyHook>();
            currentPanel = mainMenuPanel;

            backButton.gameObject.SetActive(false);
            GetComponent<Canvas>().enabled = true;

            DontDestroyOnLoad(gameObject);

            SetServerInfo("Offline", "None");
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
                CmdMessage();
        }

        public override void OnLobbyClientSceneChanged(NetworkConnection conn)
        {
            if (SceneManager.GetSceneAt(0).name == lobbyScene)
            {
                if (topPanel.isInGame)
                {
                    ChangeTo(lobbyPanel);
                    if (_isMatchmaking)
                    {
                        if (conn.playerControllers[0].unetView.isServer)
                        {
                            backDelegate = StopHostClbk;
                        }
                        else
                        {
                            backDelegate = StopClientClbk;
                        }
                    }
                    else
                    {
                        if (conn.playerControllers[0].unetView.isClient)
                        {
                            backDelegate = StopHostClbk;
                        }
                        else
                        {
                            backDelegate = StopClientClbk;
                        }
                    }
                }
                else
                {
                    ChangeTo(mainMenuPanel);
                }

                topPanel.ToggleVisibility(true);
                topPanel.isInGame = false;
            }
            else
            {
                ChangeTo(null);

                Destroy(GameObject.Find("MainMenuUI(Clone)"));

                //backDelegate = StopGameClbk;
                topPanel.isInGame = true;
                topPanel.ToggleVisibility(false);
            }
        }

        public void ChangeTo(RectTransform newPanel)
        {
            if (currentPanel != null)
            {
                currentPanel.gameObject.SetActive(false);
            }

            if (newPanel != null)
            {
                newPanel.gameObject.SetActive(true);
            }

            currentPanel = newPanel;

            if (currentPanel != mainMenuPanel)
            {
                backButton.gameObject.SetActive(true);
            }
            else
            {
                backButton.gameObject.SetActive(false);
                SetServerInfo("Offline", "None");
                _isMatchmaking = false;
            }
        }

        public void DisplayIsConnecting()
        {
            var _this = this;
            infoPanel.Display("Connecting...", "Cancel", () => { _this.backDelegate(); });
        }

        public void SetServerInfo(string status, string host)
        {
            statusInfo.text = status;
            hostInfo.text = host;
        }


        public delegate void BackButtonDelegate();
        public BackButtonDelegate backDelegate;
        public void GoBackButton()
        {
            backDelegate();
			topPanel.isInGame = false;
        }

        // ----------------- Server management

        public void AddLocalPlayer()
        {
            TryToAddPlayer();
        }

        public void RemovePlayer(LobbyPlayer player)
        {
            player.RemovePlayer();
        }

        public void SimpleBackClbk()
        {
            ChangeTo(mainMenuPanel);
        }
                 
        public void StopHostClbk()
        {
            if (_isMatchmaking)
            {
				matchMaker.DestroyMatch((NetworkID)_currentMatchID, 0, OnDestroyMatch);
				_disconnectServer = true;
            }
            else
            {
                StopHost();
            }

            ClientScene.localPlayers.Clear();
            
            ChangeTo(mainMenuPanel);
        }

        public void StopClientClbk()
        {
            StopClient();

            if (_isMatchmaking)
            {
                StopMatchMaker();
            }

            ChangeTo(mainMenuPanel);
        }

        public void StopServerClbk()
        {
            StopServer();
            ChangeTo(mainMenuPanel);
        }

        public override void OnStartHost()
        {
            base.OnStartHost();

            ChangeTo(lobbyPanel);
            backDelegate = StopHostClbk;
            SetServerInfo("Hosting", networkAddress);
        }

		public override void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
		{
			base.OnMatchCreate(success, extendedInfo, matchInfo);
            _currentMatchID = (System.UInt64)matchInfo.networkId;
		}

		public override void OnDestroyMatch(bool success, string extendedInfo)
		{
			base.OnDestroyMatch(success, extendedInfo);
			if (_disconnectServer)
            {
                StopMatchMaker();
                StopHost();
            }
        }

        public void OnPlayersNumberModified(int count)
        {
            _playerNumber += count;
            int localPlayerCount = 0;
            foreach (PlayerController p in ClientScene.localPlayers)
                localPlayerCount += (p == null || p.playerControllerId == -1) ? 0 : 1;

            addPlayerButton.SetActive(localPlayerCount < maxPlayersPerConnection && _playerNumber < maxPlayers);
        }

        public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId)
        {
            GameObject obj = Instantiate(lobbyPlayerPrefab.gameObject) as GameObject;

            LobbyPlayer newPlayer = obj.GetComponent<LobbyPlayer>();
            newPlayer.ToggleJoinButton(numPlayers + 1 >= minPlayers);

            for (int i = 0; i < lobbySlots.Length; ++i)
            {
                LobbyPlayer p = lobbySlots[i] as LobbyPlayer;

                if (p != null)
                {
                    p.ToggleJoinButton(numPlayers + 1 >= minPlayers);
                }
            }

            return obj;
        }

        public override void OnLobbyServerDisconnect(NetworkConnection conn)
        {
            for (int i = 0; i < lobbySlots.Length; ++i)
            {
                LobbyPlayer p = lobbySlots[i] as LobbyPlayer;

                if (p != null)
                {
                    p.ToggleJoinButton(numPlayers >= minPlayers);
                }
            }

        }

        public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
        {
            if (_lobbyHooks)
                _lobbyHooks.OnLobbyServerSceneLoadedForPlayer(this, lobbyPlayer, gamePlayer);

            return true;
        }

        public override void OnLobbyServerPlayersReady()
        {
			bool allready = true;
			for(int i = 0; i < lobbySlots.Length; ++i)
			{
				if(lobbySlots[i] != null)
					allready &= lobbySlots[i].readyToBegin;
			}

			if(allready)
				StartCoroutine(ServerCountdownCoroutine());
        }

        public IEnumerator ServerCountdownCoroutine()
        {
            float remainingTime = prematchCountdown;
            int floorTime = Mathf.FloorToInt(remainingTime);

            while (remainingTime > 0)
            {
                yield return null;

                remainingTime -= Time.deltaTime;
                int newFloorTime = Mathf.FloorToInt(remainingTime);

                if (newFloorTime != floorTime)
                {
                    floorTime = newFloorTime;

                    for (int i = 0; i < lobbySlots.Length; ++i)
                    {
                        if (lobbySlots[i] != null)
                        {
                            (lobbySlots[i] as LobbyPlayer).RpcUpdateCountdown(floorTime);
                        }
                    }
                }
            }

            for (int i = 0; i < lobbySlots.Length; ++i)
            {
                if (lobbySlots[i] != null)
                {
                    (lobbySlots[i] as LobbyPlayer).RpcUpdateCountdown(0);
                }
            }

            ServerChangeScene(playScene);
        }

        public override void OnClientConnect(NetworkConnection conn)
        {
            base.OnClientConnect(conn);

            infoPanel.gameObject.SetActive(false);

            if (!NetworkServer.active)
            {
                ChangeTo(lobbyPanel);
                backDelegate = StopClientClbk;
                SetServerInfo("Client", networkAddress);
            }
        }


        public override void OnClientDisconnect(NetworkConnection conn)
        {
            base.OnClientDisconnect(conn);
            ChangeTo(mainMenuPanel);
        }

        public override void OnServerDisconnect(NetworkConnection conn)
        {
            base.OnServerDisconnect(conn);

            for (int i = 0; i < lobbySlots.Length; i++)
            {
                var player = lobbySlots[i];
                if (player == null)
                    continue;

                if (player.connectionToClient == conn)
                {
                    lobbySlots[i] = null;
                    NetworkServer.Destroy(player.gameObject);
                    LobbyPlayerList._instance.RemovePlayer(player as LobbyPlayer);
                }
            }
        }

        public override void OnClientError(NetworkConnection conn, int errorCode)
        {
            ChangeTo(mainMenuPanel);
            infoPanel.Display("Server is offline.", "Close", null);
        }

        public void CmdMessage()
        {
            if (string.IsNullOrEmpty(messageInput.text)) return;
            LobbyPlayerList._instance.localPlayer.CmdMessage(messageInput.text);
        }
    }
}
