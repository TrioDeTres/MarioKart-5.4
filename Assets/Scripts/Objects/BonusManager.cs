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

	
    void Start()
    {

    }
}
