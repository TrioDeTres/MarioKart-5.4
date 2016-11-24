using UnityEngine;
using System.Collections;

public class KartHitScr : MonoBehaviour {

	AudioSource hitEffect;

	// Use this for initialization
	void Start () {
		hitEffect = GetComponent<AudioSource> ();
	}
	
	void OnTriggerEnter(Collider col){
		if (col.tag == "Kart") {
			hitEffect.loop = false;
			hitEffect.Play ();
		}
	}
}
