using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ListController : MonoBehaviour
{
  private IslandInventory inventoryI;
  private PlayerInventory inventoryP;

  public Sprite[] Icons;
  public GameObject ContentPanel;
  public GameObject ListItemPrefab;

  // Use this for initialization
  void Start() {

    // For player shop
    if (name == "SellPanel") {
      PlayerManager managerP = PlayerManager.GetInstance();
      inventoryP = managerP.player.inventory;
      FillSellShop();
    }

    // For island shop
    if (name == "BuyPanel") {
      IslandManager managerI = IslandManager.GetInstance();

      inventoryI = managerI.island.inventory;
      FillBuyShop();
    }
  }

  void FillBuyShop()
  {
    for (int i = 0; i < inventoryI.food.Count; ++i)
    {
      GameObject newItem = Instantiate(ListItemPrefab) as GameObject;
      ListItemController controller = newItem.GetComponent<ListItemController>();

      controller.name = inventoryI.food[i].name;
      controller.Name.text = inventoryI.food[i].name;
      controller.Count.text = inventoryI.food[i].quantity.ToString();
      controller.Price.text = inventoryI.food[i].price.ToString();

      newItem.transform.SetParent(ContentPanel.transform);
      newItem.transform.localScale = Vector3.one;
    }

    for (int i = 0; i < inventoryI.weapons.Count; ++i)
    {
      GameObject newItem = Instantiate(ListItemPrefab) as GameObject;
      ListItemController controller = newItem.GetComponent<ListItemController>();

      controller.name = inventoryI.weapons[i].name;
      controller.Name.text = inventoryI.weapons[i].name;
      controller.Count.text = inventoryI.weapons[i].quantity.ToString();
      controller.Price.text = inventoryI.weapons[i].price.ToString();
      newItem.transform.SetParent(ContentPanel.transform);
      newItem.transform.localScale = Vector3.one;
    }
  }

  void FillSellShop()
  {
    for (int i = 0; i < inventoryP.food.Count; ++i)
    {
      GameObject newItem = Instantiate(ListItemPrefab) as GameObject;
      ListItemController controller = newItem.GetComponent<ListItemController>();

      controller.name = inventoryP.food[i].name;
      controller.Name.text = inventoryP.food[i].name;
      controller.Count.text = inventoryP.food[i].quantity.ToString();
      controller.Price.text = inventoryP.food[i].price.ToString();
      newItem.transform.SetParent(ContentPanel.transform);
      newItem.transform.localScale = Vector3.one;
    }

    for (int i = 0; i < inventoryP.weapons.Count; ++i)
    {
      GameObject newItem = Instantiate(ListItemPrefab) as GameObject;
      ListItemController controller = newItem.GetComponent<ListItemController>();

      controller.name = inventoryP.weapons[i].name;
      controller.Name.text = inventoryP.weapons[i].name;
      controller.Count.text = inventoryP.weapons[i].quantity.ToString();
      controller.Price.text = inventoryP.weapons[i].price.ToString();
      newItem.transform.SetParent(ContentPanel.transform);
      newItem.transform.localScale = Vector3.one;
    }
  }
  static string idIsland =  "";
  static string idPlayer =  "";

  void doTradeAction()
  {
    ListItemController[] allChildren = GetComponentsInChildren<ListItemController>();

    foreach (ListItemController child in allChildren) {

      if (idIsland != "" && child.transform.parent.name == "SellContent")
      {
        foreach (ListItemController stuff in allChildren)
        {
          if (stuff.Name.text == idIsland)
          {
            stuff.Count.text = (int.Parse(stuff.Count.text) + 1).ToString();
            idIsland = "";
            return;
          }
        }

        GameObject newItem = Instantiate(ListItemPrefab) as GameObject;
        ListItemController controller = newItem.GetComponent<ListItemController>();

        controller.name = idIsland;
        controller.Name.text = idIsland;
        controller.Count.text = "1";
        controller.Price.text = "0";
        newItem.transform.SetParent(ContentPanel.transform);
        newItem.transform.localScale = Vector3.one;
        return;
      }
      if (child.getExchange() && child.Count.text != "0") {
        child.Count.text = (int.Parse(child.Count.text) - 1).ToString();
        if (child.transform.parent.name == "BuyContent")
          idIsland = child.Name.text;
        if (child.transform.parent.name == "SellContent")
          idPlayer = child.Name.text;
        return;
      }
    }
  }

  // Update is called once per frame
  void Update() {
    doTradeAction();
  }
}
