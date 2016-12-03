using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ShellManager : NetworkBehaviour
{
    public List<Shell> shells;

    public GameObject shellPrefab;
    public Transform kart;
    public Transform shellShootSpawn;

    public float shootCooldown = 3f;

    AudioSource throwShell;

    void Start ()
    {
        shells = new List<Shell>();

    }

    void Awake() {
        throwShell = GetComponent<AudioSource>();
    }

    void Update()
    {
        shootCooldown += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Q) && shootCooldown >= 2f && shells.Count > 0)
        {
            throwShell.Play();
            shells[0].transform.position = shellShootSpawn.position;
            shells[0].SetShellRoaming(kart.forward);
            shells.RemoveAt(0);
        }
    }
	
    public void CreateShells(Transform p_player)
    {
        Shell __shell;
        for (int i = 0; i < 3; i++)
        {
            GameObject __go = Instantiate(shellPrefab);
            __shell = __go.GetComponent<Shell>();
            __shell.orbitTarget = p_player;
            __shell.orbitAngle = 120f * i;
            shells.Add(__shell);
            NetworkServer.Spawn(__go);
        }
    }
   
}
