using UnityEngine;

public class Car : MonoBehaviour {
    public float acceleration;
    public float velocityDecay;
    public float minimumSpeed;
    public float rotationSpeed;
    public float turnMultiplier;

    public float maxFuel;
    public float fuelDrain;

    //Equipped car parts
    public CarPart engine;
    public CarPart wheels;
    public CarPart gearBox;
    public CarPart steeringWheel;
    public CarPart brakes;
    public CarPart exhaustSystem;

    private Vector3 velocity;
    
    private float fuel;

    private void Start() {
        fuel = maxFuel;
    }

    // Update is called once per frame
    void Update() {
        UpdateInput();
        UpdateMovement();
    }

    public void RemovePart(PartTypes type) {
        switch (type) {
            //TODO: Instead of null, have default broken part?
            case PartTypes.ENGINE:
                engine = null;
                    break;
            case PartTypes.WHEELS:
                wheels = null;
                break;
            case PartTypes.GEAR_BOX:
                wheels = null;
                break;
            case PartTypes.STEERING_WHEEL:
                steeringWheel = null;
                break;
            case PartTypes.BRAKES:
                brakes = null;
                break;
            case PartTypes.EXHAUST_SYSTEM:
                exhaustSystem = null;
                break;
            default:
                break;
        }
        UpdateComponentEffects();
    }

    public void AddPart(PartTypes type, CarPart newPart) {
        switch (type) {
            case PartTypes.ENGINE:
                engine = newPart;
                break;
            case PartTypes.WHEELS:
                wheels = newPart;
                break;
            case PartTypes.GEAR_BOX:
                wheels = newPart;
                break;
            case PartTypes.STEERING_WHEEL:
                steeringWheel = newPart;
                break;
            case PartTypes.BRAKES:
                brakes = newPart;
                break;
            case PartTypes.EXHAUST_SYSTEM:
                exhaustSystem = newPart;
                break;
            default:
                break;
        }
        UpdateComponentEffects();
    }

    public void UpdateComponentEffects() {
        //TODO: Implement CarPart method for affecting car
        //engine.
        //wheels.
        //gearbox.
        //steeringWheel.
        //brakes.
        //exhaustSystem.
    }

    private void UpdateMovement() {
        if(fuel > 0) {
            transform.position += velocity * Time.deltaTime;

            velocity *= velocityDecay;
            if(Input.GetAxis("Accelerate") != 0 && Input.GetAxis("Reverse") != 0 && velocity.magnitude < minimumSpeed) {
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
