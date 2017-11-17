using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShipManager : MonoBehaviour {

  private ShipDisposition shipD;
  private PlayerManager managerP;
  public GameObject room;

  void Start () {
    managerP = PlayerManager.GetInstance();
    shipD = managerP.player.ship.shipDisposition;

       for (int i = 0; i < shipD.rooms.Count; ++i) {
           GameObject newRoom = Instantiate(room) as GameObject;
           newRoom.transform.SetParent(this.transform);
           newRoom.transform.localScale = Vector3.one;

           newRoom.transform.localEulerAngles = new Vector3(0, 0, shipD.rooms[i].rotation);
           newRoom.transform.localPosition = new Vector3(shipD.rooms[i].x * 100, shipD.rooms[i].y * 100, shipD.rooms[i].z);
           newRoom.name = shipD.rooms[i].type;
           Debug.Log("Spawn at position: " + shipD.rooms[i].x + " " + shipD.rooms[i].y);
           setIconFromRoom(newRoom); 
           }
  }

  public void print() {
    Debug.Log("LOL");
  }

  void Update() {
  }

    void setIconFromRoom(GameObject room)
    {
        UnityEngine.UI.Image Icon = room.GetComponentInChildren<Image>();

        switch (room.name)
        {
            case "Food":
                Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Apple");
                break;
            case "Fish":
                Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Images/Fish");
                break;
            case "PetitCanon":
                Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Petit_canon");
                break;
            case "Powder":
                Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Images/BlackPowder");
                break;
            default:
                Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Images/Spider Web");
                break;
        }
    }

    void setIcon(int index, string type)
  {
    UnityEngine.UI.Image Icon = GameObject.Find("Zone " + (index + 1).ToString()).transform.GetChild(0).GetChild(0).GetComponent<Image>();

    switch (type) {
      case "Food":
        Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Apple");
        break;
      case "Fish":
        Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Images/Fish");
        break;
      case "PetitCanon":
        Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Petit_canon");
        break;
      case "Powder":
        Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Images/BlackPowder");
        break;
      default:
        Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Images/Spider Web");
        break;
    }
  }

  public void placeRoom(int index, string type) {
    shipD.ChangeRoom(index, type);
    setIcon(index, type);
  }

  // Index + 1 technically
  public void placeRoomAsCurrent(int index) {
    string current = GameObject.Find("CarpenterShop").GetComponent<CarpenterShop>().current;

    if (current != "Untouched") {
      placeRoom(index - 1, current);
    }
  }
}
