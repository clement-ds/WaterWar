using UnityEngine;
using System.Collections;

public class ListController : MonoBehaviour {
  GameObject shopMenu;
  IslandManager manager;
  Inventory inventory;

  public Sprite[] Icons;
  public GameObject ContentPanel;
  public GameObject ListItemPrefab;
  public GameObject shopList;

  public int ressourceNumber;

  // Use this for initialization
  void Start () {
    manager = IslandManager.GetInstance();
    shopMenu = GameObject.Find("ShopCanvas");
    inventory = manager.island.inventory;
    FillSellShop();
  }

  void FillSellShop() {
    IslandManager im = IslandManager.GetInstance();
    IslandInventory inv = im.island.inventory;

    for (int i = 0; i < inv.food.Count; ++i) {
      GameObject newItem = Instantiate(ListItemPrefab) as GameObject;
      ListItemController controller = newItem.GetComponent<ListItemController>();
      
      controller.Name.text = inv.food[i].name;
      controller.Count.text = inv.food[i].number.ToString();
      controller.Price.text = inv.food[i].number.ToString();
      newItem.transform.parent = ContentPanel.transform;
      newItem.transform.localScale = Vector3.one;
    }
  }

  // Update is called once per frame
  void Update () {
	
	}
}
