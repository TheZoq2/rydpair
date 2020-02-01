using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CarPart : MonoBehaviour
{
    private void Start() {
        currentHealth = maxHealth = defaultMaxHealth;
    }

    private void OnSetParts() {
        if (false) { // TODO: Figure out whether our parent is the car or the inventory.
            // If parent is the inventory, not the car
            isEquipped = false;
        } else {
            // If parent is the car, not the inventory
            isEquipped = true;
            List<CarPart> otherParts = new List<CarPart>();
            foreach (CarPart part in gameObject.GetComponentsInParent<CarPart>()) {
                if (part == gameObject) otherParts.Add(part);
            }
            
            maxHealth = defaultMaxHealth;
            maxSpeedMult = 1;
            foreach (PartTrait trait in traits) {
                int partHealthMod = 0;
                if (trait.IsEnabled(traits, otherParts)) {
                    // Part's impact, derived from traits
                    partHealthMod += trait.PartHealthImpact();
                    maxSpeedMult *= trait.MaxSpeedMult();
                }
                maxHealth += partHealthMod;
            }
        }
    }

#pragma warning disable IDE0044 // Add readonly modifier
#pragma warning disable CS0649 // Add readonly modifier Unity
	[SerializeField]
    private int defaultMaxHealth;
#pragma warning restore IDE0044 // Add readonly modifier
#pragma warning restore CS0649 // Add readonly modifier Unity

	private int maxHealth;

    public int currentHealth;

    public int healthDecay;

    bool isEquipped = false;

    bool isInvertingSteering = false;

    public float maxSpeedMult = 1;

    private void Update() {
        if (!isEquipped) return;
        currentHealth -= (int) (Time.deltaTime * healthDecay);

        if (currentHealth <= 0) {
            // Destroy this part
        }
    }

    public List<PartTrait> traits { get; } = new List<PartTrait>();

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