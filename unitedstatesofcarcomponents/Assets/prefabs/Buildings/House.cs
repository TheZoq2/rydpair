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
    Car c;

    Vector3 scale;
    Quaternion rotation;

    int randomIndex;
    bool recentlyTrippy;

    float tripTimer = 3;
    
    private void OnEnable()
    {
        ChangeTexture();
        
        cpf = FindObjectOfType<CarPartFactory>();
        c = FindObjectOfType<Car>();

        scale = transform.localScale;
        rotation = transform.rotation;
    }

    private void FixedUpdate()
    {
        if (c.psychadellic && !ignoreColorChange)
        {
            Psycho();
            recentlyTrippy = true;
        }
        else if (recentlyTrippy)
        {
            transform.localScale = scale;
            transform.rotation = rotation;
            recentlyTrippy = false;
        }
    }

    void Psycho()
    {
        #region Color
        if (tripTimer <= 0)
        {
            ChangeTexture();
            tripTimer = 3;
        }
        else
        {
            tripTimer -= 0.2f;
        }
        #endregion Color

        #region Transform
        float sin = Mathf.Max(0.7f*Mathf.Sin(Time.time), -0.7f * Mathf.Sin(Time.time));
        float sin2 = Mathf.Max(0.5f, sin);
        transform.localScale = new Vector3(sin2, sin2, sin2);
        transform.Rotate(Vector3.up, sin);
        #endregion Transform
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
