using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{

    public RectTransform itemHolder;
    public RectTransform shopItem;

    private string makeReadable(string ugly){
        var better = ugly.Replace("_"," ").ToLower();
        var textInfo = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo;
        return textInfo.ToTitleCase(better);
    }

    private void switchTab(PartTypes type){
        Debug.Log("Switched to " + type.ToString());
    }

    // Start is called before the first frame update
    void Start()
    {
        var partList = new ArrayList{"al", "afds", "skdfdsa", "fdsaf", "fafdsfa"};
        var tabAreaSize = this.GetComponent<RectTransform>().sizeDelta.x;

        for (int i = 0; i < partList.Count; i++)
        {
            string part = (string)partList[i];
            var newItem = Instantiate(shopItem) as RectTransform;

            newItem.GetComponent<RectTransform>().position = new Vector3(0, i*-100.0f, 0);
            newItem.SetParent(itemHolder, false);
            newItem.GetComponentInChildren<Text>().text = part;

            newItem.GetComponent<Button>().onClick.AddListener(() =>
            {
                Debug.Log("Hi boyi");
            });
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
