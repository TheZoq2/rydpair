using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
	private GameObject item;
	private Image itemSprite;

	private void Awake()
	{
		itemSprite = transform.Find("ItemSprite").GetComponent<Image>();
		ForceSet(null);
	}

	private void ForceSet(GameObject new_item)
	{
		item = new_item;
		itemSprite.enabled = item != null;
	}

	/**
	 * Returns false if already occupied.
	 * Otherwise sets item at posiiton and returns true.
	 */
	public bool TrySet(GameObject new_item)
	{
		if (item == null)
		{
			ForceSet(new_item);
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
	public bool TryClear()
	{
		if (item != null)
		{
			ForceSet(null);
			return true;
		}
		else
		{
			return false;
		}
	}
}
