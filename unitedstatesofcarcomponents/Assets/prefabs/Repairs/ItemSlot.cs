using System;
using UnityEngine;

public class ItemSlot : MonoBehaviour
{
	private GameObject item = null;

	protected Action<GameObject, ItemSlot, ItemSlot> onItemSet = null;

	protected void ForceSet(GameObject newItem, ItemSlot previousSlot, ItemSlot nextSlot)
	{
		item = newItem;

		if (onItemSet != null)
		{
			onItemSet.Invoke(newItem, previousSlot, nextSlot);
		}
	}

	public GameObject GetItem()
	{
		return item;
	}

	public bool IsSet()
	{
		return item != null;
	}

	/**
	 * Returns false if already occupied.
	 * Otherwise sets item at posiiton and returns true.
	 */
	public bool TrySet(GameObject newItem, ItemSlot previousSlot)
	{
		if (!IsSet())
		{
			ForceSet(newItem, previousSlot, null);
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
	public bool TryClear(ItemSlot nextSlot)
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
