using UnityEngine;

public class Hand : PartSlot
{
	private InteractiveSlot previousSlot = null;

	private void Awake()
	{
		onPartSet = OnPartSet;
	}

	private void OnPartSet(CarPart newPart, PartSlot previousSlot, PartSlot nextSlot)
	{
		// On drop
		if (newPart == null)
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
