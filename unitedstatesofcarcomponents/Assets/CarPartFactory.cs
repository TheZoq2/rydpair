using System;
using System.Collections.Generic;
using UnityEngine;

public class CarPartFactory : MonoBehaviour {
    public CarPart carPartPrefab;

	public Sprite imageNiiSanEngine;
	public Sprite imageVolvimusEngine;
	public Sprite imageSMEngine;
	public Sprite imageNiiSanWheel;
	public Sprite imageVolvimusWheels;
	public Sprite imageSMWheels;
	public Sprite imageNiiSanGearBox;
	public Sprite imageVolvimusGearBox;
	public Sprite imageSMGearBox;
	public Sprite imageNiiSanExhaust;
	public Sprite imageVolvimusExhaust;
	public Sprite imageSMExhaust;
	public Sprite imageNiiSanBrakes;
	public Sprite imageVolvimusBrakes;
	public Sprite imageSMBrakes;
	public Sprite imageNiiSanSteeringWheel;
	public Sprite imageVolvimusSteeringWheel;
	public Sprite imageSMSteeringWheel;

	Dictionary<PartTypes, Dictionary<Manufacturers, Action<Car>>> callbacks = new Dictionary<PartTypes, Dictionary<Manufacturers, Action<Car>>>();
	Dictionary<PartTypes, Dictionary<Manufacturers, Sprite>> sprites = new Dictionary<PartTypes, Dictionary<Manufacturers, Sprite>>();

	private void Awake()
	{
		foreach (PartTypes partType in Enum.GetValues(typeof(PartTypes)))
		{
			callbacks[partType] = new Dictionary<Manufacturers, Action<Car>>();
			sprites[partType] = new Dictionary<Manufacturers, Sprite>();
		}
		sprites[PartTypes.ENGINE][Manufacturers.NII_SAN] = imageNiiSanEngine;
		callbacks[PartTypes.ENGINE][Manufacturers.NII_SAN] =
			c => {
				if (c.equippedParts[PartTypes.GEAR_BOX] != null && c.equippedParts[PartTypes.GEAR_BOX].manufacturer != Manufacturers.NII_SAN)
				{
					c.fuelDrain *= 10;
				}

				if (c.equippedParts[PartTypes.STEERING_WHEEL] != null && c.equippedParts[PartTypes.STEERING_WHEEL].manufacturer != Manufacturers.NII_SAN)
				{
					c.acceleration /= 3;
				}
			};
		sprites[PartTypes.ENGINE][Manufacturers.VOLVIMUS] = imageVolvimusEngine;
		callbacks[PartTypes.ENGINE][Manufacturers.VOLVIMUS] =
			c => {
				if (c.equippedParts[PartTypes.EXHAUST_SYSTEM] != null && c.equippedParts[PartTypes.EXHAUST_SYSTEM].manufacturer == Manufacturers.SM)
				{
					c.engineSmoke.Play();
				}
			};
		sprites[PartTypes.ENGINE][Manufacturers.SM] = imageSMEngine;
		callbacks[PartTypes.ENGINE][Manufacturers.SM] =
			c => {

			};
		sprites[PartTypes.WHEELS][Manufacturers.NII_SAN] = imageNiiSanWheel;
		callbacks[PartTypes.WHEELS][Manufacturers.NII_SAN] =
			c => {

			};
		sprites[PartTypes.WHEELS][Manufacturers.VOLVIMUS] = imageVolvimusWheels;
		callbacks[PartTypes.WHEELS][Manufacturers.VOLVIMUS] =
			c => {

			};
		sprites[PartTypes.WHEELS][Manufacturers.SM] = imageSMWheels;
		callbacks[PartTypes.WHEELS][Manufacturers.SM] =
			c => {

			};
		sprites[PartTypes.GEAR_BOX][Manufacturers.NII_SAN] = imageNiiSanGearBox;
		callbacks[PartTypes.GEAR_BOX][Manufacturers.NII_SAN] =
			c => {
				c.maxFuel = 5;
			};
		sprites[PartTypes.GEAR_BOX][Manufacturers.VOLVIMUS] = imageVolvimusGearBox;
		callbacks[PartTypes.GEAR_BOX][Manufacturers.VOLVIMUS] =
			c => {
				c.maxFuel = 5;
			};
		sprites[PartTypes.GEAR_BOX][Manufacturers.SM] = imageSMGearBox;
		callbacks[PartTypes.GEAR_BOX][Manufacturers.SM] =
			c => {
				c.maxFuel = 5;
			};
		sprites[PartTypes.EXHAUST_SYSTEM][Manufacturers.NII_SAN] = imageNiiSanExhaust;
		callbacks[PartTypes.EXHAUST_SYSTEM][Manufacturers.NII_SAN] =
			c => {
				c.maxFuel = 5;
			};
		sprites[PartTypes.EXHAUST_SYSTEM][Manufacturers.VOLVIMUS] = imageVolvimusExhaust;
		callbacks[PartTypes.EXHAUST_SYSTEM][Manufacturers.VOLVIMUS] =
			c => {
				c.maxFuel = 5;
			};
		sprites[PartTypes.EXHAUST_SYSTEM][Manufacturers.SM] = imageSMExhaust;
		callbacks[PartTypes.EXHAUST_SYSTEM][Manufacturers.SM] =
			c => {
				c.maxFuel = 5;
			};
		sprites[PartTypes.BRAKES][Manufacturers.NII_SAN] = imageNiiSanBrakes;
		callbacks[PartTypes.BRAKES][Manufacturers.NII_SAN] =
			c => {
				c.maxFuel = 5;
			};
		sprites[PartTypes.BRAKES][Manufacturers.VOLVIMUS] = imageVolvimusBrakes;
		callbacks[PartTypes.BRAKES][Manufacturers.VOLVIMUS] =
			c => {
				c.maxFuel = 5;
			};
		sprites[PartTypes.BRAKES][Manufacturers.SM] = imageSMBrakes;
		callbacks[PartTypes.BRAKES][Manufacturers.SM] =
			c => {
				c.maxFuel = 5;
			};
		sprites[PartTypes.STEERING_WHEEL][Manufacturers.NII_SAN] = imageNiiSanWheel;
		callbacks[PartTypes.STEERING_WHEEL][Manufacturers.NII_SAN] =
			c => {
				c.maxFuel = 5;
			};
		sprites[PartTypes.STEERING_WHEEL][Manufacturers.VOLVIMUS] = imageVolvimusWheels;
		callbacks[PartTypes.STEERING_WHEEL][Manufacturers.VOLVIMUS] =
			c => {
				c.maxFuel = 5;
			};
		sprites[PartTypes.STEERING_WHEEL][Manufacturers.SM] = imageSMWheels;
		callbacks[PartTypes.STEERING_WHEEL][Manufacturers.SM] =
			c => {
				c.maxFuel = 5;
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

    // YOU MADE ME TO THIS!
    public CarPart Create(PartTypes type, Manufacturers manufacturer) {
        return Create(type, manufacturer, sprites[type][manufacturer], callbacks[type][manufacturer]);
    }
}
