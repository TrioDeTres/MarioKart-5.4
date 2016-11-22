using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoshiReferences : MonoBehaviour
{
    public SkinnedMeshRenderer bodyMesh;
    public SkinnedMeshRenderer eyeMesh;

    public GameObject kartContainer;
    void Start()
    {
        bodyMesh.materials[0].SetTexture("_MainTex", Resources.Load<Texture2D>("Textures/Yoshi/Yoshi_Black_Body"));
        eyeMesh.materials[0].SetTexture("_MainTex", Resources.Load<Texture2D>("Textures/Yoshi/Yoshi_Black_Eye"));
        Debug.Log(bodyMesh.materials[0].mainTexture);
    }
}
