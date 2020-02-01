using System;
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
        switch (type) {
            case PartTypes.BRAKES:
                switch (manufacturer) {
                    case Manufacturers.NII_SAN:
                        return CreateNiiSanBrakes();
                    case Manufacturers.SM:
                        return CreateSMBrakes();
                    case Manufacturers.VOLVIMUS:
                        return CreateVolvimusBrakes();
                    default:
                        Application.Quit(-1);
                        return null;
                }
            case PartTypes.ENGINE:
                switch (manufacturer) {
                    case Manufacturers.NII_SAN:
                        return CreateNiiSanEngine();
                    case Manufacturers.SM:
                        return CreateSMEngine();
                    case Manufacturers.VOLVIMUS:
                        return CreateVolvimusEngine();
                    default:
                        Application.Quit(-1);
                        return null;
                }
            case PartTypes.EXHAUST_SYSTEM:
                switch (manufacturer) {
                    case Manufacturers.NII_SAN:
                        return CreateNiiSanExhaust();
                    case Manufacturers.SM:
                        return CreateSMExhaust();
                    case Manufacturers.VOLVIMUS:
                        return CreateVolvimusExhaust();
                    default:
                        Application.Quit(-1);
                        return null;
                }
            case PartTypes.GEAR_BOX:
                switch (manufacturer) {
                    case Manufacturers.NII_SAN:
                        return CreateNiiSanGearBox();
                    case Manufacturers.SM:
                        return CreateSMGearBox();
                    case Manufacturers.VOLVIMUS:
                        return CreateVolvimusGearBox();
                    default:
                        Application.Quit(-1);
                        return null;
                }
            case PartTypes.STEERING_WHEEL:
                switch (manufacturer) {
                    case Manufacturers.NII_SAN:
                        return CreateNiiSanSteeringWheel();
                    case Manufacturers.SM:
                        return CreateSMSteeringWheel();
                    case Manufacturers.VOLVIMUS:
                        return CreateVolvimusSteeringWheel();
                    default:
                        Application.Quit(-1);
                        return null;
                }
            case PartTypes.WHEELS:
                switch (manufacturer) {
                    case Manufacturers.NII_SAN:
                        return CreateNiiSanWheels();
                    case Manufacturers.SM:
                        return CreateSMWheels();
                    case Manufacturers.VOLVIMUS:
                        return CreateVolvimusWheels();
                    default:
                        Application.Quit(-1);
                        return null;
                }
        }
        return null;
    }

    /// <summary>
    /// Engine that has terrible fuel drain if not using a NII_SAN Gearbox and/or terrible acceleration if not using a NII_SAN Steering wheel.
    /// </summary>
    /// <returns></returns>
    public CarPart CreateNiiSanEngine() {
        return Create(PartTypes.ENGINE, Manufacturers.NII_SAN, 
            c => {
                if(c.equippedParts[PartTypes.GEAR_BOX].manufacturer != Manufacturers.NII_SAN) {
                    c.fuelDrain *= 10;
                }

                if (c.equippedParts[PartTypes.STEERING_WHEEL].manufacturer != Manufacturers.NII_SAN) {
                    c.acceleration /= 3;
                }
            });
    }

    public CarPart CreateVolvimusEngine() {
        return Create(PartTypes.ENGINE, Manufacturers.VOLVIMUS, 
            c => {
                if(c.equippedParts[PartTypes.EXHAUST_SYSTEM].manufacturer == Manufacturers.SM) {
                    c.engineSmoke.Play();
                }
            });
    }

    public CarPart CreateSMEngine() {
        return Create(PartTypes.ENGINE, Manufacturers.SM,
            c => {

            });
    }

    public CarPart CreateNiiSanWheels() {
        return Create(PartTypes.WHEELS, Manufacturers.NII_SAN,
            c => {

            });
    }

    public CarPart CreateVolvimusWheels() {
        return Create(PartTypes.WHEELS, Manufacturers.VOLVIMUS,
            c => {

            });
    }

    public CarPart CreateSMWheels() {
        return Create(PartTypes.WHEELS, Manufacturers.SM,
            c => {

            });
    }

    public CarPart CreateNiiSanGearBox() {
        return Create(PartTypes.GEAR_BOX, Manufacturers.NII_SAN, 
            c => {
                c.maxFuel = 5;
            });
    }

    public CarPart CreateVolvimusGearBox() {
        return Create(PartTypes.GEAR_BOX, Manufacturers.VOLVIMUS,
            c => {
                c.maxFuel = 5;
            });
    }

    public CarPart CreateSMGearBox() {
        return Create(PartTypes.GEAR_BOX, Manufacturers.SM,
            c => {
                c.maxFuel = 5;
            });
    }

    public CarPart CreateNiiSanExhaust() {
        return Create(PartTypes.EXHAUST_SYSTEM, Manufacturers.NII_SAN,
            c => {
                c.maxFuel = 5;
            });
    }

    public CarPart CreateVolvimusExhaust() {
        return Create(PartTypes.EXHAUST_SYSTEM, Manufacturers.VOLVIMUS,
            c => {
                c.maxFuel = 5;
            });
    }

    public CarPart CreateSMExhaust() {
        return Create(PartTypes.EXHAUST_SYSTEM, Manufacturers.SM,
            c => {
                c.maxFuel = 5;
            });
    }

    public CarPart CreateNiiSanBrakes() {
        return Create(PartTypes.BRAKES, Manufacturers.NII_SAN,
            c => {
                c.maxFuel = 5;
            });
    }

    public CarPart CreateVolvimusBrakes() {
        return Create(PartTypes.BRAKES, Manufacturers.VOLVIMUS,
            c => {
                c.maxFuel = 5;
            });
    }

    public CarPart CreateSMBrakes() {
        return Create(PartTypes.BRAKES, Manufacturers.SM,
            c => {
                c.maxFuel = 5;
            });
    }

    public CarPart CreateNiiSanSteeringWheel() {
        return Create(PartTypes.STEERING_WHEEL, Manufacturers.NII_SAN,
            c => {
                c.maxFuel = 5;
            });
    }

    public CarPart CreateVolvimusSteeringWheel() {
        return Create(PartTypes.STEERING_WHEEL, Manufacturers.VOLVIMUS,
            c => {
                c.maxFuel = 5;
            });
    }

    public CarPart CreateSMSteeringWheel() {
        return Create(PartTypes.STEERING_WHEEL, Manufacturers.SM, 
            c => {
                c.maxFuel = 5;
            });
    }
}
