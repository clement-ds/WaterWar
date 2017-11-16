using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ListController : MonoBehaviour
{
    private IslandInventory inventoryI;
    private PlayerInventory inventoryP;

    private int currentIsland;

    static string idIsland = "";
    static string idPlayer = "";

    public Sprite[] Icons;
    public GameObject ContentPanelShop;
    public GameObject ContentPanelPlayer;
    public GameObject ListItemPrefab;

    private List<GameObject> buyList = new List<GameObject>();
    private List<GameObject> sellList = new List<GameObject>();

    // Use this for initialization
    void Start() {
      PlayerManager managerP = PlayerManager.GetInstance();
      inventoryP = managerP.player.inventory;
      FillSellShop();

      currentIsland = PlayerManager.GetInstance().player.currentIsland;
      IslandManager managerI = IslandManager.GetInstance();

      inventoryI = managerI.islands[currentIsland].inventory;
      FillBuyShop();
    }

    void FillBuyShop()
    {
        for (int i = 0; i < buyList.Count; ++i)
        {
            Destroy(buyList[i]);
        }

        for (int i = 0; i < inventoryI.food.Count; ++i)
        {
            GameObject newItem = Instantiate(ListItemPrefab) as GameObject;
            ListItemController controller = newItem.GetComponent<ListItemController>();

            controller.lc = this;
            controller.source = inventoryI.food[i];

            controller.InitBuyCell();
            newItem.transform.SetParent(ContentPanelShop.transform);
            newItem.transform.localScale = Vector3.one;
            buyList.Add(newItem);
        }

        for (int i = 0; i < inventoryI.weapons.Count; ++i)
        {
            GameObject newItem = Instantiate(ListItemPrefab) as GameObject;
            ListItemController controller = newItem.GetComponent<ListItemController>();

            controller.lc = this;
            controller.source = inventoryI.weapons[i];

            controller.InitBuyCell();
            newItem.transform.SetParent(ContentPanelShop.transform);
            newItem.transform.localScale = Vector3.one;
            buyList.Add(newItem);
        }
    }

    void FillSellShop()
    {
        for (int i = 0; i < sellList.Count; ++i)
        {
            Destroy(sellList[i]);
        }
        for (int i = 0; i < inventoryP.food.Count; ++i)
        {
            GameObject newItem = Instantiate(ListItemPrefab) as GameObject;
            ListItemController controller = newItem.GetComponent<ListItemController>();

            controller.lc = this;
            controller.source = inventoryP.food[i];

            controller.InitSellCell();
            newItem.transform.SetParent(ContentPanelPlayer.transform);
            newItem.transform.localScale = Vector3.one;
            sellList.Add(newItem);
        }

        for (int i = 0; i < inventoryP.weapons.Count; ++i)
        {
            GameObject newItem = Instantiate(ListItemPrefab) as GameObject;
            ListItemController controller = newItem.GetComponent<ListItemController>();

            controller.lc = this;
            controller.source = inventoryP.weapons[i];

            controller.InitSellCell();
            newItem.transform.SetParent(ContentPanelPlayer.transform);
            newItem.transform.localScale = Vector3.one;
            sellList.Add(newItem);
        }
    }

    public void Buy(InventoryObject source)
    {
        Player p = PlayerManager.GetInstance().player;
        if (p.money >= source.price)
        {
            p.money -= source.price;
            source.quantity -= 1;
            if (source.type == "Food")
            {
                bool flag = true;
                for (int i = 0; i < p.inventory.food.Count; ++i)
                {
                    if (source.name == p.inventory.food[i].name)
                    {
                        p.inventory.food[i].quantity += 1;
                        flag = false;
                    }
                }
                if (flag)
                {
                    InventoryObject obj = new InventoryObject(source);
                    obj.quantity = 1;
                    p.inventory.food.Add(new InventoryObject(obj));
                }
                if (source.quantity <= 0)
                {
                    inventoryI.food.Remove(source);
                }
            }
            else if (source.type == "Weapon")
            {
                bool flag = true;
                for (int i = 0; i < p.inventory.weapons.Count; ++i)
                {
                    if (source.name == p.inventory.weapons[i].name)
                    {
                        p.inventory.weapons[i].quantity += 1;
                        flag = false;
                    }
                }
                if (flag)
                {
                    InventoryObject obj = new InventoryObject(source);
                    obj.quantity = 1;
                    p.inventory.weapons.Add(new InventoryObject(obj));
                }
                if (source.quantity <= 0)
                {
                    inventoryI.weapons.Remove(source);
                }
            }
            FillSellShop();
            FillBuyShop();
        }
        else
        {
            // MESSAGE D'ERREUR
        }
    }

    public void Sell(InventoryObject source)
    {
        Player p = PlayerManager.GetInstance().player;
        Island isl = IslandManager.GetInstance().islands[currentIsland];
        p.money += source.price;
        source.quantity -= 1;
        if (source.type == "Food")
        {
            bool flag = true;
            for (int i = 0; i < isl.inventory.food.Count; ++i)
            {
                if (source.name == isl.inventory.food[i].name)
                {
                    isl.inventory.food[i].quantity += 1;
                    flag = false;
                }
            }
            if (flag)
            {
                InventoryObject obj = new InventoryObject(source);
                obj.quantity = 1;
                isl.inventory.food.Add(new InventoryObject(obj));
            }
            if (source.quantity <= 0)
            {
                inventoryP.food.Remove(source);
            }
        }
        else if (source.type == "Weapon")
        {
            bool flag = true;
            for (int i = 0; i < isl.inventory.weapons.Count; ++i)
            {
                if (source.name == isl.inventory.weapons[i].name)
                {
                    isl.inventory.weapons[i].quantity += 1;
                    flag = false;
                }
            }
            if (flag)
            {
                InventoryObject obj = new InventoryObject(source);
                obj.quantity = 1;
                isl.inventory.weapons.Add(new InventoryObject(obj));
            }
            if (source.quantity <= 0)
            {
                inventoryP.weapons.Remove(source);
            }
        }
        FillBuyShop();
        FillSellShop();
    }

// Update is called once per frame
void Update()
{

}
}
