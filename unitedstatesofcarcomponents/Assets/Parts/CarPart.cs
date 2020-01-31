using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CarPart : MonoBehaviour
{
    private void Start() {
        currentHealth = maxHealth;
    }

    private int maxHealth;

    public int currentHealth;

    public int healthDecay;

    private void Update() {
        currentHealth -= (int) (Time.deltaTime * healthDecay);

        if (currentHealth <= 0) {
            // Destroy this part
        }


    }

    void OnEquip() {
        // Default implementation
    }

}