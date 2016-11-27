using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class YoshiPortrait : MonoBehaviour
{
    public GameObject   playerLockedBar;
    public Text         playerName;
    public Animator     portraitAnimator;

 
    public void SetAnimatorSelected(bool p_selected)
    {
        portraitAnimator.SetBool("Selected", p_selected);
    }
    public void DisablePlayerLockedBar()
    {
        playerLockedBar.SetActive(false);
    }
    public void EnablePlayerLockedBar(string playername)
    {
        playerLockedBar.SetActive(true);
        playerName.text = playername;
    }
}
