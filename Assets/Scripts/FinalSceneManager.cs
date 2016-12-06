using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Prototype.NetworkLobby;
using UnityEngine.Networking;

public class FinalSceneManager : NetworkBehaviour
{
    public List<GameObject> playerContainer;
    public List<YoshiReferences> yoshis;
    public List<Text> nameLabels;
    public List<Text> timerLabels;
    public List<Text> positionLabels;
    public List<Text> coinLabels;
    public List<Text> coinValueLabels;
    public List<PlayerCharacterSelect> players;
    // Use this for initialization
    void Start ()
    {
        players = PlayerCharacterSelectPoolUtil.Instance.GetPlayers();
        StartCoroutine(QuitApplication());
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (players.Count == 0)
            players = PlayerCharacterSelectPoolUtil.Instance.GetPlayers();
        for (int i = 0; i < 4; i ++)
        {
            if (i >= players.Count)
            {
                playerContainer[i].SetActive(false);
                yoshis[i].transform.parent.gameObject.SetActive(false);
                continue;
            }
            yoshis[i].transform.parent.Rotate(Vector3.up * Time.deltaTime * 72f);
            playerContainer[i].SetActive(true);
            yoshis[i].gameObject.SetActive(true);
            
            PlayerCharacterSelect __char = players[i];
            yoshis[i].LoadSkin(__char.selectedSkin);
            nameLabels[i].text = __char.playerName;
            timerLabels[i].text = UITimerManager.TimeToString(__char.trackTimerWithCoins);
            positionLabels[i].text = UILapsManager.GetPositionText(__char.endPosition) + " Place";
            coinLabels[i].text = "X " + __char.coinCount.ToString();
            coinValueLabels[i].text = "= 0." + (__char.endPosition + 1).ToString();
        }
	}
    IEnumerator QuitApplication()
    {
        yield return new WaitForSeconds(10f);
        Application.Quit();
        //NetworkManager.singleton.ServerChangeScene("Lobby");
    }
}
