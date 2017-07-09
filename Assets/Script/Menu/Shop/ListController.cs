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

    // Use this for initialization
    void Start()
    {

        // For player shop
        //if (name == "SellPanel") {
        PlayerManager managerP = PlayerManager.GetInstance();
        inventoryP = managerP.player.inventory;
        FillSellShop();
        //}

        currentIsland = PlayerManager.GetInstance().player.currentIsland;

        // For island shop
        //if (name == "BuyPanel") {
        IslandManager managerI = IslandManager.GetInstance();

        inventoryI = managerI.islands[currentIsland].inventory;
        FillBuyShop();
        //}
    }

    void FillBuyShop()
    {
        for (int i = 0; i < inventoryI.food.Count; ++i)
        {
            GameObject newItem = Instantiate(ListItemPrefab) as GameObject;
            ListItemController controller = newItem.GetComponent<ListItemController>();

            controller.lc = this;
            controller.source = inventoryI.food[i];

            controller.InitBuyCell();
            newItem.transform.SetParent(ContentPanelShop.transform);
            newItem.transform.localScale = Vector3.one;
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
        }
    }

    void FillSellShop()
    {
        for (int i = 0; i < inventoryP.food.Count; ++i)
        {
            GameObject newItem = Instantiate(ListItemPrefab) as GameObject;
            ListItemController controller = newItem.GetComponent<ListItemController>();

            controller.lc = this;
            controller.source = inventoryP.food[i];

            controller.InitSellCell();
            newItem.transform.SetParent(ContentPanelPlayer.transform);
            newItem.transform.localScale = Vector3.one;
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
        }
    }

    public void Buy(InventoryObject source)
    {
        Player p = PlayerManager.GetInstance().player;
        if (p.money >= source.price)
        {
            source.quantity -= 1;

        } else
        {
            // MESSAGE D'ERREUR
        }
            
        
    }

    public void Sell(InventoryObject source)
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
