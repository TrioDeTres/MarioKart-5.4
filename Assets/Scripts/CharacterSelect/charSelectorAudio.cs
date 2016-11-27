using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharSelectorAudio : MonoBehaviour {
	private AudioSource[] charAwakeEffect;

	// Use this for initialization
	void Start () {
		charAwakeEffect = GetComponents<AudioSource> ();
		charAwakeEffect [0].Play ();


	}
		
	void Update(){
		if (!charAwakeEffect[1].isPlaying) {
			charAwakeEffect [1].Play ();
		}
	}

}
