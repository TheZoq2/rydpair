using System;
using UnityEngine;

public class PartSlot : MonoBehaviour
{
	private CarPart part = null;

	protected Action<CarPart, PartSlot, PartSlot> onPartSet = null;

	protected void ForceSet(CarPart newPart, PartSlot previousSlot, PartSlot nextSlot)
	{
		part = newPart;

		if (onPartSet != null)
		{
			onPartSet.Invoke(newPart, previousSlot, nextSlot);
		}
	}

	public CarPart GetPart()
	{
		return part;
	}

	public bool IsSet()
	{
		return part != null;
	}

	/**
	 * Returns false if already occupied.
	 * Otherwise sets item at posiiton and returns true.
	 */
	public virtual bool TrySet(CarPart newPart, PartSlot previousSlot)
	{
		if (!IsSet())
		{
			ForceSet(newPart, previousSlot, null);
			return true;
		}
		else
		{
			return false;
		}
	}

	/**
	 * Returns false if isn't occupied.
	 * Otherwise removes item and returns true.
	 */
	public bool TryClear(PartSlot nextSlot)
	{
		if (IsSet())
		{
			ForceSet(null, null, nextSlot);
			return true;
		}
		else
		{
			return false;
		}
	}
}
