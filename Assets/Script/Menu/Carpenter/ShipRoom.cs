﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShipRoom : MonoBehaviour {
	public Image Icon;
	public Room source;

	public Button btn;

	void Start () {
	}

	void setIcon(string type) {
		switch (type) {
		case "defenseBody":
			Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Apple");
			break;
		case "blockBody":
			Icon.GetComponent<Image>().sprite = Resources.Load<Sprite> ("Sprites/Images/Fish");
			break;
		case "attackBody":
			btn.GetComponent<Image>().color = Color.red;
			Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Images/BlackPowder");
			break;
		default:
			Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Images/Spider Web");
			break;
		}
	}

	public void initCard(CarpenterController cl) {
		btn = this.GetComponentInChildren<Button>();
		Icon = this.btn.GetComponentsInChildren<Image>()[1];
		btn.onClick.AddListener(delegate { cl.printType(source); });
		setIcon(source.type);
	}

	public void setRoom(Room room) {
	}

	void Update() {
	}
}