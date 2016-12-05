using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class Coin : NetworkBehaviour
{
    public event Action<Coin> OnCoinHit;

    public MeshRenderer meshRenderer;
    public Collider coinCollider;
    public AudioSource coinEffects;
    public float rotSpeed = 180f;

    void Awake()
    {
        meshRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
        coinCollider = GetComponent<SphereCollider>();
		coinEffects = GetComponent<AudioSource> ();
    }
    void Update()
    {
        transform.Rotate(0, rotSpeed * Time.deltaTime, 0);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            if (isServer)
            {
                PlayerManager __player = col.transform.parent.parent.parent.GetComponent<PlayerManager>();
                
                RpcEnableCoin(false);
                StartCoroutine(AppearCoin());
                __player.coins++;
            }
            meshRenderer.enabled = false;
            coinCollider.enabled = false;
            
            coinEffects.Play();
        }
    }
    IEnumerator AppearCoin()
    {
        yield return new WaitForSeconds(3.0f);
        coinCollider.enabled = true;
        if (isServer)
            RpcEnableCoin(true);
    }

    [ClientRpc]
    public void RpcEnableCoin(bool p_enable)
    {
        meshRenderer.enabled = p_enable;
        coinCollider.enabled = p_enable;
    }
}
