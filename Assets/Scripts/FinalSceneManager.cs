using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Prototype.NetworkLobby;

public class FinalSceneManager : MonoBehaviour
{
    public List<GameObject> playerContainer;
    public List<PlayerCharacterSelect> players;
    public List<Text> nameLabels;
    public List<Text> timerLabels;
    public List<Text> positionLabels;
    public List<Text> coinLabels;
    public List<Text> coinValueLabels;
    public Dictionary<int, PlayerMetadata> playersMeta;
    // Use this for initialization
    void Start ()
    {
        playersMeta = LobbyManager.singleton.playerMetadata;
    }
	
	// Update is called once per frame
	void Update ()
    {
        for (int i = 0; i < 4; i ++)
        {
            if (i >= playersMeta.Count)
            {
                playerContainer[i].SetActive(false);
                players[i].gameObject.SetActive(false);
                continue;
            }
            playerContainer[i].SetActive(true);
            players[i].gameObject.SetActive(true);
           
            PlayerMetadata __meta = playersMeta[i];
            players[i].ChangeSkin(__meta.skin);
            nameLabels[i].text = __meta.playername;
            timerLabels[i].text = UITimerManager.TimeToString(__meta.trackTimerWithCoins);
            positionLabels[i].text = UILapsManager.GetPositionText(__meta.endPosition) + " Place";
            coinLabels[i].text = "X " + __meta.coinCount.ToString();
            coinValueLabels[i].text = "= 0." + (__meta.endPosition + 1).ToString();
        }
	}
}
