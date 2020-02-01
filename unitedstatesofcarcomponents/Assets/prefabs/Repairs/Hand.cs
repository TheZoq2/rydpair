using UnityEngine;

public class Hand : ItemSlot
{
	private InteractiveSlot previousSlot = null;

	private void Awake()
	{
		onItemSet = OnItemSet;
	}

	private void OnItemSet(GameObject newItem, ItemSlot previousSlot, ItemSlot nextSlot)
	{
		// On drop
		if (newItem == null)
		{
			if (this.previousSlot != null && this.previousSlot != nextSlot)
			{
				this.previousSlot.RemoveAfterImage();
			}
		}
		// On pickup
		else
		{
			this.previousSlot = previousSlot as InteractiveSlot;
		}
	}
}
