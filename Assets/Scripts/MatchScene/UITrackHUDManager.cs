using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UITrackHUDManager : MonoBehaviour
{
    public RectTransform minPosHUD;
    public RectTransform maxPosHUD;
    public Transform minPosWorld;
    public Transform maxPosWorld;

    public List<Image> playerIcons;
    public Vector2 hudPosSize;
    public Vector3 worldPosSize;
    public Transform test;
	// Use this for initialization
	void Start ()
    {
        hudPosSize = maxPosHUD.anchoredPosition - minPosHUD.anchoredPosition;
        worldPosSize = maxPosWorld.position - minPosWorld.position;
    }
	
	void Update ()
    {
        UpdatePlayersIconPosition(GameSceneManager.instance.players);
	}
    public void UpdatePlayersIconPosition(List<PlayerManager> p_players)
    {
        for (int i = 0; i < 4; i++)
        {
            if (i < p_players.Count)
            {
                float __x = (p_players[i].transform.position.x - minPosWorld.position.x) / worldPosSize.x;
                float __y = (p_players[i].transform.position.z - minPosWorld.position.z) / worldPosSize.z;
                //NEED ID
                playerIcons[i].rectTransform.anchoredPosition = new Vector2(minPosHUD.anchoredPosition.x + (hudPosSize.x * __x),
                    minPosHUD.anchoredPosition.y + (hudPosSize.y * __y));
                playerIcons[i].color = Enums.YoshiSkinToColor((YoshiSkin)p_players[i].skin);
            }
            playerIcons[i].gameObject.SetActive(i < p_players.Count ? true : false);
        }
    }
}
