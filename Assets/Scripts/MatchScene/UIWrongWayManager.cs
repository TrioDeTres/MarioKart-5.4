using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIWrongWayManager : MonoBehaviour
{
    public static bool isWrongWay = false;
    public GameObject wrongWayContainer;
	
	void Update ()
    {
        wrongWayContainer.SetActive(isWrongWay);
	}
}
