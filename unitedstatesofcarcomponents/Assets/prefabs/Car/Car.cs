using System;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {
    public ParticleSystem engineSmoke;
    public CarPartFactory carPartFactory;
    public GameObject steeringWheel;
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

        Manufacturers[] allManufacturers = (Manufacturers[])Enum.GetValues(typeof(Manufacturers));
        foreach (PartTypes slot in Enum.GetValues(typeof(PartTypes))) {
            Manufacturers randomManufacturer = allManufacturers[UnityEngine.Random.Range(0, allManufacturers.Length - 1)];
            equippedParts[slot] = carPartFactory.Create(slot, randomManufacturer);
        }
        gState = FindObjectOfType<GameState>();
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
                    gState.DeliverPackage();
                }
            }
        }
        else if(pickup != null)
        {
            gState.PickUpPackage();
            Destroy(other.gameObject);
        }
    }

    public CarPart RemovePart(PartTypes type) {
        CarPart removedPart = equippedParts[type];
        equippedParts[type] = null;
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
            part.UpdateEffect(this);
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

       // Debug.Log(velocity);

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

        if (velocity > maxVelocity) {
            velocity = maxVelocity;
        }
        else if(velocity < -maxVelocity) {
            velocity = -maxVelocity;
        }
    }
}
