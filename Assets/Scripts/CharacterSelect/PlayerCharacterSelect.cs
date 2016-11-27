using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Vehicles.Car;

public class PlayerCharacterSelect : NetworkBehaviour
{
    public enum PlayerStatus
    {
        CHOOSING,
        LOCKED
    }

    public YoshiReferences  yoshi;
    public PlayerStatus     status;
    public YoshiSkin        selectedSkin { get; private set; }
    public GameObject       yoshiKart;
    public BoxCollider      yoshiBottomCollider;
    public bool             isLocal;
    public PlayerManager    playerManager;

    public bool addedToManager;

    [SyncVar]
    public int id;

    public void Awake()
    {
        CarUserControl carUserControl = GetComponentInChildren<CarUserControl>();
        CarController carController = GetComponentInChildren<CarController>();
        WheelCollider[] wheelColliders = GetComponentsInChildren<WheelCollider>();
        NetworkTransform networkTransform = GetComponent<NetworkTransform>();
        NetworkTransformChild networkTransformChild = GetComponent<NetworkTransformChild>();

        carUserControl.enabled = false;
        carController.enabled = false;
        networkTransform.enabled = false;
        networkTransformChild.enabled = false;

        for (int i = 0; i < wheelColliders.Length; i++)
        {
            wheelColliders[i].enabled = false;
        }

        transform.localScale = Vector3.one * 0.6f;

        Vector3 colliderValues = yoshiBottomCollider.transform.localPosition;

        colliderValues.y = -0.1f;

        yoshiBottomCollider.transform.localPosition = colliderValues;
    }

    public override void OnStartLocalPlayer()
    {
        isLocal = true;
    }

    //Rotate Yoshi Model
    void Update()
    {
        yoshiKart.transform.Rotate(Vector3.up * Time.deltaTime * 72f);
    }

    //Define Player ID and Load a Preset Skin
    public void SetID(int p_id)
    {
        id = p_id;
        status = PlayerStatus.CHOOSING;
        ChangeSkin((YoshiSkin) id);
        yoshi.LoadSkin(selectedSkin);
        playerManager.id = id;
    }

    //Force Change Skin
    public void ChangeSkin(YoshiSkin p_skin)
    {
        selectedSkin = p_skin;
        yoshi.LoadSkin(selectedSkin);
        playerManager.skin = p_skin;
    }

    [Command]
    public void CmdUpdatePlayerSkin(int skin)
    {
        RpcUpdatePlayerSkin(skin);
    }

    [Command]
    public void CmdUpdatePlayerStatus(int status)
    {
        RpcUpdatePlayerStatus(status);
    }

    [Command]
    public void CmdUpdatePlayerName()
    {
        RpcUpdatePlayerName(playerManager.playername);
    }

    [ClientRpc]
    public void RpcUpdatePlayerSkin(int skin)
    {
        ChangeSkin((YoshiSkin)skin);
    }

    [ClientRpc]
    public void RpcUpdatePlayerStatus(int status)
    {
        this.status = (PlayerStatus) status;
    }

    [ClientRpc]
    public void RpcUpdatePlayerName(string playername)
    {
        playerManager.playername = playername;
    }
}
