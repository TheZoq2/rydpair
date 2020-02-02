using System;
using System.Collections.Generic;
using UnityEngine;

public class CarPartFactory : MonoBehaviour {

	public CarPart carPartPrefab;

	public Sprite imageBrakesNiiSan;
	public Sprite imageBrakesSM;
	public Sprite imageBrakesVolvimus;
	public Sprite imageEngineNiiSan;
	public Sprite imageEngineSM;
	public Sprite imageEngineVolvimus;
	public Sprite imageExhaustNiiSan;
	public Sprite imageExhaustSM;
	public Sprite imageExhaustVolvimus;
	public Sprite imageGearNiiSan;
	public Sprite imageGearSM;
	public Sprite imageGearVolvimus;
	public Sprite imageSteeringNiiSan;
	public Sprite imageSteeringSM;
	public Sprite imageSteeringVolvimus;
	public Sprite imageWheelsNiiSan;
	public Sprite imageWheelsSM;
	public Sprite imageWheelsVolvimus;

	Dictionary<PartTypes, Dictionary<Manufacturers, Action<Car>>> callbacks = new Dictionary<PartTypes, Dictionary<Manufacturers, Action<Car>>>();
	public Dictionary<PartTypes, Dictionary<Manufacturers, Sprite>> sprites = new Dictionary<PartTypes, Dictionary<Manufacturers, Sprite>>();

	private bool TryGetEquipped(Car c, PartTypes type, out CarPart part)
	{
		if (c.equippedParts.TryGetValue(type, out part))
		{
			return part != null;
		}
		return false;
	}

	private void Awake()
	{
		foreach (PartTypes partType in Enum.GetValues(typeof(PartTypes)))
		{
			callbacks[partType] = new Dictionary<Manufacturers, Action<Car>>();
			sprites[partType] = new Dictionary<Manufacturers, Sprite>();
		}
		sprites[PartTypes.ENGINE][Manufacturers.NII_SAN] = imageEngineNiiSan;
		callbacks[PartTypes.ENGINE][Manufacturers.NII_SAN] =
			c => {
				if (TryGetEquipped(c, PartTypes.GEAR_BOX, out CarPart gear) && gear.manufacturer != Manufacturers.NII_SAN)
				{
					c.fuelDrain *= 10;
				}

				if (TryGetEquipped(c, PartTypes.STEERING_WHEEL, out CarPart steering) && steering.manufacturer != Manufacturers.NII_SAN)
				{
					c.acceleration /= 3;
				}
			};
		sprites[PartTypes.ENGINE][Manufacturers.VOLVIMUS] = imageEngineVolvimus;
		callbacks[PartTypes.ENGINE][Manufacturers.VOLVIMUS] =
			c => {
				if (TryGetEquipped(c, PartTypes.EXHAUST_SYSTEM, out CarPart exhaust) && exhaust.manufacturer == Manufacturers.SM)
				{
					c.engineSmoke.Play();
				}
			};
		sprites[PartTypes.ENGINE][Manufacturers.SM] = imageEngineSM;
		callbacks[PartTypes.ENGINE][Manufacturers.SM] =
			c => {
                if (TryGetEquipped(c, PartTypes.GEAR_BOX, out CarPart gear) && gear.currentHealth <= gear.maxHealth / 2) {
                    c.acceleration *= -1;
                }
            };
		sprites[PartTypes.WHEELS][Manufacturers.NII_SAN] = imageWheelsNiiSan;
		callbacks[PartTypes.WHEELS][Manufacturers.NII_SAN] =
			c => {
                if (TryGetEquipped(c, PartTypes.ENGINE, out CarPart engine) && engine.currentHealth <= engine.maxHealth * 0.3) {
                    c.acceleration *= 0.3f;
                }
            };
		sprites[PartTypes.WHEELS][Manufacturers.VOLVIMUS] = imageWheelsVolvimus;
		callbacks[PartTypes.WHEELS][Manufacturers.VOLVIMUS] =
			c => {
                //if() {
                //TODO: Implement weight?
                //}
            };
		sprites[PartTypes.WHEELS][Manufacturers.SM] = imageWheelsSM;
		callbacks[PartTypes.WHEELS][Manufacturers.SM] =
			c => {
                if (TryGetEquipped(c, PartTypes.BRAKES, out CarPart brakes) && brakes.manufacturer == Manufacturers.SM) {
                    c.velocityDecay = 1;
                }
            };
		sprites[PartTypes.GEAR_BOX][Manufacturers.NII_SAN] = imageGearNiiSan;
		callbacks[PartTypes.GEAR_BOX][Manufacturers.NII_SAN] =
			c => {
                if (TryGetEquipped(c, PartTypes.STEERING_WHEEL, out CarPart steering) && steering.manufacturer == Manufacturers.NII_SAN) {
                    c.maxVelocity /= 2;
                }
            };
		sprites[PartTypes.GEAR_BOX][Manufacturers.VOLVIMUS] = imageGearVolvimus;
		callbacks[PartTypes.GEAR_BOX][Manufacturers.VOLVIMUS] =
			c => {
                if (TryGetEquipped(c, PartTypes.WHEELS, out CarPart wheels) && wheels.manufacturer == Manufacturers.NII_SAN) {
                    //TODO: implement sinus steering.
                }
            };
		sprites[PartTypes.GEAR_BOX][Manufacturers.SM] = imageGearSM;
		callbacks[PartTypes.GEAR_BOX][Manufacturers.SM] =
			c => {
                if (TryGetEquipped(c, PartTypes.BRAKES, out CarPart brakes) && brakes.manufacturer == Manufacturers.VOLVIMUS) {
                    //TODO: Implement random braking.
                }
            };
		sprites[PartTypes.EXHAUST_SYSTEM][Manufacturers.NII_SAN] = imageExhaustNiiSan;
		callbacks[PartTypes.EXHAUST_SYSTEM][Manufacturers.NII_SAN] =
			c => {
				if (TryGetEquipped(c, PartTypes.WHEELS, out CarPart wheels) && wheels.manufacturer == Manufacturers.VOLVIMUS) {
					//Pink smoke or sumthing (combination of exhaust + particles from tires)
					// TODO: This should work, but I'm getting to particles *at all*, so ¯\_(ツ)_/¯
					Debug.Log("Pinkifying smoke o(≧▽≦)o");
                    ParticleSystem.Particle[] particleArray = new ParticleSystem.Particle[1];
                    c.engineSmoke.GetParticles(particleArray);
                    particleArray[0].startColor = new Color(1.0f, 0.6f, 0.3f);
                    c.engineSmoke.Play();
                }
            };
		sprites[PartTypes.EXHAUST_SYSTEM][Manufacturers.VOLVIMUS] = imageExhaustVolvimus;
		callbacks[PartTypes.EXHAUST_SYSTEM][Manufacturers.VOLVIMUS] =
			c => {
				if (TryGetEquipped(c, PartTypes.STEERING_WHEEL, out CarPart steering) && steering.manufacturer == Manufacturers.SM)
				{
					//randomly accelerate, not listen to accelerate
				}
            };
		sprites[PartTypes.EXHAUST_SYSTEM][Manufacturers.SM] = imageExhaustSM;
		callbacks[PartTypes.EXHAUST_SYSTEM][Manufacturers.SM] =
			c => {
                if (TryGetEquipped(c, PartTypes.WHEELS, out CarPart wheels) && wheels.manufacturer == Manufacturers.SM) {
                    //change turning
                }
            };
		sprites[PartTypes.BRAKES][Manufacturers.NII_SAN] = imageBrakesNiiSan;
		callbacks[PartTypes.BRAKES][Manufacturers.NII_SAN] =
			c => {
                if (TryGetEquipped(c, PartTypes.EXHAUST_SYSTEM, out CarPart exhaust) && exhaust.manufacturer == Manufacturers.VOLVIMUS) {
                    //TODO
                }
            };
		sprites[PartTypes.BRAKES][Manufacturers.VOLVIMUS] = imageBrakesVolvimus;
		callbacks[PartTypes.BRAKES][Manufacturers.VOLVIMUS] =
			c => {
                if (TryGetEquipped(c, PartTypes.EXHAUST_SYSTEM, out CarPart exhaust) && exhaust.currentHealth <= exhaust.maxHealth * 0.3) {
                    //TODO
                }
            };
		sprites[PartTypes.BRAKES][Manufacturers.SM] = imageBrakesSM;
		callbacks[PartTypes.BRAKES][Manufacturers.SM] =
			c => {
                if (TryGetEquipped(c, PartTypes.WHEELS, out CarPart wheels) && wheels.currentHealth >= wheels.maxHealth * 0.8) {
					//TODO
				}
            };
		sprites[PartTypes.STEERING_WHEEL][Manufacturers.NII_SAN] = imageSteeringNiiSan;
		callbacks[PartTypes.STEERING_WHEEL][Manufacturers.NII_SAN] =
			c => {
                if (TryGetEquipped(c, PartTypes.GEAR_BOX, out CarPart gear) && gear.manufacturer == Manufacturers.NII_SAN) {
                    c.acceleration *= 100;
                }
            };
		sprites[PartTypes.STEERING_WHEEL][Manufacturers.VOLVIMUS] = imageSteeringVolvimus;
		callbacks[PartTypes.STEERING_WHEEL][Manufacturers.VOLVIMUS] =
			c => {
                if (TryGetEquipped(c, PartTypes.BRAKES, out CarPart brakes) && brakes.manufacturer == Manufacturers.NII_SAN) {
                    //TODO
                }
            };
		sprites[PartTypes.STEERING_WHEEL][Manufacturers.SM] = imageSteeringSM;
		callbacks[PartTypes.STEERING_WHEEL][Manufacturers.SM] =
			c => {
                if (TryGetEquipped(c, PartTypes.ENGINE, out CarPart engine) && engine.manufacturer == Manufacturers.VOLVIMUS) {
                    //TODO
                }
            };
	}

	public CarPart Create(PartTypes type, Manufacturers manufacturer, Sprite sprite, Action<Car> setPartsDelegate) {
        CarPart newPart = Instantiate(carPartPrefab);

        newPart.type = type;
        newPart.manufacturer = manufacturer;
		newPart.sprite = sprite;
		newPart.UpdateEffect = setPartsDelegate ?? (_ => throw new System.NotImplementedException($"The part equipped in slot {type} is not implemented"));

        //TODO: Set other stuff

        return newPart;
    }

    public CarPart Create(PartTypes type, Manufacturers manufacturer) {
        return Create(type, manufacturer, sprites[type][manufacturer], callbacks[type][manufacturer]);
    }
}
