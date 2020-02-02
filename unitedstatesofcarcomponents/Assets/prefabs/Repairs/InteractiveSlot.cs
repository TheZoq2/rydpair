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
	private Image emptySprite;
    private Image hpBar;
	private Hand hand;

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
            Debug.Log($"Current: {GetPart().currentHealth}, max: {GetPart().maxHealth}");
            hpBar.fillAmount = GetPart().currentHealth / GetPart().maxHealth;
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
                hpBar.enabled = false;
			}
			else
			{
				partSprite.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
				emptySprite.enabled = true;
                hpBar.enabled = false;
			}
		}
		else
		{
            newPart.slot = this;
			partSprite.sprite = newPart.sprite;
			partSprite.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
			emptySprite.enabled = false;
            hpBar.enabled = true;
		}
	}

	public void RemoveAfterImage()
	{
		if (!IsSet())
		{
			partSprite.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
			emptySprite.enabled = true;
			hpBar.enabled = false;
		}
	}

	public virtual bool IsCompatible(PartTypes type)
	{
		return true;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (hand.IsSet())
		{
			// Add item to inventory
			if (!this.IsSet())
			{
				this.TrySet(hand.GetPart(), hand);
				hand.TryClear(this);
			}
			// Swap items
			else if (this.IsCompatible(hand.GetPart().type) && hand.previousSlot.IsCompatible(this.GetPart().type))
			{
				CarPart newPart = hand.GetPart();
				hand.previousSlot.ForceSet(this.GetPart(), this, this);
				this.ForceSet(newPart, hand, hand);
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
