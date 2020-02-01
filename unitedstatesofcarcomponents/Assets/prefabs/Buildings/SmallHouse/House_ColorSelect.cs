﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House_ColorSelect : MonoBehaviour
{
    [Header("ColorParent")]
    public List<Sprite> sprites;

    [Header("Material")]
    public Material houseMat;

    public MeshRenderer meshRenderer;

    int randomIndex;
    
    private void OnEnable()
    {
        ChangeTexture();
    }

    void ChangeTexture()
    {
        randomIndex = Random.Range(0, sprites.Count);

        meshRenderer.material.SetTexture("_MainTex", sprites[randomIndex].texture);
    }
}