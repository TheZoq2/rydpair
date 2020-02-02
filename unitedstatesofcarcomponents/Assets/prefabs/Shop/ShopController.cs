using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    public RectTransform itemHolder;
    public RectTransform shopItem;
    private CarPartFactory factory;
    private Inventory inv;
    public float itemHeight = 300.0f;

    private int money = 500;
    public TextMeshProUGUI moneyText;

    private Dictionary<PartTypes, int> partCosts = new Dictionary<PartTypes, int>{
        {PartTypes.BRAKES, 30},
        {PartTypes.ENGINE, 100},
        {PartTypes.EXHAUST_SYSTEM, 50},
        {PartTypes.GEAR_BOX, 40},
        {PartTypes.STEERING_WHEEL, 30},
        {PartTypes.WHEELS, 50},
    };

    private void updateMoneyUI(){
        this.moneyText.text = "Money: "+this.money.ToString()+" $";
    }

    public void changeMoney(int deltaMoney){
        this.money += deltaMoney;
        updateMoneyUI();
    }

    public int getMoney(){
        return this.money;
    }

    private string makeReadable(string ugly){
        var better = ugly.Replace("_"," ").ToLower();
        var textInfo = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo;
        return textInfo.ToTitleCase(better);
    }

    // Start is called before the first frame update
    void Start()
    {
        updateMoneyUI();

        factory = FindObjectOfType<CarPartFactory>();
        inv = FindObjectOfType<Inventory>();
        
        var tabAreaSize = this.GetComponent<RectTransform>().sizeDelta.x;
        int itemI = 0;
        foreach (KeyValuePair<PartTypes, Dictionary<Manufacturers, Sprite>> partTypeEntry in factory.sprites)
        {
            foreach (KeyValuePair<Manufacturers, Sprite> manufacturerEntry in partTypeEntry.Value)
            {
                var manu = manufacturerEntry.Key;
                var partType = partTypeEntry.Key;
                var itemSprite = manufacturerEntry.Value;
                int cost = partCosts[partType];

                string partName = manu.ToString() + "_" + partType.ToString() + "_("+cost.ToString()+"_$)";
                var newItem = Instantiate(shopItem) as RectTransform;

                newItem.GetComponent<RectTransform>().position = new Vector3(0, itemI*(-1)*this.itemHeight, 0);
                newItem.GetComponent<RectTransform>().sizeDelta = new Vector2(0, this.itemHeight);
                newItem.SetParent(itemHolder, false);
                newItem.GetComponentInChildren<Text>().text = this.makeReadable(partName);
                newItem.GetComponentsInChildren<Image>()[1].sprite = itemSprite;

                newItem.GetComponent<Button>().onClick.AddListener(() =>
                {
                    var newPart = factory.Create(partType, manu);

                    // Add to inventory (?)
                    if (this.money >= cost){
                        inv.TryAddItem(newPart);
                        this.changeMoney(-cost);
                    }
                });

                ++itemI;
            }
        }

        itemHolder.sizeDelta = new Vector2(0, itemI*this.itemHeight);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
