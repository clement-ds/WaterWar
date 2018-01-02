using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarpenterController : MonoBehaviour {

	private PlayerShip ship;
	public GameObject TypeRoomCardPrefab;

	private List<GameObject> typeRoomList = new List<GameObject>();

	private Room selectedRoom;

	// Use this for initialization
	void Start () {
		PlayerManager manager = PlayerManager.GetInstance();
		this.ship = manager.player.ship;
		FillRoomList();
	}


  void FillRoomList() {
    for (int i = 0; i < this.typeRoomList.Count; ++i) {
        Destroy(typeRoomList[i]);
    }

    for (int i = 0; i < this.ship.shipDisposition.rooms.Count; ++i) {
        GameObject newCard = Instantiate(TypeRoomCardPrefab) as GameObject;
        TypeRoomCard card = newCard.GetComponent<TypeRoomCard>();

		card.source = this.ship.shipDisposition.rooms[i];

        card.initCard(this);
        newCard.transform.SetParent(this.transform);
            newCard.transform.localScale = Vector3.one;
            newCard.transform.localPosition = Vector3.one;
        this.typeRoomList.Add(newCard);
    }
  }

	public void printType(Room room) {
		Debug.Log(room.component);
		this.selectedRoom = room;
	}

	public Room getSelectedRoom() {
		return this.selectedRoom;
	}

	void Update () {
	}
}
