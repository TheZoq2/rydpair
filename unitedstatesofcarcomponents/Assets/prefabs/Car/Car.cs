using System;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {
	[HideInInspector]
	public ParticleSystem engineSmoke;
    private CarPartFactory carPartFactory;
    public GameObject steeringWheel;
    public GameObject spedometerNeedle;

    private bool flippedSteering = false;

	[HideInInspector]
	public float steeringWheelMultiplier;
	[HideInInspector]
	public float acceleration;
	[HideInInspector]
	public float maxFuel;
	[HideInInspector]
	public float fuelDrain;
	[HideInInspector]
	public float maxVelocity;
	[HideInInspector]
	public float velocityDecay;
	[HideInInspector]
	public float wheelDistance;
	[HideInInspector]
	public float steeringSpeed;
	[HideInInspector]
	public float breakFactor;

	public float defaultSteeringWheelMultiplier;
	public float defaultAcceleration;
	public float defaultMaxFuel;
	public float defaultFuelDrain;
	public float defaultMaxVelocity;
	public float defaultVelocityDecay;
	public float defaultWheelDistance;
	public float defaultSteeringSpeed;
	public float defaultBreakFactor;

	private string destinationAddress = "";
	[HideInInspector]
	public int score = 0;
	private float deltaVelocity = 0.0f;

    //Equipped car parts
    public Dictionary<PartTypes, CarPart> equippedParts = new Dictionary<PartTypes, CarPart>();

    private float velocity;
    private float wheelAngle = 0;

    private float fuel;

    private GameState gState;

    public void flipSteering(){
        this.flippedSteering = true;
    }

    private void Awake() {
        engineSmoke = gameObject.GetComponent<ParticleSystem>();

        gState = FindObjectOfType<GameState>();
        engineSmoke.Play();
    }

    // Update is called once per frame
    void Update() {
		deltaVelocity = 0.0f;
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

    public CarPart RemovePart(PartTypes type)
	{
		CarPart removedPart = equippedParts[type];
        equippedParts[type] = null;
		UpdateComponentEffects();
        removedPart.isEquipped = false;
		return removedPart;
    }


    public void AddPart(PartTypes type, CarPart newPart) {
        if (newPart.type != type) Debug.Log($"Equipped {newPart.type}-slot part in {type} slot (probably shouldn't happen).");
        equippedParts[type] = newPart;
        newPart.isEquipped = true;
        UpdateComponentEffects();
    }

    public void UpdateComponentEffects() {
        ResetStats();
		// Missing parts effects
		foreach (PartTypes type in equippedParts.Keys)
		{
			if (equippedParts[type] == null)
			{
				switch (type)
				{
					case PartTypes.BRAKES:
						breakFactor *= 0.0f;
						break;
					case PartTypes.ENGINE:
						acceleration *= 0.1f;
						maxVelocity *= 0.1f;
						break;
					case PartTypes.EXHAUST_SYSTEM:
						engineSmoke.Play();
						break;
					case PartTypes.GEAR_BOX:
						acceleration *= 0.1f;
						maxVelocity *= 0.1f;
						break;
					case PartTypes.STEERING_WHEEL:
						steeringSpeed *= 0.1f;
						break;
					case PartTypes.WHEELS:
						acceleration *= 0.5f;
						maxVelocity *= 0.5f;
						break;
				}
			}
		}
		// Part-specific effects
		foreach (CarPart part in equippedParts.Values) {
            part?.UpdateEffect(this);
        }
    }

    private void ResetStats() {
		steeringWheelMultiplier = defaultSteeringWheelMultiplier;
		acceleration = defaultAcceleration;
		maxFuel = defaultMaxFuel;
		fuelDrain = defaultFuelDrain;
		maxVelocity = defaultMaxVelocity;
		velocityDecay = defaultVelocityDecay;
		wheelDistance = defaultWheelDistance;
		steeringSpeed = defaultSteeringSpeed;
		breakFactor = defaultBreakFactor;

        flippedSteering = false;

	// Clear smoke colour
	ParticleSystem.Particle[] particleArray = new ParticleSystem.Particle[1];
        if (engineSmoke.GetParticles(particleArray) > 0) {
            particleArray[0].startColor = new Color(0, 0, 0);
        }
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

		deltaVelocity -= velocity * (1.0f - velocityDecay);
		float currentBreakFactor = deltaVelocity * velocity < 0.0f ? breakFactor : 1.0f;
		velocity += deltaVelocity * currentBreakFactor;

		Vector3 currentVelocity = new Vector3(
                velocity * Mathf.Sin(transform.rotation.eulerAngles.y * Mathf.PI / 180),
                0.0f,
                velocity * Mathf.Cos(transform.rotation.eulerAngles.y * Mathf.PI / 180)
            );

        transform.position += currentVelocity * Time.deltaTime;

        float turnFactor = 1f;
        if(this.flippedSteering){
            turnFactor = -1f;
        }

        transform.RotateAround(
            transform.position,
            transform.up,
            (velocity * Mathf.Tan(wheelAngleRad) / wheelDistance) * 180 / Mathf.PI
                * Time.deltaTime * turnFactor
        );

        steeringWheel.transform.localRotation = Quaternion.Euler(
            0, wheelAngle * steeringWheelMultiplier, 0
        );
        spedometerNeedle.transform.localRotation = Quaternion.Euler(
            0, 0, - Mathf.Abs(velocity / maxVelocity) * 160 + 80
        );
    }

    private void UpdateInput()
	{
		deltaVelocity += acceleration * Input.GetAxis("Accelerate") * Time.deltaTime;
		deltaVelocity -= acceleration * Input.GetAxis("Reverse") * Time.deltaTime;

        if (velocity > maxVelocity) {
            velocity = maxVelocity;
        }
        else if(velocity < -maxVelocity) {
            velocity = -maxVelocity;
        }
    }
}
