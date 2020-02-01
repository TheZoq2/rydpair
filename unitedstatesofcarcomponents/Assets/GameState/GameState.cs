using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    #region Declarations
    [Header("Package")]
    public GameObject packagePrefab;

    [Header("Buildings")]
    public List<House> houses;

    [Header("Target House")] // The package should be delivered to this adress
    public House targetHouse;
    public string targetAdress;

    [Header("Pickup House")] // The package spawns at this adress
    public House pickupHouse;
    public string pickupAdress;

    [Header("Variables")]
    public int playerScore;
    public float maxTime = 300f;
    
    int randomIndex;
    bool hasPackage;
    float timer;
    #endregion Declarations

    private void FixedUpdate()
    {
        if (hasPackage)
        {
            timer -= Time.deltaTime;
        }
    }

    #region packages
    public void PickUpPackage()
    {
        hasPackage = true;
        timer = maxTime;
    }

    public void DeliverPackage()
    {
        hasPackage = false;

        GiveScore();

        SpawnPackage();
    }

    public void SpawnPackage()
    {
        ResetAdresses();

        AssignPickupAdress();
        Instantiate(packagePrefab, pickupHouse.pickupPoint);

        AssignTargetAdress();
    }

    void AssignPickupAdress()
    {
        randomIndex = Random.Range(0, houses.Count);
        pickupHouse = houses[randomIndex];
        pickupAdress = pickupHouse.adress;
    }

    void AssignTargetAdress()
    {
        while(targetHouse != pickupHouse)
        {
            randomIndex = Random.Range(0, houses.Count);
            targetHouse = houses[randomIndex];
            targetAdress = targetHouse.adress;
        }
    }

    void ResetAdresses()
    {
        pickupHouse = null;
        pickupAdress = "";

        targetHouse = null;
        targetAdress = "";
    }
    #endregion packages

    #region Support
    private void OnEnable()
    {
        GetHouses();

        SpawnPackage();
    }

    void GiveScore()
    {
        if (timer >= 0.8 * maxTime)
        {
            playerScore += 5;
        }
        else if (timer >= 0.6 * maxTime)
        {
            playerScore += 4;
        }
        else if (timer >= 0.4 * maxTime)
        {
            playerScore += 3;
        }
        else if (timer >= 0.2 * maxTime)
        {
            playerScore += 2;
        }
        else
        {
            playerScore += 1;
        }
    }

    void GetHouses()
    {
        houses = new List<House>(FindObjectsOfType<House>());
    }
    #endregion Support
}
