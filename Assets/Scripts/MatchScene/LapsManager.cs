using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class LapsManager : NetworkBehaviour
{
    public Transform trackBasePoint;

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!isServer)
            return;
        foreach (PlayerManager __player in GameSceneManager.instance.players)
        {
            if (__player == null)
                continue;
            float __baseAngle = Mathf.Atan2(__player.transform.position.z - trackBasePoint.position.z,
                __player.transform.position.x - trackBasePoint.position.x) * Mathf.Rad2Deg;
            __baseAngle -= 180f;
            __baseAngle *= -1f;
            __player.lapProgression = __baseAngle / 360f;
        }
        UpdatePlayersPosition(GameSceneManager.instance.players);
        
	}
    private void UpdatePlayersPosition(List<PlayerManager> p_players)
    {
        List<float> __prog = new List<float>();
        foreach (PlayerManager __player in p_players)
        {
            __prog.Add((__player.laps * 2f) + __player.lapProgression);
        }
        __prog.Sort();
        foreach (PlayerManager __player in p_players)
        {
            __player.currentPlace = __prog.Count - 1 - __prog.IndexOf((__player.laps * 2f) + __player.lapProgression);
        }
    }
}
