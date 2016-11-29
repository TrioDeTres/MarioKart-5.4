using UnityEngine;
using UnityEngine.Networking;

public class PlayerManager : NetworkBehaviour
{
    [SyncVar]
    public string playername;

    [SyncVar(hook = "UpdateSkin")]
    public int skin;

    public YoshiReferences references;

    public void Start() {}
    public void Update() { }

    public void UpdateSkin(int skin)
    {
        references.LoadSkin((YoshiSkin) skin);
    }

    [ClientRpc]
    public void RpcUpdatePlayerPosition(Vector3 position)
    {
        transform.position = position;
    }
}
