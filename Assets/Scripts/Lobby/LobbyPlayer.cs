using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Prototype.NetworkLobby
{
    public class LobbyPlayer : NetworkLobbyPlayer
    {
        public InputField nameInput;
        public Button readyButton;

        public GameObject localIcone;
        public GameObject remoteIcone;

        [SyncVar(hook = "OnMyName")]
        public string playerName = "";

        public Color OddRowColor = new Color(250.0f / 255.0f, 250.0f / 255.0f, 250.0f / 255.0f, 1.0f);
        public Color EvenRowColor = new Color(180.0f / 255.0f, 180.0f / 255.0f, 180.0f / 255.0f, 1.0f);

        public Color JoinColor = new Color(0.0f, 255.0f, 149.0f, 1.0f);
        public Color NotReadyColor = new Color(34.0f / 255.0f, 44 / 255.0f, 55.0f / 255.0f, 1.0f);
        public Color ReadyColor = new Color(0.0f, 204.0f / 255.0f, 204.0f / 255.0f, 1.0f);
        public Color HighlightedColor = new Color(9.0f, 56.0f, 56.0f, 1.0f);

        public override void OnClientEnterLobby()
        {
            base.OnClientEnterLobby();

            if (LobbyManager.singleton != null) LobbyManager.singleton.OnPlayersNumberModified(1);

            LobbyPlayerList._instance.AddPlayer(this);
            LobbyPlayerList._instance.DisplayDirectServerWarning(isServer && LobbyManager.singleton.matchMaker == null);

            if (isLocalPlayer)
            {
                
                SetupLocalPlayer();
            }
            else
            {
                SetupOtherPlayer();
            }

            OnMyName(playerName);
        }

        public override void OnStartAuthority()
        {
            base.OnStartAuthority();

            readyButton.transform.GetChild(0).GetComponent<Text>().color = Color.white;

            SetupLocalPlayer();
        }

        void ChangeReadyButtonColor(Color c)
        {
            ColorBlock b = readyButton.colors;
            b.normalColor = c;
            b.pressedColor = c;
            b.highlightedColor = c;
            b.disabledColor = c;
            readyButton.colors = b;
        }

        void SetupOtherPlayer()
        {
            nameInput.interactable = false;

            ChangeReadyButtonColor(NotReadyColor);

            readyButton.transform.GetChild(0).GetComponent<Text>().text = "...";
            readyButton.interactable = false;

            OnClientReady(false);
        }

        void SetupLocalPlayer()
        {
            LobbyPlayerList._instance.localPlayer = this;

            nameInput.interactable = true;
            remoteIcone.gameObject.SetActive(false);
            localIcone.gameObject.SetActive(true);

            ChangeReadyButtonColor(JoinColor);

            readyButton.transform.GetChild(0).GetComponent<Text>().text = "READY";
            readyButton.interactable = true;

            if (playerName == "")
                CmdNameChanged("Player " + (LobbyPlayerList._instance.playerListContentTransform.childCount-1));

            nameInput.interactable = true;

            nameInput.onEndEdit.RemoveAllListeners();
            nameInput.onEndEdit.AddListener(OnNameChanged);


            readyButton.onClick.RemoveAllListeners();
            readyButton.onClick.AddListener(OnReadyClicked);

            if (LobbyManager.singleton != null) LobbyManager.singleton.OnPlayersNumberModified(0);
        }

        public override void OnClientReady(bool readyState)
        {
            if (readyState)
            {
                Text textComponent = readyButton.transform.GetChild(0).GetComponent<Text>();
                textComponent.text = "READY";
                textComponent.color = Color.white;      
                ChangeReadyButtonColor(ReadyColor);
                readyButton.interactable = false;
                nameInput.interactable = false;
            }
            else
            {
                ChangeReadyButtonColor(isLocalPlayer ? JoinColor : NotReadyColor);

                Text textComponent = readyButton.transform.GetChild(0).GetComponent<Text>();
                textComponent.text = isLocalPlayer ? "READY" : "...";
                textComponent.color = Color.white;
                readyButton.interactable = isLocalPlayer;
                nameInput.interactable = isLocalPlayer;
            }

            ColorBlock readyButtonCollorBlock = readyButton.colors;
            readyButtonCollorBlock.highlightedColor = HighlightedColor;
            readyButton.colors = readyButtonCollorBlock;
        }

        public void OnMyName(string newName)
        {
            playerName = newName;
            nameInput.text = playerName;
        }

        public void OnReadyClicked()
        {
            SendReadyToBeginMessage();
        }

        public void OnNameChanged(string str)
        {
            CmdNameChanged(str);
        }

        public void ToggleJoinButton(bool enabled)
        {
            readyButton.gameObject.SetActive(enabled);
        }

        [ClientRpc]
        public void RpcUpdateCountdown(int countdown)
        {
            LobbyManager.singleton.countdownPanel.UIText.text = "Match Starting in " + countdown;
            LobbyManager.singleton.countdownPanel.gameObject.SetActive(countdown != 0);
        }

        [Command]
        public void CmdNameChanged(string name)
        {
            playerName = name;
        }

        [Command]
        public void CmdMessage(string message)
        {
            RpcBroacastMessage(message, playerName);
        }

        [ClientRpc]
        public void RpcBroacastMessage(string message, string playerNickname)
        {
            
            ChatPanel.singleton.CreateMessage(message, playerNickname);
        }

        public void OnDestroy()
        {
            LobbyPlayerList._instance.RemovePlayer(this);
            if (LobbyManager.singleton != null) LobbyManager.singleton.OnPlayersNumberModified(-1);
        }
    }
}
