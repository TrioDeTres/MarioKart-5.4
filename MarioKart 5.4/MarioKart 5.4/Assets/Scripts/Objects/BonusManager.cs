using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusManager : MonoBehaviour
{
    public event Action OnShellGot;
    public List<Bonus> bonusList;

	// Use this for initialization
	void Start ()
    {
        bonusList = new List<Bonus> (FindObjectsOfType<Bonus>());
        for (int i = 0; i < bonusList.Count; i++)
            bonusList[i].OnBonusHit += BonusHit;
	}

    private void BonusHit(Bonus obj)
    {
        if (OnShellGot != null)
            OnShellGot();
    }
}
