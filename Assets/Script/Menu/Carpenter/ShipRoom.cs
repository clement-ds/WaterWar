using UnityEngine;
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
		case "Infirmary":
			Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Infirmary");
			break;
		case "CanonBall":
			Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/CanonBall");
			break;
		case "Alcohol":
			Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Alcohol");
			break;
		case "PetitCanon":
			btn.GetComponent<Image>().color = Color.red;
			Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/PetitCanon");
			break;
		case "GunPowder":
			Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/GunPowder");
			break;
		case "Canteen":
			Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/poulet");
			break;
		case "Wheel":
			Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Wheel");
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


		switch (source.type) {
		case "defenseBody":
			btn.GetComponent<Image> ().color = Color.green;
			break;
		case "blockBody":
			btn.GetComponent<Image> ().color = Color.white;
			break;
		case "attackBody":
			btn.GetComponent<Image> ().color = Color.red;
			break;
		default:
			btn.GetComponent<Image> ().color = Color.white;
			break;
		}
		setIcon(source.component);
		print (source.component);
	}

	public void setRoom(Room room) {
	}

	void Update() {
	}
}
