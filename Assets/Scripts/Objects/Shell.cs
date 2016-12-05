using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Shell : NetworkBehaviour
{
    public enum ShellState
    {
        ORBIT,
        ROAMING
    }
    [SyncVar]
    public ShellState shellState = ShellState.ORBIT;

    public GameObject shellModel;
    public Rigidbody rigidBody;
    public Collider shellCollider;
    [SyncVar]
    public Transform orbitTarget;
    public float orbitRadius;
    [SyncVar]
    public float orbitAngle;
    public float orbitSpeed;
    [SyncVar]
    public Vector3 velocity;
    public float rotSpeed;
    void Start()
    {
        rigidBody.isKinematic = true;
        shellCollider.isTrigger = true;
    }
    void Update()
    {
        //if (isServer)
        {
            shellModel.transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime);

            if (shellState == ShellState.ORBIT)
            {
                orbitAngle += orbitSpeed * Time.deltaTime;
                transform.position = new Vector3(orbitRadius * Mathf.Cos(orbitAngle * Mathf.Deg2Rad), 0f,
                    orbitRadius * Mathf.Sin(orbitAngle * Mathf.Deg2Rad)) + orbitTarget.transform.position;
            }
        }
    }
    void FixedUpdate()
    {
        if (shellState == ShellState.ROAMING)
        {
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, 0f, rigidBody.velocity.z);
            rigidBody.velocity = rigidBody.velocity.normalized * 50f;


            //rigidBody.velocity = new Vector3(velocity.x, 0f, velocity.z);
            //rigidBody.velocity = rigidBody.velocity.normalized * 50f;

            if (isServer)
                velocity = rigidBody.velocity;
        }
    }

    public void SetShellRoaming(Vector3 p_direction)
    {
        shellState = ShellState.ROAMING;
        rigidBody.isKinematic = false;
        shellCollider.isTrigger = false;
        rigidBody.velocity = p_direction.normalized * 25f;
        velocity = p_direction.normalized * 25f;
        RpcSetShellVelocity(p_direction.normalized * 25f);
    }
    [ClientRpc]
    public void RpcSetShellVelocity(Vector3 p_velocity)
    {
        rigidBody.isKinematic = false;
        shellCollider.isTrigger = false;
        rigidBody.velocity = p_velocity;
    }
}
