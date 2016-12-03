using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Vehicles.Car;
public class PlayerManager : NetworkBehaviour
{
    [SyncVar]
    public string playername;

    [SyncVar(hook = "UpdateSkin")]
    public int skin;
    
    public YoshiReferences references;
    public CarController carController;
    public CarUserControl carUserControl;

    public void Start()
    {
        if (isLocalPlayer)
        {
            carUserControl.isLocal = true;
            Camera.main.GetComponent<KartCameraController>().target = carController.transform;
            GameSceneManager.instance.player = this;
        }
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
        GameSceneManager.instance.shellManager.CreateShells(carController.transform);
    }
}
