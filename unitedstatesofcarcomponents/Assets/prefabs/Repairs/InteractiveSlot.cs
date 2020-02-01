using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InteractiveSlot : PartSlot
	 , IPointerClickHandler
	 , IPointerEnterHandler
	 , IPointerExitHandler
{
	private Image sprite;
	private Image partSprite;
	private PartSlot hand;

	private void Awake()
	{
		sprite = GetComponent<Image>();
		partSprite = transform.Find("PartSprite").GetComponent<Image>();
		hand = FindObjectOfType<Hand>();

		onPartSet = OnPartSet;
		ForceSet(null, null, null);
		sprite.color = Color.grey;
	}

	private void OnPartSet(CarPart newPart, PartSlot previousSlot, PartSlot nextSlot)
	{
		if (newPart == null)
		{
			if (nextSlot == null)
			{
				partSprite.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
			}
			else
			{
				partSprite.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
			}
		}
		else
		{
			partSprite.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
		}
	}

	public void RemoveAfterImage()
	{
		partSprite.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		// Add item to inventory
		if (hand.IsSet())
		{
			if (this.TrySet(hand.GetPart(), hand))
			{
				hand.TryClear(this);
			}
		}
		// Take item from inventory
		else
		{
			if (hand.TrySet(this.GetPart(), this))
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
