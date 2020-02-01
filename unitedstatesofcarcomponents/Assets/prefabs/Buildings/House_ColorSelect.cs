using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House_ColorSelect : MonoBehaviour
{
    [Header("ColorParent")]
    public List<Sprite> sprites;

    [Header("MeshRenderer")]
    public MeshRenderer meshRenderer;
    
    [Header("Adress")]
    public string adress = "unknown";

    int randomIndex;
    
    private void OnEnable()
    {
        ChangeTexture();
    }

    public void ChangeTexture()
    {
        randomIndex = Random.Range(0, sprites.Count);

        meshRenderer.material.SetTexture("_MainTex", sprites[randomIndex].texture);
    }
}
