using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectManager : MonoBehaviour
{

    public YoshiReferences  yoshi;
    public YoshiSkin        selectedSkin = YoshiSkin.GREEN;
    public KartType         selectedKart = KartType.STANDART;
    public List<Image>      portraits;
    public List<GameObject> kartPrefabs;
    void Start()
    {
        UpdateSelectedPortrait();
        SpawnKart();
    }
    void Update()
    {
        yoshi.transform.Rotate(Vector3.up * Time.deltaTime * 72f);
    }
    public void SpawnKart()
    {
        if (yoshi.kartContainer.transform.childCount > 0)
            Destroy(yoshi.kartContainer.transform.GetChild(0).gameObject);
        GameObject __go = Instantiate(kartPrefabs[(int)selectedKart]);
        __go.transform.parent = yoshi.kartContainer.transform;
        __go.transform.localPosition = kartPrefabs[(int)selectedKart].transform.position;
        __go.transform.localScale = Vector3.one;
        __go.transform.localRotation = Quaternion.identity;
    }
    public void UpdateSelectedPortrait()
    {
        for (int i = 0; i < portraits.Count; i++)
        {
            if (i == (int)selectedSkin)
                portraits[i].color = Color.cyan;
            else
                portraits[i].color = Color.white;
        }
    }
    public void CharactedSelected(int p_skinIndex)
    {
        if ((int)selectedSkin == p_skinIndex)
            return;
        selectedSkin = (YoshiSkin)p_skinIndex;
        UpdateSelectedPortrait();
        //yoshi.ChangeSkin(selectedSkin);
    }
}
