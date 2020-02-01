using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPart : MonoBehaviour
{

    public CarPart(PartTypes type, Action<Car> setPartsDelegate) {
        this.type = type;
        this.onSetPartsDelegate = setPartsDelegate ?? (_ => throw new System.NotImplementedException($"The part equipped in slot {type} is not implemented"));
    }

    public PartTypes type;

    private void Start() {
        currentHealth = maxHealth = defaultMaxHealth;
    }

    private Action<Car> onSetPartsDelegate;
    public void OnSetParts(Car car) {
        onSetPartsDelegate(car);
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

    bool isInvertingSteering = false;

    public float maxSpeedMult = 1;

    private void Update() {
        // if (!isEquipped) return; // Is knowing this the component's job, or the car's?
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