using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIPlayerPortraitsManager : MonoBehaviour
{
    public List<Sprite> yoshiPortraitsList;
    public List<Image>  playerPortrait;
    public List<Image>  playerPortraitFrame;

    void Update()
    {
        UpdatePlayerPortrait(GameSceneManager.instance.players);
    }
    public void UpdatePlayerPortrait(List<PlayerManager> p_players)
    {
        for (int i = 0; i < 4; i++)
        {
            if (i < p_players.Count)
            {
                playerPortrait[p_players[i].currentPlace].sprite = yoshiPortraitsList[p_players[i].skin];
                playerPortraitFrame[p_players[i].currentPlace].color = Enums.YoshiSkinToColor((YoshiSkin)p_players[i].skin);
            }
            playerPortrait[i].transform.parent.gameObject.SetActive(i < p_players.Count ? true : false);
        }
    }
}
