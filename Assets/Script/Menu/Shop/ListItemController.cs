using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ListItemController : MonoBehaviour
{

    public Image Icon;
    public Text count, objName, price;
    private bool toExchange = false;
    public ListController lc;
    public InventoryObject source;

    private Button btn;

    // Use this for initialization
    void Start()
    {
        //btn = this.GetComponent<Button>();
        //btn.onClick.AddListener(delegate { lc.Buy(source); });
    }

    public void InitBuyCell()
    {
        count.text = source.quantity.ToString();
        objName.text = source.name;
        price.text = source.price.ToString();
        btn = this.GetComponent<Button>();
        btn.onClick.AddListener(delegate { lc.Buy(source); });
    }

    public void InitSellCell()
    {
        count.text = source.quantity.ToString();
        objName.text = source.name;
        price.text = source.price.ToString();
        btn = this.GetComponent<Button>();
        btn.onClick.AddListener(delegate { lc.Sell(source); });
    }

    // Update is called once per frame
    void Update()
    {
        count.text = source.quantity.ToString();
    }

    void TaskOnClick()
    {
        toExchange = true;
        Debug.Log("Click ! ");
    }

    public bool getExchange()
    {
        if (toExchange)
        {
            toExchange = false;
            return true;
        }
        return false;
    }
}
