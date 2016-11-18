using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeKartFlag : MonoBehaviour
{
    public static bool wrongWay = false;
    public int flagValue = 0;

    void OnTriggerEnter(Collider col)
    {
        if (col.tag != "Kart")
            return;
        if (TriggerFlag.currentFlagValue < flagValue && TriggerFlag.currentFlagValue != flagValue && (TriggerFlag.currentFlagValue + 1 == flagValue))
        {
            TriggerFlag.currentFlagValue = flagValue;
            Debug.Log("Flag " + flagValue + " current " + TriggerFlag.currentFlagValue);
            wrongWay = false;
        }
        else
        {
            wrongWay = true;
            Debug.Log("Wrong way");
        }
        
    }
}
