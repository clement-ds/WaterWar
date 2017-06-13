using UnityEngine;
using System.Collections;

public class ListController : MonoBehaviour
{
  private IslandInventory inventoryI;
  private PlayerInventory inventoryP;

  public Sprite[] Icons;
  public GameObject ContentPanel;
  public GameObject ListItemPrefab;

  // Use this for initialization
  void Start() {
    IslandManager managerI = IslandManager.GetInstance();
    PlayerManager managerP = PlayerManager.GetInstance();
    IslandGenerator generator = new IslandGenerator();

    generator.GenerateObjects();
    generator.GenerateIsland(managerI.island);

    inventoryI = managerI.island.inventory;
    inventoryP = managerP.player.inventory;
    FillBuyShop();
    FillSellShop();
  }

  void FillBuyShop()
  {
    for (int i = 0; i < inventoryI.food.Count; ++i)
    {
      GameObject newItem = Instantiate(ListItemPrefab) as GameObject;
      ListItemController controller = newItem.GetComponent<ListItemController>();

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
