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

	public override bool TrySet(CarPart newPart, PartSlot previousSlot)
	{
		if (newPart.type == partType)
		{
			return base.TrySet(newPart, previousSlot);
		}
		else
		{
			return false;
		}
	}
}
