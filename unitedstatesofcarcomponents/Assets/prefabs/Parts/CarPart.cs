using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPart : MonoBehaviour {
    public PartTypes type;
    public Manufacturers manufacturer;

    public Action<Car> UpdateEffect;
	public Sprite sprite;

#pragma warning disable IDE0044 // Add readonly modifier
#pragma warning disable CS0649 // Add readonly modifier Unity
	[SerializeField]
#pragma warning restore IDE0044 // Add readonly modifier
#pragma warning restore CS0649 // Add readonly modifier Unity

	public float maxHealth;

    public float currentHealth;

    public float healthDecay;

    public float maxSpeedMult = 1;

    public bool isEquipped = false;

    public InteractiveSlot slot;

    private void Update() {
        if (!isEquipped) return;
        
        currentHealth -= Time.deltaTime * healthDecay;

        if (currentHealth <= 0) {
            slot.Remove();
            Destroy(gameObject);
        }
    }
}