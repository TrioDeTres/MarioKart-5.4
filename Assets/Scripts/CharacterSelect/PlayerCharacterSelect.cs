using UnityEngine;
using UnityEngine.Networking;

public class PlayerCharacterSelect : NetworkBehaviour
{
    public enum PlayerStatus
    {
        CHOOSING,
        LOCKED
    }

    [SyncVar]
    public string               playername;

    [SyncVar(hook = "SetID")]
    public int                  id = -1;

    public YoshiReferences      yoshi;
    public PlayerStatus         status;
    public YoshiSkin            selectedSkin { get; private set; }
    

    public void Start()
    {
        PlayerCharacterSelectPoolUtil.Instance.AddPlayer(this);
    }

    //Rotate Yoshi Model
    void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * 72f);
    }

    //Define Player ID and Load a Preset Skin
    public void SetID(int p_id)
    {
        id = p_id;
        status = PlayerStatus.CHOOSING;
        ChangeSkin((YoshiSkin) p_id);
        yoshi.LoadSkin(selectedSkin);
    }

    //Force Change Skin
    public void ChangeSkin(YoshiSkin p_skin)
    {
        selectedSkin = p_skin;
        yoshi.LoadSkin(selectedSkin);
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

    [ClientRpc]
    public void RpcUpdatePlayerSkin(int skin)
    {
        ChangeSkin((YoshiSkin) skin);
    }

    [ClientRpc]
    public void RpcUpdatePlayerStatus(int status)
    {
        this.status = (PlayerStatus) status;
    }
    [ClientRpc]
    public void RpcUpdateID(int p_id)
    {
        SetID(p_id);
    }
}
