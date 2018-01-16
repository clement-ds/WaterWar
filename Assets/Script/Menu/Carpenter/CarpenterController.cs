﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarpenterController : UIController {

	private PlayerShip ship;
	public GameObject TypeRoomCardPrefab;
	public GameObject ShipRoomPrefab;
	public GameObject CarpenterShip;
	public GameObject CarpenterShop;

	private List<GameObject> typeRoomList = new List<GameObject>();
	private List<GameObject> ShipRoomList = new List<GameObject>();

	private Room selectedRoom;

	// Use this for initialization
	void Start () {
		PlayerManager manager = PlayerManager.GetInstance();
		this.ship = manager.player.ship;
	}

	public override void Populate ()
	{
		FillShopRoomList();
		FillShipRoomList();	
	}

	void FillShipRoomList() {
		for (int i = 0; i < this.ShipRoomList.Count; ++i) {
			Destroy(ShipRoomList[i]);
		}

		for (int i = 0; i < this.ship.shipDisposition.rooms.Count; ++i) {
			if (this.ship.shipDisposition.rooms[i].type != "blockBody") {
			GameObject newRoom = Instantiate(ShipRoomPrefab) as GameObject;
			ShipRoom card = newRoom.GetComponent<ShipRoom>();

			card.source = 
					this.ship.shipDisposition.rooms[i];

			card.initCard(this);
			newRoom.transform.SetParent(this.CarpenterShip.transform);
			newRoom.transform.localScale = Vector3.one;
			newRoom.transform.localPosition = new Vector3((int)((card.source.x - 250) * 1.7), (int)((card.source.y - 55) * 1.5), 0);
			
			RectTransform rt = (RectTransform)newRoom.transform;
			rt.sizeDelta = new Vector2(card.source.width, card.source.height);

			this.ShipRoomList.Add(newRoom);
			}
		}
	}

  void FillShopRoomList() {
    for (int i = 0; i < this.typeRoomList.Count; ++i) {
        Destroy(typeRoomList[i]);
    }

    for (int i = 0; i < this.ship.shipDisposition.rooms.Count; ++i) {
			if (this.ship.shipDisposition.rooms [i].component != "") {
				
				GameObject newCard = Instantiate (TypeRoomCardPrefab) as GameObject;
				TypeRoomCard card = newCard.GetComponent<TypeRoomCard> ();

				card.source = this.ship.shipDisposition.rooms[i];

				card.initCard (this);
				newCard.transform.SetParent (this.CarpenterShop.transform);
				newCard.transform.localScale = Vector3.one;
				newCard.transform.localPosition = Vector3.one;
				this.typeRoomList.Add (newCard);
			}
    }
  }

	public void printType(Room room) {
		Debug.Log(room.component);
		this.selectedRoom = room;
	}

	public void selectRoom(Room room) {
		this.selectedRoom = room;
		print ("Selected room is now " + this.selectedRoom.component);
	}

	public void setRoom (GameObject sr) {
		print ("Setting room to " + this.selectedRoom.component);

			if (selectedRoom.type != "blockBody") {
				GameObject newRoom = Instantiate(ShipRoomPrefab) as GameObject;
				ShipRoom card = newRoom.GetComponent<ShipRoom>();

			card.source = sr.GetComponent<ShipRoom>().source;
			card.source.type = selectedRoom.type;
			card.source.component = selectedRoom.component;

				card.initCard(this);
				newRoom.transform.SetParent(this.CarpenterShip.transform);
				newRoom.transform.localScale = Vector3.one;
				newRoom.transform.localPosition = new Vector3((int)((card.source.x - 250) * 1.7), (int)((card.source.y - 55) * 1.5), 0);

				RectTransform rt = (RectTransform)newRoom.transform;
				rt.sizeDelta = new Vector2(card.source.width, card.source.height);

				this.ShipRoomList.Add(newRoom);
				Destroy(sr);
			}
		}
}
