using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Vehicles.Car;

public class PlayerManager : NetworkBehaviour
{
    public string playername;
    public YoshiSkin skin;
    public int id;
    public BoxCollider yoshiBottomCollider;

    public void Start()
    {
        DontDestroyOnLoad(this);
    }

    [ClientRpc]
    public void RpcResetPlayerComponents()
    {
        CarUserControl carUserControl = GetComponentInChildren<CarUserControl>();
        CarController carController = GetComponentInChildren<CarController>();
        WheelCollider[] wheelColliders = GetComponentsInChildren<WheelCollider>();
        NetworkTransform networkTransform = GetComponent<NetworkTransform>();
        NetworkTransformChild networkTransformChild = GetComponent<NetworkTransformChild>();
        PlayerCharacterSelect playerCharacterSelect = GetComponent<PlayerCharacterSelect>();

        playerCharacterSelect.enabled = false;
        carUserControl.enabled = true;
        carController.enabled = true;
        networkTransform.enabled = true;
        networkTransformChild.enabled = true;

        for (int i = 0; i < wheelColliders.Length; i++)
        {
            wheelColliders[i].enabled = true;
        }

        transform.localScale = Vector3.one;

        Vector3 colliderValues = yoshiBottomCollider.transform.localPosition;

        colliderValues.y = 0.4f;

        yoshiBottomCollider.transform.localPosition = colliderValues;
    }

    [ClientRpc]
    public void RpcUpdatePlayerPosition(Vector3 position)
    {
        transform.position = position;
    }
}
