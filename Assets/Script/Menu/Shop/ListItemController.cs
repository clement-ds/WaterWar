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
    void Start() {
    }

  private void loadImage()
  {
    Sprite sprite = Resources.Load<Sprite>("Sprites/Images/" + source.name);
    if (sprite == null)
      sprite = Resources.Load<Sprite>("Sprites/Images/Spider Web");
    Icon.GetComponent<Image>().sprite = sprite;
  }

  public void InitBuyCell()
    {
        count.text = source.quantity.ToString();
        objName.text = source.name;
        price.text = source.price.ToString();
        btn = this.GetComponent<Button>();
        btn.onClick.AddListener(delegate { lc.Buy(source); });
        loadImage();
  }

  public void InitSellCell()
    {
        count.text = source.quantity.ToString();
        objName.text = source.name;
        price.text = source.price.ToString();
        btn = this.GetComponent<Button>();
        btn.onClick.AddListener(delegate { lc.Sell(source); });
        loadImage();
  }

  // Update is called once per frame
  void Update()
    {
      if (source.quantity.ToString() != count.text)
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
