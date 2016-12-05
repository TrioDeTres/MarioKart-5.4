using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Vehicles.Car;
using System.Collections.Generic;
public class PlayerManager : NetworkBehaviour
{
    [SyncVar]
    public string playerName;

    [SyncVar(hook = "UpdateSkin")]
    public int skin;

    public PlayerMetadata metaData;
    public YoshiReferences references;
    public CarController carController;
    public CarUserControl carUserControl;

    [SyncVar]
    public int laps = -1;
    [SyncVar]
    public float lapProgression;
    [SyncVar]
    public int currentPlace;
    [SyncVar]
    public int finishedPlace = -1;
    [SyncVar]
    public float trackCompletionTime;
    [SyncVar]
    public bool trackCompleted;
    [SyncVar]
    public int coins = 0;
    [SyncVar]
    public BonusState   bonusState = BonusState.NOTHING;

    public Transform    shellSpawnPoint;
    public List<Shell>  playerShells;

    public void Start()
    {
        if (isLocalPlayer)
        {
            carUserControl.isLocal = true;
            Camera.main.GetComponent<KartCameraController>().target = carController.transform;
            GameSceneManager.instance.player = this;
            
        }
        GameSceneManager.instance.players.Add(this);
    }
    public void Update()
    {
        UpdateSkin(skin);
    }

    public void UpdateSkin(int skin)
    {
        references.LoadSkin((YoshiSkin) skin);
    }
    
    [ClientRpc]
    public void RpcUpdatePlayerPosition(Vector3 position)
    {
        transform.position = position;
    }

    [ClientRpc]
    public void RpcSpawnShell()
    {
        //if (bonusState == BonusState.NOTHING)
        //    GameSceneManager.instance.shellManager.CreateShells(this);
    }
    [Command]
    public void CmdPlayerShoot()
    {
        if (bonusState == BonusState.SHELL)
            GameSceneManager.instance.shellManager.ThrowShell(this);
    }
}
