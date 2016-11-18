using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KartController : MonoBehaviour
{
    public WheelCollider[] wheels;
    public Transform centerOfMasss;
    public float maxMotorTorque;
    public float maxSteeringAngle;
    public float maxSpeed = 60;
    public float halfSpeed;

    public List<AudioSource> soundEffects;

    private new Rigidbody rigidbody;
    private bool isBraking;
    private Vector3 initialPosition;

    private WheelFrictionCurve sidewayFriction;
    private WheelFrictionCurve forwardFriction;
    private JointSpring targetPosition;

    private float timeAfterBrake;

    [SerializeField]
    public float speed;

    public void Start()
    {
        initialPosition = transform.position;

        rigidbody = GetComponent<Rigidbody>();

        rigidbody.centerOfMass = centerOfMasss.localPosition;

        halfSpeed =  maxSpeed / 2.0f;

        sidewayFriction = wheels[0].sidewaysFriction;
        forwardFriction = wheels[0].forwardFriction;
        targetPosition = wheels[0].suspensionSpring;

        foreach (WheelCollider w in wheels)
        {
            w.ConfigureVehicleSubsteps(1000, 1, 1);
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public float CurrentSpeed()
    {
        float wheelCircunference = wheels[0].radius * 2 * Mathf.PI;
        float rpm = (wheels[0].rpm + wheels[1].rpm) / 2.0f;
        return wheelCircunference * rpm * 60.0f / 1000.0f;
    }

    public void FixedUpdate()
    {
        float forward = maxMotorTorque * Input.GetAxis("Vertical");
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
        
        float speed = Vector3.Dot(rigidbody.velocity, transform.forward) * 3.6f;


        speed = CurrentSpeed();

        Brake();

        wheels[0].steerAngle = steering;
        wheels[1].steerAngle = steering;
        
        if (!isBraking)
        {
            if (speed < maxSpeed && forward > 0)
            {
                wheels[0].motorTorque = forward;
                wheels[1].motorTorque = forward;
                wheels[2].motorTorque = forward;
                wheels[3].motorTorque = forward;

            }
            //Backwards
            else if (forward < 0)
            {
                wheels[0].motorTorque = forward * 0.5f;
                wheels[1].motorTorque = forward * 0.5f;
                wheels[2].motorTorque = forward * 0.5f;
                wheels[3].motorTorque = forward * 0.5f;
            }
            else
            {
                wheels[0].motorTorque = 0;
                wheels[1].motorTorque = 0;
                wheels[2].motorTorque = 0;
                wheels[3].motorTorque = 0;
            }
        }
        else
        {
            wheels[0].motorTorque = 0;
            wheels[1].motorTorque = 0;
            wheels[2].motorTorque = 0;
            wheels[3].motorTorque = 0;
        }

        UpdateWheelFriction();
        PlaySounds(forward);
    }

    private void Brake()
    {
        if ( Input.GetKey(KeyCode.Space))
        {
            isBraking = true;
            wheels[0].brakeTorque = 250000.0f;
            wheels[1].brakeTorque = 250000.0f;
        }
        else {
            isBraking = false;
            wheels[0].brakeTorque = 0.0f;
            wheels[1].brakeTorque = 0.0f;
        }
    }

    private void UpdateWheelFriction()
    {
        float slipSpeed = halfSpeed + halfSpeed * 0.5f;

        foreach (var w in wheels)
        {
            if (speed > slipSpeed)
            {
                WheelFrictionCurve forward = w.forwardFriction;

                forward.extremumSlip = 1.3f;
                forward.extremumValue = 2.0f;
                forward.asymptoteSlip = 0.8f;
                forward.asymptoteValue = 0.5f;
                forward.stiffness = 0.7f;

                w.forwardFriction = forward;

                WheelFrictionCurve sideway = w.sidewaysFriction;

                sideway.extremumSlip = 0.4f;
                sideway.extremumValue = 1.0f;
                sideway.asymptoteSlip = 0.5f;
                sideway.asymptoteValue = 1.0f;
                sideway.stiffness = 1.6f;

                w.sidewaysFriction = sideway;

                w.suspensionSpring = targetPosition;
            }
            else
            {
                w.forwardFriction = forwardFriction;
                w.sidewaysFriction = sidewayFriction;
                w.suspensionSpring = targetPosition;
            }

            
        }

        if (isBraking || timeAfterBrake > 0)
        {
            WheelCollider w = wheels[2];

            WheelFrictionCurve forward = w.forwardFriction;
            WheelFrictionCurve sideway = w.sidewaysFriction;

            forward.stiffness = 1.2f;

            sideway.extremumSlip = 1.0f;
            sideway.extremumValue = 1.4f;
            sideway.asymptoteSlip = 0.5f;
            sideway.asymptoteValue = 0.9f;
            sideway.stiffness = 1.5f;

            JointSpring targetPosition = w.suspensionSpring;

            targetPosition.targetPosition = 0.35f;

            wheels[0].suspensionSpring = targetPosition;
            wheels[0].sidewaysFriction = sideway;
            wheels[0].forwardFriction = forward;

            wheels[1].suspensionSpring = targetPosition;
            wheels[1].sidewaysFriction = sideway;
            wheels[1].forwardFriction = forward;

            wheels[2].suspensionSpring = targetPosition;
            wheels[2].sidewaysFriction = sideway;
            wheels[2].forwardFriction = forward;

            wheels[3].suspensionSpring = targetPosition;
            wheels[3].sidewaysFriction = sideway;
            wheels[3].forwardFriction = forward;

            if (isBraking)
            {
                timeAfterBrake = 0.3f;
            }
            else
            {
                timeAfterBrake -= Time.deltaTime;
            }
        }

        timeAfterBrake = Mathf.Clamp01(timeAfterBrake);
    }

    private void PlaySounds(float motor)
    {
        if (!isBraking)
        {
            if (motor > 0.3)
            {
                soundEffects[0].Play();

                soundEffects[1].Stop();
                soundEffects[2].Stop();
                soundEffects[3].Stop();
            }
            else if (motor > 0.0)
            {
                soundEffects[1].Play();

                soundEffects[0].Stop();
                soundEffects[2].Stop();
                soundEffects[3].Stop();
            }
            else if (motor < 0)
            {
                soundEffects[1].Play();

                soundEffects[0].Stop();
                soundEffects[2].Stop();
                soundEffects[3].Stop();
            }
        }
        else {
            soundEffects[3].Play();

            soundEffects[0].Stop();
            soundEffects[1].Stop();
            soundEffects[2].Stop();
        }
    }
}