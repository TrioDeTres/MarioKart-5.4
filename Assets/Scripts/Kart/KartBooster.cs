using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartBooster : MonoBehaviour {

    public GameObject kart;
    KartController kartController;

    void Awake() {
        kartController = kart.GetComponent<KartController>();
    }

    void OnTriggerEnter() {
        float originalMaxSpeed = kartController.maxSpeed;
        kartController.maxSpeed *= 2;
        kartController.halfSpeed = kartController.maxSpeed / 2;
        Debug.Log(kart.GetComponent<Rigidbody>().velocity);
        kart.GetComponent<Rigidbody>().velocity += (kart.transform.forward * 20f);
        kartController.speed = kartController.CurrentSpeed();

        StartCoroutine(WaitTime());

        kartController.maxSpeed = originalMaxSpeed;
        kartController.halfSpeed = kartController.maxSpeed / 2;


    }

    IEnumerator WaitTime() {
        yield return new WaitForSeconds(3.0f);
    }
}
