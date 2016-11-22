using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateLastTrigger : MonoBehaviour
{
    public GameObject lastTrigger;

    void OnTriggerExit(Collider col)
    {
        lastTrigger.SetActive(true);
        Debug.Log("Last Trigger Active");
    }
}
