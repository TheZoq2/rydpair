using System;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {
    public float acceleration;
    public ParticleSystem engineSmoke;
    public CarPart carPartFactory;

    [SerializeField]
    private float defaultMaxFuel;
    public float maxFuel;
    [SerializeField]
    private float defaultFuelDrain;
    public float fuelDrain;
    [SerializeField]
    private float defaultMaxSpeed;
    public float maxSpeed;

    private string destinationAddress = "";
    public int score = 0;

    //Equipped car parts
    public Dictionary<PartTypes, CarPart> equippedParts = new Dictionary<PartTypes, CarPart>();
    private readonly Dictionary<PartTypes, CarPart> defaultParts = new Dictionary<PartTypes, CarPart>();

    private float velocity;
    private float wheelAngle = 0;
    public float wheelDistance;
    public float steeringSpeed;
    public float velocityDecay;

    private float fuel;

    private Inventory inventory;

    private void Start() {
        engineSmoke = gameObject.GetComponent<ParticleSystem>();
        inventory = FindObjectOfType<Inventory>();
        fuel = maxFuel;

        // defaultParts[PartTypes.BRAKES] = carPartFactory.Create(PartTypes.BRAKES, c => {
        //     /* TODO */
        // });
        // defaultParts[PartTypes.ENGINE] = carPartFactory.Create(PartTypes.ENGINE, c => {
        //     c.defaultFuelDrain *= 3.0f;
        //     c.defaultMaxSpeed *= 0.5f;
        //     c.engineSmoke.Play();
        // });
        // defaultParts[PartTypes.EXHAUST_SYSTEM] = carPartFactory.Create(PartTypes.EXHAUST_SYSTEM, c => {
        //     /* TODO */
        // });
        // defaultParts[PartTypes.GEAR_BOX] = carPartFactory.Create(PartTypes.GEAR_BOX, c => {
        //     /* TODO */
        // });
        // defaultParts[PartTypes.STEERING_WHEEL] = carPartFactory.Create(PartTypes.STEERING_WHEEL, c => {
        // });
        // defaultParts[PartTypes.WHEELS] = carPartFactory.Create(PartTypes.WHEELS, c => {
        //     c.defaultMaxSpeed *= 0.9f;
        // });

        // foreach (PartTypes type in Enum.GetValues((typeof(PartTypes)))) {
        //     equippedParts[type] = defaultParts[type];
        // }
    }

    // Update is called once per frame
    void Update() {
        UpdateInput();
        UpdateMovement();
    }

    private void OnTriggerEnter(Collider other) {
        Pickup pickup = other.gameObject.GetComponent<Pickup>();
        House house = other.gameObject.GetComponent<House>();
        
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
    }

    public float maxVelocity;
    public GameObject steeringWheel;
    public float steeringWheelMultiplier;
}
