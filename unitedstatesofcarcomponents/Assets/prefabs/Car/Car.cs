﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {
    public ParticleSystem engineSmoke;
    private CarPartFactory carPartFactory;
    public GameObject steeringWheel;
    public GameObject camera;
    public float steeringWheelMultiplier;

    public float defaultAcceleration;
    public float acceleration;
    public float defaultMaxFuel;
    public float maxFuel;
    public float defaultFuelDrain;
    public float fuelDrain;
    public float defaultMaxVelocity;
    public float maxVelocity;
    public float defaultVelocityDecay;
    public float velocityDecay;

    public float wheelDistance;
    public float steeringSpeed;

    private string destinationAddress = "";
    public int score = 0;

    //Equipped car parts
    public Dictionary<PartTypes, CarPart> equippedParts = new Dictionary<PartTypes, CarPart>();

    private float velocity;
    private float wheelAngle = 0;

    private float fuel;

    private Inventory inventory;
    private GameState gState;

    private void Start() {
        engineSmoke = gameObject.GetComponent<ParticleSystem>();
        inventory = FindObjectOfType<Inventory>();

		carPartFactory = FindObjectOfType<CarPartFactory>();
		Manufacturers[] allManufacturers = (Manufacturers[]) Enum.GetValues(typeof(Manufacturers));
        foreach (PartTypes slot in Enum.GetValues(typeof(PartTypes))) {
            Manufacturers randomManufacturer = allManufacturers[(int)UnityEngine.Random.Range(0, allManufacturers.Length - 1)];
            CarPart randomPart = carPartFactory.Create(slot, randomManufacturer);
            equippedParts[slot] = randomPart;
        }
    }

    // Update is called once per frame
    void Update() {
        UpdateInput();
        UpdateMovement();
    }

    private void OnTriggerEnter(Collider other) {
        Pickup pickup = other.gameObject.GetComponent<Pickup>();
        House house = other.gameObject.GetComponentInParent<House>();

        if(other.tag == "Delivery")
        {
            if (gState.targetAdress != "")
            {
                if(house.adress == gState.targetAdress)
                {
                    // Sucessfull delivery!
                }
            }
        }
        else
        {

        }
        
        if (pickup != null) {
            if (string.IsNullOrEmpty(destinationAddress)) {
                // Successful pickup
                destinationAddress = pickup.destination;
                Destroy(other.gameObject);
            }
        } else if (!string.IsNullOrEmpty(destinationAddress) && destinationAddress == house?.adress) {
            // A successful delivery
            score++;
            destinationAddress = "";
        }

        if(other.tag != "Delivery")
        {
            if (pickup != null)
            {
                //if (pickup.part != null && inventory.TryAddItem(pickup.part) {
                //}
            }

            Destroy(other.gameObject);
        }
    }

    public CarPart RemovePart(PartTypes type) {
        CarPart removedPart = equippedParts[type];
        equippedParts[type] = null;
		Debug.Log("Remove " + type);
		return removedPart;
    }


    public void AddPart(PartTypes type, CarPart newPart) {
        if (newPart?.type != type) Debug.Log($"Equipped {newPart?.type}-slot part in {type} slot (probably shouldn't happen).");
        equippedParts[type] = newPart;
        UpdateComponentEffects();
		Debug.Log("Add " + type);
	}

    public void UpdateComponentEffects() {
        ResetStats();
        foreach (PartTypes slot in equippedParts.Keys) {
            if (equippedParts[slot] == null) {
                switch (slot) {
                    case PartTypes.BRAKES:
                        maxVelocity /= 4;
                        break;
                    case PartTypes.ENGINE:
                        acceleration /= 2;
                        maxVelocity /= 2;
                        break;
                    case PartTypes.EXHAUST_SYSTEM:
                        engineSmoke.Play();
                        fuelDrain *= 2;
                        break;
                    case PartTypes.GEAR_BOX:
                        acceleration /= 4;
                        break;
                    case PartTypes.STEERING_WHEEL:
                        steeringSpeed /= 2;
                        break;
                    case PartTypes.WHEELS:
                        velocityDecay = 1;
                        break;
                    default:
                        Debug.Log($"No penanty implemented for null part in slot {slot}");
                        break;
                }
                Debug.Log($"Null part equipped in slot {slot}");
            } else {
                equippedParts[slot].UpdateEffect(this);
            }
        }
    }

    private void ResetStats() {
        maxFuel = defaultMaxFuel;
        fuelDrain = defaultFuelDrain;
        maxVelocity = defaultMaxVelocity;
        engineSmoke.Stop();
    }

    private void UpdateMovement() {
        wheelAngle -= (wheelAngle - Input.GetAxis("Horizontal") * steeringSpeed) * Time.deltaTime;
        if(wheelAngle > 1) {
            wheelAngle = 1;
        }
        else if (wheelAngle < -1) {
            wheelAngle = -1;
        }
        float wheelAngleRad = wheelAngle * Mathf.PI / 4.0F;

        Vector3 currentVelocity = new Vector3(
                velocity * Mathf.Sin(transform.rotation.eulerAngles.y * Mathf.PI / 180),
                0.0f,
                velocity * Mathf.Cos(transform.rotation.eulerAngles.y * Mathf.PI / 180)
            );

        transform.position += currentVelocity * Time.deltaTime;

        transform.RotateAround(
            transform.position,
            transform.up,
            (velocity * Mathf.Tan(wheelAngleRad) / wheelDistance) * 180 / Mathf.PI
                * Time.deltaTime
        );

        velocity *= velocityDecay;

        steeringWheel.transform.localRotation = Quaternion.Euler(
            0, wheelAngle * steeringWheelMultiplier, 0
        );
    }

    private void UpdateInput() {
        velocity += acceleration * Input.GetAxis("Accelerate") * Time.deltaTime;
        velocity -= acceleration * Input.GetAxis("Reverse") * Time.deltaTime;

        if(velocity > maxVelocity) {
            velocity = maxVelocity;
        }
        else if(velocity < -maxVelocity) {
            velocity = -maxVelocity;
        }

        float lookAngleMax = 80;

        float cameraRX = Input.GetAxis("RotationX") * lookAngleMax;
        float cameraRY = Input.GetAxis("RotationY") * lookAngleMax;

        camera.transform.localRotation = Quaternion.Euler(
            cameraRX, cameraRY, 0
        );

    }
}
