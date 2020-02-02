﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InteractiveSlot : PartSlot
	 , IPointerClickHandler
	 , IPointerEnterHandler
	 , IPointerExitHandler
{
	private Image sprite;
	private Image partSprite;
	private Image emptySprite;
    private Image hpBar;
	private PartSlot hand;

	protected void Awake()
	{
		sprite = GetComponent<Image>();
		partSprite = transform.Find("PartSprite").GetComponent<Image>();
		emptySprite = transform.Find("EmptySprite").GetComponent<Image>();
		hpBar = transform.Find("HpBar").GetComponent<Image>();
		hand = FindObjectOfType<Hand>();

        onPartSet = OnPartSet;
		ForceSet(null, null, null);
		sprite.color = Color.grey;
	}

    void Update() {
        if(GetPart() != null) {
            hpBar.fillAmount = GetPart().maxHealth / GetPart().currentHealth;
        }
    }

	protected void OnPartSet(CarPart newPart, PartSlot previousSlot, PartSlot nextSlot)
	{
		if (newPart == null)
		{
			if (nextSlot == null)
			{
				partSprite.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
				emptySprite.enabled = true;
			}
			else
			{
				partSprite.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
				emptySprite.enabled = true;
			}
		}
		else
		{
            newPart.slot = this;
			partSprite.sprite = newPart.sprite;
			partSprite.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
			emptySprite.enabled = false;
		}
	}

	public void RemoveAfterImage()
	{
		partSprite.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
		emptySprite.enabled = true;
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
