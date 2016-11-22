using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellManager : MonoBehaviour
{
    public List<Shell> shells;

    public GameObject shellPrefab;
    public Transform kart;
    public Transform shellShootSpawn;

    public float shootCooldown = 3f;

    void Start ()
    {
        shells = new List<Shell>();
	}

    void Update()
    {
        shootCooldown += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Q) && shootCooldown >= 2f && shells.Count > 0)
        {
            shells[0].transform.position = shellShootSpawn.position;
            shells[0].SetShellRoaming(kart.forward);
            shells.RemoveAt(0);
        }
    }
	
    public void CreateShells()
    {
        if (shells.Count > 0)
            return;
        Shell __shell;
        for (int i = 0; i < 3; i++)
        {
            GameObject __go = Instantiate(shellPrefab);
            __shell = __go.GetComponent<Shell>();
            __shell.orbitTarget = kart;
            __shell.orbitAngle = 120f * i;
            shells.Add(__shell);
        }
    }
}
