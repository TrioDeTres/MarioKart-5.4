using UnityEngine;
using System.Collections;

public class PlayerCharacterSelect : MonoBehaviour
{
    public enum PlayerStatus
    {
        CHOOSING,
        LOCKED
    }
    public YoshiReferences  yoshi;
    public PlayerStatus     status;
    public YoshiSkin        selectedSkin { get; private set; }
    [SerializeField]
    private int id;

    //Rotate Yoshi Model
    void Update()
    {
        yoshi.transform.Rotate(Vector3.up * Time.deltaTime * 72f);
    }

    //Define Player ID and Load a Preset Skin
    public void SetID(int p_id)
    {
        id = p_id;
        status = PlayerStatus.CHOOSING;
        ChangeSkin((YoshiSkin)id);
        yoshi.LoadSkin(selectedSkin);
    }
    //Force Change Skin
    public void ChangeSkin(YoshiSkin p_skin)
    {
        selectedSkin = p_skin;
        yoshi.LoadSkin(selectedSkin);
    }
}
