using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {
    public float acceleration;
    public float velocityDecay;
    public float minimumSpeed;
    public float rotationSpeed;
    public float turnMultiplier;
    public ParticleSystem particleSystem;

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

    private Dictionary<PartTypes, CarPart> defaultParts = new Dictionary<PartTypes, CarPart>();

    private Vector3 velocity;

    private float fuel;

    private Inventory inventory;

    private void Start() {
        particleSystem = gameObject.GetComponent<ParticleSystem>();
        inventory = FindObjectOfType<Inventory>();
        fuel = maxFuel;

        defaultParts[PartTypes.BRAKES] = new CarPart(PartTypes.BRAKES, c => {
            /* TODO */
        });
        defaultParts[PartTypes.ENGINE] = new CarPart(PartTypes.ENGINE, c => {
            c.defaultFuelDrain *= 3.0f;
            c.defaultMaxSpeed *= 0.5f;
        });
        defaultParts[PartTypes.EXHAUST_SYSTEM] = new CarPart(PartTypes.EXHAUST_SYSTEM, c => {
            /* TODO */
        });
        defaultParts[PartTypes.GEAR_BOX] = new CarPart(PartTypes.GEAR_BOX, c => {
            /* TODO */
        });
        defaultParts[PartTypes.STEERING_WHEEL] = new CarPart(PartTypes.STEERING_WHEEL, c => {
            c.turnMultiplier *= -1.0f;
        });
        defaultParts[PartTypes.WHEELS] = new CarPart(PartTypes.WHEELS, c => {
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
        if (fuel > 0) {
            if (velocity.magnitude > maxSpeed) {
                velocity = velocity.normalized * maxSpeed;
            }
            transform.position += velocity * Time.deltaTime;

            velocity *= velocityDecay;
            if (Input.GetAxis("Accelerate") == 0 && Input.GetAxis("Reverse") == 0 && velocity.magnitude < minimumSpeed) {
                velocity = Vector3.zero;
            }

            fuel -= fuelDrain;
        }
    }

    private void UpdateInput() {
        velocity += transform.forward * acceleration * Input.GetAxis("Accelerate") * Time.deltaTime;
        velocity -= transform.forward * acceleration * Input.GetAxis("Reverse") * Time.deltaTime;

        transform.Rotate(0, Input.GetAxis("Horizontal") * rotationSpeed * (Vector3.Project(velocity, transform.forward).magnitude * turnMultiplier) * Time.deltaTime, 0);
    }
}
