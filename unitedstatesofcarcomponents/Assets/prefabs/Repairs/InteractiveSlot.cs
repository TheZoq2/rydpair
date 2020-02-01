using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InteractiveSlot : ItemSlot
	 , IPointerClickHandler
	 , IPointerEnterHandler
	 , IPointerExitHandler
{
	private Image sprite;
	private Image itemSprite;
	private ItemSlot hand;

	private void Awake()
	{
		sprite = GetComponent<Image>();
		itemSprite = transform.Find("ItemSprite").GetComponent<Image>();
		hand = FindObjectOfType<Hand>();

		onItemSet = OnItemSet;
		ForceSet(null, null, null);
		sprite.color = Color.grey;
	}

	private void OnItemSet(GameObject newItem, ItemSlot previousSlot, ItemSlot nextSlot)
	{
		if (newItem == null)
		{
			if (nextSlot == null)
			{
				itemSprite.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
			}
			else
			{
				itemSprite.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
			}
		}
		else
		{
			itemSprite.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
		}
	}

	public void RemoveAfterImage()
	{
		itemSprite.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		// Add item to inventory
		if (hand.IsSet())
		{
			if (this.TrySet(hand.GetItem(), hand))
			{
				hand.TryClear(this);
			}
		}
		// Take item from inventory
		else
		{
			if (hand.TrySet(this.GetItem(), this))
			{
				this.TryClear(hand);
			}
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		sprite.color = Color.white;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		sprite.color = Color.grey;
	}
}
