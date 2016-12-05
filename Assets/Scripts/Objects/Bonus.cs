using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Bonus : NetworkBehaviour
{
    public event Action<Bonus>  OnBonusHit;

    public MeshRenderer         meshRenderer;
    public Collider             bonusCollider;
    public float                rotSpeed = 180f;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        bonusCollider = GetComponent<SphereCollider>();
    }

	void Update ()
    {
        transform.Rotate(0, rotSpeed * Time.deltaTime, 0);
    }

    void OnTriggerEnter(Collider col)
    {
        if (isServer && col.tag == "Player")
        {
            PlayerManager __player = col.transform.parent.parent.parent.GetComponent<PlayerManager>();
            Debug.Log(__player.playerName);
            RpcEnableBonus(false);
            StartCoroutine(AppearBonus());

            if (__player.bonusState == BonusState.NOTHING)
            {
                GameSceneManager.instance.shellManager.CreateShells(__player);
                //__player.RpcSpawnShell();
                __player.bonusState = BonusState.SHELL;
            }
            
        }
    }

    IEnumerator AppearBonus()
    {
        yield return new WaitForSeconds(3.5f);
        if (isServer)
            RpcEnableBonus(true);
    }

    [ClientRpc]
    public void RpcEnableBonus(bool p_enable)
    {
        meshRenderer.enabled = p_enable;
        bonusCollider.enabled = p_enable;
    }
}
