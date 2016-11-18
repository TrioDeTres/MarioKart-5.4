using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    public enum ShellState
    {
        ORBIT,
        ROAMING
    }
    public ShellState shellState = ShellState.ORBIT;

    public GameObject   shellModel;
    public Rigidbody    rigidBody;
    public Collider     shellCollider;

    public Transform    orbitTarget;
    public float        orbitRadius;
    public float        orbitAngle;
    public float        orbitSpeed;

    public float        rotSpeed;
    
    void Start()
    {
        rigidBody.isKinematic = true;
        shellCollider.isTrigger = true;
    }
    void Update()
    {
        shellModel.transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime);

        if (shellState == ShellState.ORBIT)
        {
            orbitAngle += orbitSpeed * Time.deltaTime;
            transform.position = new Vector3(orbitRadius * Mathf.Cos(orbitAngle * Mathf.Deg2Rad), 0f,
                orbitRadius * Mathf.Sin(orbitAngle * Mathf.Deg2Rad)) + orbitTarget.transform.position;
        }
    }
    void FixedUpdate ()
    {
        if (shellState == ShellState.ROAMING)
            rigidBody.velocity = rigidBody.velocity.normalized * 50f;
	}

    public void SetShellRoaming(Vector3 p_direction)
    {
        shellState = ShellState.ROAMING;
        rigidBody.isKinematic = false;
        shellCollider.isTrigger = false;
        rigidBody.velocity = p_direction.normalized * 25f;
    }
}
