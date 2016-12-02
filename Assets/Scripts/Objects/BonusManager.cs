using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BonusManager : NetworkBehaviour
{
    public event Action OnShellGot;
    public List<Bonus> bonusList;
    public List<Transform> bonusRowContainerList;
    public Transform bonusContainer;
    public GameObject bonusRowPrefab;
	// Use this for initialization
	void Start ()
    {
        if (!isServer)
            return;

        bonusList = new List<Bonus> (FindObjectsOfType<Bonus>());
        for (int i = 0; i < bonusList.Count; i++)
            bonusList[i].OnBonusHit += BonusHit;
        foreach (Transform __t in bonusRowContainerList)
            SpawnBonus(__t);
	}

    private void BonusHit(Bonus obj)
    {
        if (OnShellGot != null)
            OnShellGot();
    }

    public void SpawnBonus(Transform p_trans)
    {
        GameObject __go = (GameObject)Instantiate(bonusRowPrefab, p_trans.position, p_trans.rotation, bonusContainer.transform);
       // NetworkServer.S
    }
}
