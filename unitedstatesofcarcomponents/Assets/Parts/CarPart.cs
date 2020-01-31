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

    bool isEquipped = false;

    private void Update() {
        if (!isEquipped) return;
        currentHealth -= (int) (Time.deltaTime * healthDecay);

        if (currentHealth <= 0) {
            // Destroy this part
        }


    }

    public List<PartTrait> traits { get; } = new List<PartTrait>();

    void OnEquip(List<CarPart> otherParts) {
        
    }

    bool IsEnabled(List<CarPart> otherParts) {
        foreach (PartTrait trait in traits) {
            if (trait.DisablesOwnPart() && trait.IsEnabled(traits, otherParts)) {
                return false;
            }

            return true;
        }

        return true;
    }

}