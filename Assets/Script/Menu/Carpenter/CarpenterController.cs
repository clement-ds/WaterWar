using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarpenterController : MonoBehaviour {

	private PlayerShip ship;
	private ShipDisposition shipD;
	public GameObject carpenterShopPanel;
	public GameObject TypeRoomCardPrefab;

	private List<GameObject> typeRoomList = new List<GameObject>();

	// Use this for initialization
	void Start () {
		PlayerManager manager = PlayerManager.GetInstance();
		ship = manager.player.ship;
		fillRoomList();
	}


    void fillRoomList() {
      for (int i = 0; i < typeRoomList.Count; ++i) {
          Destroy(typeRoomList[i]);
      }

      for (int i = 0; i < ship.shipDisposition.rooms.Count; ++i) {
          GameObject newCard = Instantiate(TypeRoomCardPrefab) as GameObject;
          TypeRoomCard card = newCard.GetComponent<TypeRoomCard>();

          card.source = ship.shipDisposition.rooms[i];

          card.initCard();
          newCard.transform.SetParent(carpenterShopPanel.transform);
          newCard.transform.localScale = Vector3.one;
          typeRoomList.Add(newCard);
      }
    }

	// Update is called once per frame
	void Update () {
	}

/*	public void placeRoom(int index, string type) {
		shipD.ChangeRoom(index, type);
		setIcon(index, type);
	}

	// Index + 1 technically
	public void placeRoomAsCurrent(int index) {
		string current = GameObject.Find("CarpenterShop").GetComponent<CarpenterShop>().current;

		if (current != "Untouched") {
			placeRoom(index - 1, current);
		}
	}*/
}
