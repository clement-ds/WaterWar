using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShipRoom : MonoBehaviour {
	public Image Icon;
	public Room source;
	public CarpenterController cl;

	public Button btn;

	void Start () {
	}

	void setIcon(string type) {
		switch (type) {
		case "Infirmary":
			Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Infirmary");
			break;
		case "Canonball":
			Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Canonball");
			break;
		case "Alcohol":
			Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Alcohol");
			break;
		case "PetitCanon":
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
		this.cl = cl;
		btn = this.GetComponentInChildren<Button>();
	 	btn.onClick.AddListener(setRoom);
		
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

		Icon = this.btn.GetComponentsInChildren<Image>()[1];
		setIcon(source.component);
	}

	public void setRoom() {
		this.cl.setRoom(this.gameObject);
	}
}
