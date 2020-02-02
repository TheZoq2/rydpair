using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    [Header("Fulhack")]
    public bool ignoreColorChange;

    [Header("ColorParent")]
    public List<Sprite> sprites;

    [Header("MeshRenderer")]
    public MeshRenderer meshRenderer;

    [Header("Adress")]
    public string adress = "";
    public Transform pickupPoint;

    CarPartFactory cpf;

    int randomIndex;
    
    private void OnEnable()
    {
        ChangeTexture();
        
        cpf = FindObjectOfType<CarPartFactory>();
    }

    private void FixedUpdate()
    {
        if (cpf.psychadellic)
        {
            ChangeTexture();
        }
    }

    public void ChangeTexture()
    {
        if (!ignoreColorChange)
        {
            randomIndex = Random.Range(0, sprites.Count);

            meshRenderer.material.SetTexture("_MainTex", sprites[randomIndex].texture);
        }
    }
}
