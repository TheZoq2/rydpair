﻿using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {
    public float acceleration;
    public ParticleSystem engineSmoke;
    private CarPart carPartPrefab;
    public GameObject carPartPrefabContainer;

    [SerializeField]
    private float defaultMaxFuel;
    public float maxFuel;
    [SerializeField]
    private float defaultFuelDrain;
    public float fuelDrain;
    [SerializeField]
    private float defaultMaxSpeed;
    public float maxSpeed;

    //Equipped car parts
    public Dictionary<PartTypes, CarPart> equippedParts;
    private readonly Dictionary<PartTypes, CarPart> defaultParts = new Dictionary<PartTypes, CarPart>();

    private float velocity;
    private float wheelAngle = 0;
    public float wheelDistance;
    public float steeringSpeed;

    private float fuel;

    private Inventory inventory;

    private void Start() {
        engineSmoke = gameObject.GetComponent<ParticleSystem>();
        inventory = FindObjectOfType<Inventory>();
        fuel = maxFuel;

        carPartPrefab = carPartPrefabContainer.GetComponent<CarPart>();

        defaultParts[PartTypes.BRAKES] = carPartPrefab.Create(PartTypes.BRAKES, c => {
            /* TODO */
        });
        defaultParts[PartTypes.ENGINE] = carPartPrefab.Create(PartTypes.ENGINE, c => {
            c.defaultFuelDrain *= 3.0f;
            c.defaultMaxSpeed *= 0.5f;
        });
        defaultParts[PartTypes.EXHAUST_SYSTEM] = carPartPrefab.Create(PartTypes.EXHAUST_SYSTEM, c => {
            /* TODO */
        });
        defaultParts[PartTypes.GEAR_BOX] = carPartPrefab.Create(PartTypes.GEAR_BOX, c => {
            /* TODO */
        });
        defaultParts[PartTypes.STEERING_WHEEL] = carPartPrefab.Create(PartTypes.STEERING_WHEEL, c => {
        });
        defaultParts[PartTypes.WHEELS] = carPartPrefab.Create(PartTypes.WHEELS, c => {
            c.defaultMaxSpeed *= 0.9f;
        });
    }

    // Update is called once per frame
    void Update() {
        UpdateInput();
        UpdateMovement();
    }

    private void OnTriggerEnter(Collider other) {
        Pickup pickup = other.gameObject.GetComponent<Pickup>();
        if (pickup != null) {
            if (pickup.part != null) {
                //TODO: Fix inventory integration when inventory feels like integrating.
                //inventory
            }
        }

        Destroy(other.gameObject);
    }

    public CarPart RemovePart(PartTypes type) {
        CarPart removedPart = equippedParts[type];
        equippedParts[type] = defaultParts[type]; // TODO: Not null?
        return removedPart;
    }


    public void AddPart(PartTypes type, CarPart newPart) {
        if (newPart.type != type) Debug.Log($"Equipped {newPart.type}-slot part in {type} slot (probably shouldn't happen).");
        equippedParts[type] = newPart;
        UpdateComponentEffects();
    }

    public void UpdateComponentEffects() {
        ResetStats();
        foreach (CarPart part in equippedParts.Values) {
            part.OnSetParts(this);
        }
    }

    private void ResetStats() {
        maxFuel = defaultMaxFuel;
        fuelDrain = defaultFuelDrain;
        maxSpeed = defaultMaxSpeed;
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
    }

    private void UpdateInput() {
        velocity += acceleration * Input.GetAxis("Accelerate") * Time.deltaTime;
        velocity -= acceleration * Input.GetAxis("Reverse") * Time.deltaTime;
    }

}
