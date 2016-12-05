using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class LapsManager : NetworkBehaviour
{
    public Transform trackBasePoint;
    public List<Transform> testPlayers;

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
        List<float> __progressions = new List<float>();
        foreach (PlayerManager __player in p_players)
        {
            __progressions.Add((__player.laps * 2f) + __player.lapProgression);
        }
        __progressions.Sort();
        foreach (PlayerManager __player in p_players)
        {
            __player.currentPlace = __progressions.Count - 1 - __progressions.IndexOf((__player.laps * 2f) + __player.lapProgression);
        }
    }
}
