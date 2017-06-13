using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ListController : MonoBehaviour
{
  private IslandInventory inventoryI;
  private PlayerInventory inventoryP;

  private List<ListItemController> listI;
  private List<ListItemController> listP;

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
      IslandGenerator generator = new IslandGenerator();

      generator.GenerateObjects();
      generator.GenerateIsland(managerI.island);

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
      controller.Count.text = inventoryI.food[i].number.ToString();
      controller.Price.text = inventoryI.food[i].number.ToString();

      newItem.transform.SetParent(ContentPanel.transform);
      newItem.transform.localScale = Vector3.one;
    }

    for (int i = 0; i < inventoryI.weapons.Count; ++i)
    {
      GameObject newItem = Instantiate(ListItemPrefab) as GameObject;
      ListItemController controller = newItem.GetComponent<ListItemController>();

      controller.name = inventoryI.weapons[i].name;
      controller.Name.text = inventoryI.weapons[i].name;
      controller.Count.text = inventoryI.weapons[i].number.ToString();
      controller.Price.text = inventoryI.weapons[i].number.ToString();
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
      controller.Count.text = inventoryP.food[i].number.ToString();
      controller.Price.text = inventoryP.food[i].number.ToString();
      newItem.transform.SetParent(ContentPanel.transform);
      newItem.transform.localScale = Vector3.one;
    }

    for (int i = 0; i < inventoryP.weapons.Count; ++i)
    {
      GameObject newItem = Instantiate(ListItemPrefab) as GameObject;
      ListItemController controller = newItem.GetComponent<ListItemController>();

      controller.name = inventoryP.weapons[i].name;
      controller.Name.text = inventoryP.weapons[i].name;
      controller.Count.text = inventoryP.weapons[i].number.ToString();
      controller.Price.text = inventoryP.weapons[i].number.ToString();
      newItem.transform.SetParent(ContentPanel.transform);
      newItem.transform.localScale = Vector3.one;
    }
  }

  // Update is called once per frame
  void Update() {
    
  }
}
