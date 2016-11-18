using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerFlag : MonoBehaviour {
    public int maxFlagValue;
    public static int currentFlagValue = 0;
    public static int laps = 0;
    public static string time;

    float startTime;

    void Start() {
        startTime = Time.time;
    }

	// Update is called once per frame
	void Update ()
    {
        if (currentFlagValue != 0 && currentFlagValue >= maxFlagValue)
        {
            currentFlagValue = 0;
            laps++;
            resetTime();
        }
        setTime();
	}

    private void setTime()
    {
        float timeDiff = Time.time - startTime;
        int min = (int)(Mathf.FloorToInt(timeDiff) / 60);
        int sec = Mathf.FloorToInt(timeDiff) % 60;
        int milisec = (int)((timeDiff - Mathf.Floor(timeDiff)) * 100);
        time = "   " + min + "' " + sec + "'' " + milisec;
    }

    private void resetTime()
    {
        startTime = Time.time;
    }
}
