using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
#pragma warning disable IDE0044 // Add readonly modifier
#pragma warning disable CS0649 // Add readonly modifier Unity
	[SerializeField]
	private InteractiveSlot slotPrefab;

	[SerializeField]
	private int slotSize;

	[SerializeField]
	private int cols;
	[SerializeField]
	private int rows;
#pragma warning restore IDE0044 // Add readonly modifier
#pragma warning restore CS0649 // Add readonly modifier Unity

	private CarPartFactory carPartFactory;

	List<InteractiveSlot> slots = new List<InteractiveSlot>();

	void Start()
    {
        carPartFactory = FindObjectOfType<CarPartFactory>();

        for (int c = 0; c < cols; ++c)
		{
			for (int r = 0; r < rows; ++r)
			{
				InteractiveSlot slot = Instantiate(slotPrefab) as InteractiveSlot;
				RectTransform slotTransform = slot.GetComponent<RectTransform>();
				slotTransform.SetParent(GetComponent<RectTransform>(), false);
				slotTransform.anchoredPosition = new Vector2(slotTransform.rect.width * c, -slotTransform.rect.height * r);
				slots.Add(slot);
			}
		}
		
		
		TryAddItem(carPartFactory.Create(PartTypes.ENGINE, Manufacturers.NII_SAN));
		TryAddItem(carPartFactory.Create(PartTypes.WHEELS, Manufacturers.NII_SAN));
		TryAddItem(carPartFactory.Create(PartTypes.EXHAUST_SYSTEM, Manufacturers.NII_SAN));
		TryAddItem(carPartFactory.Create(PartTypes.BRAKES, Manufacturers.NII_SAN));
		TryAddItem(carPartFactory.Create(PartTypes.STEERING_WHEEL, Manufacturers.NII_SAN));
	}

	public void DummyFunc(Car car)
	{
		
	}

	public bool TryAddItem(CarPart newPart)
	{
		foreach (InteractiveSlot slot in slots)
		{
			if (slot.TrySet(newPart, null)) {
				return true;
			}
		}

		return false;
	}
}
