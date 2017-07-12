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

  private void loadImage()
  {
    if (source.name == "Apple")
      Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Apple");
    if (source.name == "Beef")
      Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Images/Beef");
    if (source.name == "Water")
      Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Images/Water");
    if (source.name == "Wine")
      Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Images/Wine");
    if (source.name == "Biscuit")
      Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Images/Biscuit");
    if (source.name == "Tea")
      Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Images/Spider Web");
    if (source.name == "Shrapnel")
      Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Images/Shrapnel");
    if (source.name == "Bread")
      Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Images/Bread");
    if (source.name == "Sabre")
      Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Images/Sabre");
    if (source.name == "Coconut")
      Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Images/Coconut");
    if (source.name == "Banane")
      Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Images/Banane");
    if (source.name == "BlackPowder")
      Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Images/BlackPowder");
    if (source.name == "Canon ball")
      Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Images/Cannonballs");
    if (source.name == "Rhum")
      Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Images/Rhum");
    if (source.name == "Coffee")
      Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Images/Coffee");
    if (source.name == "Pistol")
      Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Images/Pistol");
    if (source.name == "Fish")
      Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Images/Fish");
    if (source.name == "Graplin hooks")
      Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Images/Grappling_Hook");
    if (source.name == "Canon")
      Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Images/Canon");
    if (source.name == "Musket")
      Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Images/Musket");
    if (source.name == "Rifle")
      Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Images/Pistol");
    if (source.name == "Bullet")
      Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Images/Spider Web");
    if (source.name == "Canon")
      Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Images/Spider Web");
    if (source.name == "Black powder")
      Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Images/BlackPowder");
    if (source.name == "Olive")
      Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Images/Olive");
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
