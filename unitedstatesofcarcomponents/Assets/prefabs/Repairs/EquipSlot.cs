using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipSlot : InteractiveSlot
{
#pragma warning disable IDE0044 // Add readonly modifier
#pragma warning disable CS0649 // Add readonly modifier Unity
	[SerializeField]
	private PartTypes partType;
#pragma warning restore IDE0044 // Add readonly modifier
#pragma warning restore CS0649 // Add readonly modifier Unity

	new private void Awake()
	{
		base.Awake();
		onPartSet = OnPartSet;
	}

	private void Start()
	{

		CarPartFactory carPartFactory = FindObjectOfType<CarPartFactory>();
		Manufacturers[] allManufacturers = (Manufacturers[])Enum.GetValues(typeof(Manufacturers));

		Manufacturers randomManufacturer = allManufacturers[UnityEngine.Random.Range(0, allManufacturers.Length - 1)];
		TrySet(carPartFactory.Create(partType, randomManufacturer), null);
	}

	public override bool IsCompatible(PartTypes type)
	{
		return type == partType;
	}

	public override bool TrySet(CarPart newPart, PartSlot previousSlot)
	{
		if (IsCompatible(newPart.type))
		{
			return base.TrySet(newPart, previousSlot);
		}
		else
		{
			return false;
		}
	}
	
	new private void OnPartSet(CarPart newPart, PartSlot previousSlot, PartSlot nextSlot)
	{
		base.OnPartSet(newPart, previousSlot, nextSlot);
		Car car = FindObjectOfType<Car>();
		if (car != null)
		{
			if (newPart == null)
			{
				car.RemovePart(partType);
			}
			else
			{
				car.AddPart(partType, newPart);
			}
		}
	}
}
