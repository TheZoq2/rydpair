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
    
    int randomIndex;
    #endregion Declarations

    #region packages
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

    void GetHouses()
    {
        houses = new List<House>(FindObjectsOfType<House>());
    }
    #endregion Support
}
