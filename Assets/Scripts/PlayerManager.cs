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

    public PlayerMetadata   metaData;
    public YoshiReferences  references;
    public CarController    carController;
    public CarUserControl   carUserControl;
    public Rigidbody        rigidBody;
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
    [SyncVar]
    public bool hasControl;

    public Transform    shellSpawnPoint;
    public List<Shell>  playerShells;
    public List<int>    flagIds = new List<int>();
    public int          lastFlagId = -1;

    public void Start()
    {
        if (isLocalPlayer)
        {
            carUserControl.hasControl = hasControl;
            carUserControl.isLocal = true;
            Camera.main.GetComponent<KartCameraController>().target = carController.transform;
            GameSceneManager.instance.player = this;
        }
        GameSceneManager.instance.players.Add(this);
    }
    public void Update()
    {
        UpdateSkin(skin);
        if (carUserControl.hasControl != hasControl)
            carUserControl.hasControl = hasControl;
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
    [ClientRpc]
    public void RpcPlayerDamaged(Vector3 p_angularVelocity)
    {
        rigidBody.angularVelocity = p_angularVelocity;
    } 
    [Command]
    public void CmdPlayerShoot()
    {
        if (bonusState == BonusState.SHELL)
            GameSceneManager.instance.shellManager.ThrowShell(this);
    }
}
