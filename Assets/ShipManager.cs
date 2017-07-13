using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShipManager : MonoBehaviour {

    private ShipDisposition shipD;
    private PlayerManager managerP;

    void Start () {
        managerP = PlayerManager.GetInstance();
        shipD = managerP.player.ship;

        for (int i = 0; i < shipD.rooms.Count; ++i)
        {
            setIcon(i, shipD.rooms[i].type);
        }
    }

    void setIcon(int index, string type)
    {
        UnityEngine.UI.Image Icon = GameObject.Find("Zone " + (index + 1).ToString()).transform.GetChild(0).GetChild(0).GetComponent<Image>();
        switch (type)
        {
            case "Food":
                Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Apple");
                break;
            case "Fish":
                Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Images/Fish");
                break;
            case "Powder":
                Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Images/BlackPowder");
                break;
            default:
                Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Images/Spider Web");
                break;
        }
       
    }

    public void placeRoom(int index, string type)
    {
        shipD.ChangeRoom(index, type);
        setIcon(index, type);
    }

    public void placeRoomAsCurrent(int index) // Index + 1 technically
    {
            string current = GameObject.Find("CarpenterShop").GetComponent<CarpenterShop>().current;
            if (current != "Untouched")
            {
                placeRoom(index - 1, current);
            }
    }
}
