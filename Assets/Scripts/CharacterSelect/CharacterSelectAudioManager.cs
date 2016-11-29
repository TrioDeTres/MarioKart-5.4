using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterSelectAudioManager : MonoBehaviour
{
    public AudioSource selectYourPlayerSource;
    public AudioSource bgmSource;
    
    void Start()
    {
        StartCoroutine(SelectYourPlayerDelay());
    }
    private IEnumerator SelectYourPlayerDelay()
    {
        yield return new WaitForSeconds(1.5f);
        selectYourPlayerSource.Play();
    }

}
