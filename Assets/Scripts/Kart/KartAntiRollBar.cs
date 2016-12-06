﻿using UnityEngine;
using System.Collections;

public class KartAntiRollBar : MonoBehaviour
{
    public WheelCollider    WheelL;
    public WheelCollider    WheelR;
    public Rigidbody        rigidBody;
    public float AntiRoll = 5000.0f;

    public void FixedUpdate()
    {
        WheelHit hit;
        float travelL = 1.0f;
        float travelR = 1.0f;

        bool groundedL = WheelL.GetGroundHit(out hit);
        if (groundedL)
            travelL = (-WheelL.transform.InverseTransformPoint(hit.point).y - WheelL.radius) / WheelL.suspensionDistance;

        bool groundedR = WheelR.GetGroundHit(out hit);
        if (groundedR)
            travelR = (-WheelR.transform.InverseTransformPoint(hit.point).y - WheelR.radius) / WheelR.suspensionDistance;

        float antiRollForce = (travelL - travelR) * AntiRoll;

        if (groundedL)
            rigidBody.AddForceAtPosition(WheelL.transform.up * -antiRollForce,
                   WheelL.transform.position);
        if (groundedR)
            rigidBody.AddForceAtPosition(WheelR.transform.up * antiRollForce,
                   WheelR.transform.position);
    }

}
