using System;
using System.Collections.Generic;
using UnityEngine;

public class CarPartFactory : MonoBehaviour {
    public CarPart carPartPrefab;

    public CarPart Create(PartTypes type, Manufacturers manufacturer, Action<Car> setPartsDelegate) {
        CarPart newPart = Instantiate(carPartPrefab);

        newPart.type = type;
        newPart.manufacturer = manufacturer;
        newPart.UpdateEffect = setPartsDelegate ?? (_ => throw new System.NotImplementedException($"The part equipped in slot {type} is not implemented"));

        //TODO: Set other stuff

        return newPart;
    }

    // YOU MADE ME TO THIS!
    public CarPart Create(PartTypes type, Manufacturers manufacturer) {
        Dictionary<PartTypes, Dictionary<Manufacturers, Action<Car>>> callbacks = new Dictionary<PartTypes, Dictionary<Manufacturers, Action<Car>>>();
        foreach (PartTypes partType in Enum.GetValues(typeof(PartTypes))) {
            callbacks[partType] = new Dictionary<Manufacturers, Action<Car>>();
        }
        callbacks[PartTypes.ENGINE][Manufacturers.NII_SAN] =
            c => {
                if(c.equippedParts[PartTypes.GEAR_BOX].manufacturer != Manufacturers.NII_SAN) {
                    c.fuelDrain *= 10;
                }

                if (c.equippedParts[PartTypes.STEERING_WHEEL].manufacturer != Manufacturers.NII_SAN) {
                    c.acceleration /= 3;
                }
            };
        callbacks[PartTypes.ENGINE][Manufacturers.VOLVIMUS] =
            c => {
                if(c.equippedParts[PartTypes.EXHAUST_SYSTEM].manufacturer == Manufacturers.SM) {
                    c.engineSmoke.Play();
                }
            };
        callbacks[PartTypes.ENGINE][Manufacturers.SM] =
            c => {

            };
        callbacks[PartTypes.WHEELS][Manufacturers.NII_SAN] =
            c => {

            };
        callbacks[PartTypes.WHEELS][Manufacturers.VOLVIMUS] =
            c => {

            };
        callbacks[PartTypes.WHEELS][Manufacturers.SM] =
            c => {

            };
        callbacks[PartTypes.GEAR_BOX][Manufacturers.NII_SAN] =
            c => {
                c.maxFuel = 5;
            };
        callbacks[PartTypes.GEAR_BOX][Manufacturers.VOLVIMUS] =
            c => {
                c.maxFuel = 5;
            };
        callbacks[PartTypes.GEAR_BOX][Manufacturers.SM] =
            c => {
                c.maxFuel = 5;
            };
        callbacks[PartTypes.EXHAUST_SYSTEM][Manufacturers.NII_SAN] =
            c => {
                c.maxFuel = 5;
            };
        callbacks[PartTypes.EXHAUST_SYSTEM][Manufacturers.VOLVIMUS] =
            c => {
                c.maxFuel = 5;
            };
        callbacks[PartTypes.EXHAUST_SYSTEM][Manufacturers.SM] =
            c => {
                c.maxFuel = 5;
            };
        callbacks[PartTypes.BRAKES][Manufacturers.NII_SAN] =
            c => {
                c.maxFuel = 5;
            };
        callbacks[PartTypes.BRAKES][Manufacturers.VOLVIMUS] =
            c => {
                c.maxFuel = 5;
            };
        callbacks[PartTypes.BRAKES][Manufacturers.SM] =
            c => {
                c.maxFuel = 5;
            };
        callbacks[PartTypes.STEERING_WHEEL][Manufacturers.NII_SAN] =
            c => {
                c.maxFuel = 5;
            };
        callbacks[PartTypes.STEERING_WHEEL][Manufacturers.VOLVIMUS] =
            c => {
                c.maxFuel = 5;
            };
        callbacks[PartTypes.STEERING_WHEEL][Manufacturers.SM] =
            c => {
                c.maxFuel = 5;
            };

        return Create(type, manufacturer, callbacks[type][manufacturer]);
    }
}
