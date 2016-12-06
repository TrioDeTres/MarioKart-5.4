using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class UITimerManager : MonoBehaviour
{
    public Text timerLabel;
	// Update is called once per frame
	void Update ()
    {
        timerLabel.text = TimeToString(GameSceneManager.instance.GetTimer());
    }

    public static string TimeToString(float p_timer)
    {
        TimeSpan __t = TimeSpan.FromSeconds(p_timer);
        string answer = string.Format("{0:D2}:{1:D2}:{2:D3}",
                __t.Minutes,
                __t.Seconds,
                __t.Milliseconds);
        return answer;
    }
}
