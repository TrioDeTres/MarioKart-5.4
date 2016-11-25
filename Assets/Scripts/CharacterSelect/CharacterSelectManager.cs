using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectManager : MonoBehaviour
{

    public YoshiReferences  yoshi;
    public List<GameObject> kartPrefabs;

    public List<PlayerCharacterSelect>  players;
    public List<YoshiPortrait>          portraits;
    public List<Image>                  playerStatusImage;
    public List<Text>                   playerStatusLabel;
    public RectTransform                youLabel;

    public int activePlayerID;

    void Start()
    {
        for (int i = 0; i < players.Count; i++)
            players[i].SetID(i);
        UpdateUI();
    }
   
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            ChangeActivePlayer(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            ChangeActivePlayer(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            ChangeActivePlayer(2);
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            ChangeActivePlayer(3);
        else if (Input.GetKeyDown(KeyCode.Return))
            LockPlayerDecision(true);
    }
    //Test-Only
    private void ChangeActivePlayer(int p_id)
    {
        if (p_id < players.Count)
        {
            activePlayerID = p_id;
            UpdateUI();
        }
    }
    //Not used
    public void SpawnKart()
    {
        /*if (yoshi.kartContainer.transform.childCount > 0)
            Destroy(yoshi.kartContainer.transform.GetChild(0).gameObject);
        GameObject __go = Instantiate(kartPrefabs[(int)selectedKart]);
        __go.transform.parent = yoshi.kartContainer.transform;
        __go.transform.localPosition = kartPrefabs[(int)selectedKart].transform.position;
        __go.transform.localScale = Vector3.one;
        __go.transform.localRotation = Quaternion.identity;*/
    }

    //Change player locked status and update the ui
    public void LockPlayerDecision(bool p_lock)
    {
        if (p_lock)
            players[activePlayerID].status = PlayerCharacterSelect.PlayerStatus.LOCKED;
        else
            players[activePlayerID].status = PlayerCharacterSelect.PlayerStatus.CHOOSING;
        UpdateUI();
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
        youLabel.anchoredPosition = playerStatusImage[activePlayerID].GetComponent<RectTransform>().anchoredPosition + (Vector2.up * 30f);
    }
    private void UpdatePortrait()
    {
        //Convert the active player skin to and ID
        int __skinID = (int)players[activePlayerID].selectedSkin;

        
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
                portraits[(int)players[i].selectedSkin].EnablePlayerLockedBar(i);
        }
    }
    private void UpdatePlayerStatus()
    {
        for(int i = 0; i < 4; i ++)
        {
            if (i < players.Count)
            {
                // Choosing: Light Blue
                if (players[i].status == PlayerCharacterSelect.PlayerStatus.CHOOSING)
                {
                    playerStatusImage[i].color = new Color(0.55f, 1f, 1f);
                    playerStatusLabel[i].text = "CHOOSING...";
                }
                // Ready: Light Green
                else
                {
                    playerStatusImage[i].color = new Color(0.25f, 1f, 0.55f);
                    playerStatusLabel[i].text = "READY!";
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
        if ((int)players[activePlayerID].selectedSkin == p_skinIndex)
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
        players[activePlayerID].ChangeSkin((YoshiSkin)p_skinIndex);
        UpdateUI();
    }
}
