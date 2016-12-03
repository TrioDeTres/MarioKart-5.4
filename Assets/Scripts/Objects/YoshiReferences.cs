using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoshiReferences : MonoBehaviour
{
    public SkinnedMeshRenderer bodyMesh;
    public SkinnedMeshRenderer eyeMesh;

    public GameObject kartContainer;

    public void LoadSkin(YoshiSkin p_skin)
    {
        string __skinToString = Enums.YoshiSkinToString(p_skin);
        if (bodyMesh.materials[0].mainTexture != null && bodyMesh.materials[0].mainTexture.name == "Textures/Yoshi/Yoshi_" + __skinToString + "_Body")
            return;
        bodyMesh.materials[0].SetTexture("_MainTex", Resources.Load<Texture2D>("Textures/Yoshi/Yoshi_" + __skinToString + "_Body"));
        eyeMesh.materials[0].SetTexture("_MainTex", Resources.Load<Texture2D>("Textures/Yoshi/Yoshi_" + __skinToString + "_Eye"));
    }
}
