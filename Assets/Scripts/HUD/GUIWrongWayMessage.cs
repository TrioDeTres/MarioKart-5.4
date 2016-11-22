using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIWrongWayMessage : MonoBehaviour
{
    public GameObject wrongWayGO;

	void LateUpdate ()
    {
        wrongWayGO.SetActive(ChangeKartFlag.wrongWay);
	}
}
