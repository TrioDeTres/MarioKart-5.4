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

    void Awake()
    {
        throwShell = GetComponent<AudioSource>();
    }
	
    public void CreateShells(PlayerManager p_player)
    {
        Shell __shell;
        for (int i = 0; i < 3; i++)
        {
            GameObject __go = Instantiate(shellPrefab);
            __shell = __go.GetComponent<Shell>();
            __shell.orbitTarget = p_player.transform;
            __shell.orbitAngle = 120f * i;
            p_player.playerShells.Add(__shell);
            shells.Add(__shell);
            NetworkServer.Spawn(__go);
        }
    }
   public void ThrowShell(PlayerManager p_player)
    {
        if (p_player.bonusState != BonusState.SHELL)
            return;

        Shell __shell = p_player.playerShells[0];
        __shell.transform.position = p_player.shellSpawnPoint.position;
        __shell.SetShellRoaming(p_player.transform.forward);
        shells.Remove(__shell);
        p_player.playerShells.RemoveAt(0);

        if (p_player.playerShells.Count == 0)
            p_player.bonusState = BonusState.NOTHING;
    }
}
