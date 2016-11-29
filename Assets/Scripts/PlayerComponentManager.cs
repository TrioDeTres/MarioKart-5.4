using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Vehicles.Car;

public class PlayerComponentManager : MonoBehaviour
{
    public BoxCollider yoshiBottomCollider;

    public void EnableComponents(ComponentState state)
    {
        if (state == ComponentState.CHAR_SELECT)
        {
            EnableCharSelectionState();
        }
        else if (state == ComponentState.MATCH)
        {
            EnableMatchState();
        }
    }

    public void EnableMatchState()
    {
        PlayerManager playerManager = GetComponentInChildren<PlayerManager>();
        PlayerCharacterSelect playerCharacterSelect = GetComponentInChildren<PlayerCharacterSelect>();

        playerManager.enabled = true;
        playerCharacterSelect.enabled = false;
    }

    public void EnableCharSelectionState()
    {
        PlayerManager playerManager = GetComponentInChildren<PlayerManager>();
        PlayerCharacterSelect playerCharacterSelect = GetComponentInChildren<PlayerCharacterSelect>();
        CarUserControl carUserControl = GetComponentInChildren<CarUserControl>();
        CarController carController = GetComponentInChildren<CarController>();
        WheelCollider[] wheelColliders = GetComponentsInChildren<WheelCollider>();
        NetworkTransform networkTransform = GetComponent<NetworkTransform>();
        NetworkTransformChild networkTransformChild = GetComponent<NetworkTransformChild>();

        carUserControl.enabled = false;
        carController.enabled = false;
        networkTransform.enabled = false;
        networkTransformChild.enabled = false;
        playerManager.enabled = false;
        playerCharacterSelect.enabled = true;

        for (int i = 0; i < wheelColliders.Length; i++)
        {
            wheelColliders[i].enabled = false;
        }

        transform.localScale = Vector3.one * 0.6f;

        Vector3 colliderValues = yoshiBottomCollider.transform.localPosition;

        colliderValues.y = -0.1f;

        yoshiBottomCollider.transform.localPosition = colliderValues;
    }
}
