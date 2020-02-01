using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPart : MonoBehaviour {
    public PartTypes type;
    public Manufacturers manufacturer;

    private void Start() {
        currentHealth = maxHealth = defaultMaxHealth;
    }

    public Action<Car> UpdateEffect;

#pragma warning disable IDE0044 // Add readonly modifier
#pragma warning disable CS0649 // Add readonly modifier Unity
	[SerializeField]
    private int defaultMaxHealth;
#pragma warning restore IDE0044 // Add readonly modifier
#pragma warning restore CS0649 // Add readonly modifier Unity

	private int maxHealth;

    public int currentHealth;

    public int healthDecay;

    bool isInvertingSteering = false;

    public float maxSpeedMult = 1;

    private void Update() {
        // if (!isEquipped) return; // Is knowing this the component's job, or the car's?
        currentHealth -= (int) (Time.deltaTime * healthDecay);

        if (currentHealth <= 0) {
            // Destroy this part
        }
    }
}