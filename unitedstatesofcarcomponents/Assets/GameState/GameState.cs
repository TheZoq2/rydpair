﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameState : MonoBehaviour
{
    #region Declarations
    [Header("Package")]
    public GameObject packagePrefab;

    [Header("User Interface")]
    public TextMeshProUGUI missionTMP;
    public TextMeshProUGUI scoreTMP;
    public TextMeshProUGUI timerTMP;

    [HideInInspector]
    public List<House> houses;

    [HideInInspector] // The package should be delivered to this adress
    public House targetHouse;
    [HideInInspector]
    public string targetAdress;

    [HideInInspector] // The package spawns at this adress
    public House pickupHouse;
    [HideInInspector]
    public string pickupAdress;

    [HideInInspector]
    public int playerScore;
    [HideInInspector]
    public float maxTime = 300f;
    [HideInInspector]
    public float timer;

    int randomIndex;
    bool hasPackage;
    #endregion Declarations

    private void FixedUpdate()
    {
        if (hasPackage)
        {
            timer -= Time.deltaTime;
            timerTMP.text = $"Time left = {timer}";
        }
    }

    #region packages
    public void PickUpPackage()
    {
        hasPackage = true;
        timer = maxTime;

        missionTMP.text = $"Deliver the package to {targetAdress}!";
    }

    public void DeliverPackage()
    {
        hasPackage = false;

        GiveScore();

        SpawnPackage();

        timerTMP.text = "";
    }

    public void SpawnPackage()
    {
        ResetAdresses();

        AssignPickupAdress();
        Instantiate(packagePrefab, pickupHouse.pickupPoint);

        AssignTargetAdress();

        missionTMP.text = $"Pick up package at {pickupAdress}!";
    }

    void AssignPickupAdress()
    {
        randomIndex = Random.Range(0, houses.Count);
        pickupHouse = houses[randomIndex];
        pickupAdress = pickupHouse.adress;
    }

    void AssignTargetAdress()
    {
        while(targetHouse == null || targetHouse == pickupHouse)
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

        scoreTMP.text = $"Score = {playerScore}";
    }

    void GetHouses()
    {
        houses = new List<House>(FindObjectsOfType<House>());
    }
    #endregion Support
}
