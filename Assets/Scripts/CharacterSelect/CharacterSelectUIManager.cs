using System.Collections;
using System.Collections.Generic;
using Prototype.NetworkLobby;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CharacterSelectUIManager : MonoBehaviour
{
    public List<YoshiPortrait>          portraits;
    public List<Image>                  playerStatusImage;
    public List<Text>                   playerStatusLabel;
    public List<Transform>              playerSpots;
    
    public RectTransform                youLabel;

    public int selectedSkin = -1;

    public void UpdateYouLabel(int p_playerID)
    {
        youLabel.anchoredPosition = playerStatusImage[p_playerID].rectTransform.anchoredPosition + Vector2.up * 32.0f;
    }
    public void UpdatePortrait(List<PlayerCharacterSelect> p_players, int p_selectedSkin)
    {
        selectedSkin = p_selectedSkin;
        //Convert the active player skin to and ID

        for (int i = 0; i < portraits.Count; i++)
        {
            //Enable the white/red animation on the portrait of the selected skin
            portraits[i].SetAnimatorSelected(selectedSkin == i ? true : false);
            //Disable all portraits Locked Bar
            portraits[i].DisablePlayerLockedBar();
        }
        for (int i = 0; i < p_players.Count; i++)
        {
            //Enable Locked Bar for the skins selected by locked players
            if (p_players[i].status == PlayerCharacterSelect.PlayerStatus.LOCKED)
            {
                portraits[(int)p_players[i].selectedSkin].EnablePlayerLockedBar(p_players[i].playername);
            }
        }
    }
    public void UpdatePlayerStatus(List<PlayerCharacterSelect> p_players)
    {
        for (int i = 0; i < 4; i++)
        {
            if (i < p_players.Count)
            {
                playerStatusImage[i].gameObject.SetActive(true);
                playerStatusImage[i].gameObject.SetActive(true);
                

                // Choosing: Light Blue
                if (p_players[i].status == (int)PlayerCharacterSelect.PlayerStatus.CHOOSING)
                {
                    playerStatusImage[p_players[i].id].color = new Color(0.55f, 1f, 1f);
                    playerStatusLabel[p_players[i].id].text = "Choosing...";
                }
                // Ready: Light Green
                else
                {
                    playerStatusImage[p_players[i].id].color = new Color(0.25f, 1f, 0.55f);
                    playerStatusLabel[p_players[i].id].text = "Ready!";
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
}
