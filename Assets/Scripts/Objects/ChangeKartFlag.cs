using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ChangeKartFlag : NetworkBehaviour
{
    public int flagValue = 0;
    public bool isWinBox;
    void OnTriggerEnter(Collider col)
    {
        if (!isServer)
            return;
        if (col.tag != "Player")
            return;
        PlayerManager __player = col.transform.parent.parent.parent.GetComponent<PlayerManager>();
        Debug.Log(__player.playerName);
        if (isWinBox && __player.laps == -1)
            __player.laps = 0;
        else if (isWinBox)
        {
            if (__player.lastFlagId == 37 && __player.flagIds.Count == 38)
            {
                __player.flagIds.Clear();
                __player.lastFlagId = -1;
                GameSceneManager.instance.AddPlayerLap(__player);
            }
        }
        else
        {
            //Progressing
            if (__player.lastFlagId < flagValue && !__player.flagIds.Contains(flagValue)
                && __player.lastFlagId == (flagValue - 1))
            {
                __player.lastFlagId = flagValue;
                __player.flagIds.Add(flagValue);
                UIWrongWayManager.isWrongWay = false;
            }
            //Regressing
            else if (__player.lastFlagId > flagValue && __player.flagIds.Contains(flagValue))
            {
                __player.flagIds.Remove(__player.lastFlagId);
                __player.lastFlagId = flagValue;
                //__player.flagIds.Remove(flagValue);
                UIWrongWayManager.isWrongWay = true;
            }
        }
    }
}
