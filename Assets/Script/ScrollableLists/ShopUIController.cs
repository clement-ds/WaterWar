using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using TMPro;


// works with ListPanelRoot/ListReceiverPanel/ListRowPanel
public class ShopUIController : UIController
{
    private IslandInventory inventoryI;
    private PlayerInventory inventoryP;

    private List<InventoryObject> shopTrade;
    private List<InventoryObject> selfTrade;
    private int transactionPrice = 0;



    [Header("Player panels")]
    public GameObject RootSelf;
    public GameObject RowPrefabSelf;
    public Transform ReceiverSelf;
    
    [Space(20)]
    [Header("Shop Trade panels")]
    public GameObject RootShopTrade;
    public Transform ReceiverShopTrade;
   
    [Header("Self Trade panels")]
    public GameObject RootSelfTrade;
    public Transform ReceiverSelfTrade;
    
    [Header("Trade Misc")]
    public GameObject PricePanel;
    public TextMeshProUGUI TransactionPriceText;

    public void Start() {
        shopTrade = new List<InventoryObject>();
        selfTrade = new List<InventoryObject>();
    }

    public override void Populate()
    {
        base.Populate();
        FillItems();
    }

    public override void TogglePanel() {
        bool activeState = !rootPanel.activeSelf;
        rootPanel.SetActive(activeState);
        RootSelf.SetActive(activeState);
        RootSelfTrade.SetActive(activeState);
        RootShopTrade.SetActive(activeState);
        PricePanel.SetActive(activeState);
        
        panel.gameObject.SetActive(activeState);
        if (activeState)
            Populate();
 
    }

    protected override void ClearPanel()
    {
        base.ClearPanel();
        foreach (Transform child in ReceiverSelf)
        {
            if (child != panel.transform)
                GameObject.Destroy(child.gameObject);
        }
        foreach (Transform child in ReceiverSelfTrade)
        {
            if (child != panel.transform)
                GameObject.Destroy(child.gameObject);
        }
        foreach (Transform child in ReceiverShopTrade)
        {
            if (child != panel.transform)
                GameObject.Destroy(child.gameObject);
        }
    }

    private Sprite loadImage(String name)
    {
        Sprite sprite = Resources.Load<Sprite>("Sprites/Images/" + name);
        if (sprite == null)
            sprite = Resources.Load<Sprite>("Sprites/Images/Spider Web");
        return sprite;
    }

    private void FillList(GameObject row, Transform panel, List<InventoryObject> objects, Action<InventoryObject> callback) {
        foreach (InventoryObject item in objects)
        {
            GameObject itemRow = (GameObject)GameObject.Instantiate(row);

            foreach (Transform child in itemRow.transform)
            {
                if (child.name == "memberImage") {
                    Image img = (Image)child.GetComponent<Image>();
                    img.sprite = loadImage(item.name);
                }
                if (child.name == "memberQuantity") {
                    TextMeshProUGUI text = (TextMeshProUGUI)child.GetComponent<TextMeshProUGUI>();
                    text.SetText("(" + item.quantity + ")");
                }
                if (child.name == "memberName") {
                    TextMeshProUGUI text = (TextMeshProUGUI)child.GetComponent<TextMeshProUGUI>();
                    text.SetText(item.name);
                }
                if (child.name == "memberValue") {
                    TextMeshProUGUI text = (TextMeshProUGUI)child.GetComponent<TextMeshProUGUI>();
                    text.SetText(item.price + "£");
                }
                if (child.name == "AcceptButton") {
                    Button button = (Button)child.GetComponent<Button>();
                    CreateClosureForButton(item, button, callback);
                }
            }
            itemRow.transform.SetParent(panel.transform, false);
        }
    }

    Economy eco = new Economy();

    private void SendToList(List<InventoryObject> previousList, List<InventoryObject> newList, InventoryObject item, bool shouldDelete = true) {
        eco.setInventoryPrices(inventoryI, inventoryP);
        bool isFound = false;
        for (int i = 0; i < newList.Count; i++) {
            if (newList[i].name == item.name) {
                isFound = true;
                newList[i].quantity++;
                break;
            }
        }
        if (!isFound) {
            InventoryObject newObject = new InventoryObject(item);
            newObject.quantity = 1;
            newList.Add(newObject);
        }

        if (shouldDelete) {
            for (int i = 0; i < previousList.Count; i++) {
                if (previousList[i].name == item.name) {
                    previousList[i].quantity--;
                    if (previousList[i].quantity <= 0) {
                        previousList.Remove(item);
                    }
                    break;
                }
            }
        }

        if (shouldDelete) {
            ProcessTradePrice();
            Populate();             
        }
    }

    private void ProcessTradePrice() {
        int myItems = 0;
        int shopItems = 0;
        foreach (InventoryObject obj in selfTrade) {
            myItems += (obj.price * obj.quantity);
        }
        foreach (InventoryObject obj in shopTrade) {
            shopItems += (obj.price * obj.quantity);
        }
        this.transactionPrice = myItems - shopItems;
        this.TransactionPriceText.SetText(this.transactionPrice + "£");
    }

    public void Exchange() {
        if (PlayerManager.GetInstance().player.money >= -this.transactionPrice) {
            foreach (InventoryObject item in this.selfTrade) {
                for (int i = 0; i < item.quantity; i++) {
                    SendToList(selfTrade, item.type == "Weapon" ? inventoryI.weapons : inventoryI.food, item, false);
                }
            }
            foreach (InventoryObject item in this.shopTrade) {
                for (int i = 0; i < item.quantity; i++) {
                    SendToList(shopTrade, item.type == "Weapon" ? inventoryP.weapons : inventoryP.food, item, false);
                }           
            }

            shopTrade.Clear();
            selfTrade.Clear();
            PlayerManager.GetInstance().player.money += transactionPrice;

            transactionPrice = 0;
            TransactionPriceText.SetText(0 + "£");
            TrimList(selfTrade);
            TrimList(shopTrade);
            Populate();
        }
    }

    private void TrimList(List<InventoryObject> list) {
        List<InventoryObject> toDelete = new List<InventoryObject>();

        foreach (InventoryObject item in list) {
            if (item.quantity <= 0) {
                toDelete.Add(item);
            }
        }
        foreach (InventoryObject item in toDelete) {
            list.Remove(item);
        }
        toDelete.Clear();
    }

    private void FillItems()
    {
        inventoryI = IslandManager.GetInstance().islands[PlayerManager.GetInstance().player.currentIsland].inventory;
        inventoryP = PlayerManager.GetInstance().player.inventory;

        FillList(rowPrefab, panel, inventoryI.food, (InventoryObject item) => SendToList(inventoryI.food, shopTrade, item));
        FillList(rowPrefab, panel, inventoryI.weapons, (InventoryObject item) => SendToList(inventoryI.weapons, shopTrade, item));

        FillList(RowPrefabSelf, ReceiverSelf, inventoryP.food, (InventoryObject item) => SendToList(inventoryP.food, selfTrade, item));
        FillList(RowPrefabSelf, ReceiverSelf, inventoryP.weapons, (InventoryObject item) => SendToList(inventoryP.weapons, selfTrade, item));

        FillList(RowPrefabSelf, ReceiverShopTrade, shopTrade, (InventoryObject item) => SendToList(shopTrade, item.type == "Weapon" ? inventoryI.weapons : inventoryI.food, item));
        FillList(rowPrefab, ReceiverSelfTrade, selfTrade, (InventoryObject item) => SendToList(selfTrade, item.type == "Weapon" ? inventoryP.weapons : inventoryP.food, item));
    }

    // Necessary because of unity bug in lambda
    void CreateClosureForButton(InventoryObject item, Button button, Action<InventoryObject> callback)
    {
        button.onClick.AddListener(() => callback(item));
    }
    // -----------------------------------------


}
