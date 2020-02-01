using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
#pragma warning disable IDE0044 // Add readonly modifier
#pragma warning disable CS0649 // Add readonly modifier Unity
	[SerializeField]
	private InventorySlot slotPrefab;

	[SerializeField]
	private int slotSize;

	[SerializeField]
	private int cols;
	[SerializeField]
	private int rows;
#pragma warning restore IDE0044 // Add readonly modifier
#pragma warning restore CS0649 // Add readonly modifier Unity

	List<InventorySlot> slots = new List<InventorySlot>();

	void Start()
    {
        for (int c = 0; c < cols; ++c)
		{
			for (int r = 0; r < rows; ++r)
			{
				InventorySlot slot = Instantiate(slotPrefab) as InventorySlot;
				RectTransform slotTransform = slot.GetComponent<RectTransform>();
				slotTransform.SetParent(GetComponent<RectTransform>(), false);
				slotTransform.anchoredPosition = new Vector2(slotTransform.rect.width * c, -slotTransform.rect.height * r);
				slots.Add(slot);
			}
		}

		slots[5].TrySet(gameObject);

	}
}
