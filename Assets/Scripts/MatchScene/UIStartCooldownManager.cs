using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIStartCooldownManager : MonoBehaviour
{
    public Text timer;
    public GameObject cooldown;
    public AudioSource cooldownSound;
    void Start()
    {
        cooldownSound.PlayDelayed(0.5f);
    }

	// Update is called once per frame
	void Update ()
    {
	    if (GameSceneManager.instance.startCooldown >= 0f)
        {
            cooldown.SetActive(true);
            timer.text = ((int)GameSceneManager.instance.startCooldown).ToString();
        }
        else
            cooldown.SetActive(false);
    }
}
