using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public event Action OnCoinHit;
    public List<Coin> coinList;
	// Update is called once per frame
	void Start ()
    {
        coinList = new List<Coin>(FindObjectsOfType<Coin>());
        for (int i = 0; i < coinList.Count; i++)
            coinList[i].OnCoinHit += CoinHit;
    }

    private void CoinHit(Coin obj)
    {
        if (OnCoinHit != null)
            OnCoinHit();
    }
}
