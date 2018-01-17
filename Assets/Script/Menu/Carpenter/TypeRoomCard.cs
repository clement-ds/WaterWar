using UnityEngine;
using UnityEngine.UI;
using System.Collections;



public class TypeRoomCard : MonoBehaviour {
  public Image Icon;
  public Text Title;
  public Text Description;
	public Text Price;

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

	public void initCard(CarpenterController cl, string title, string desc, int price) {
    Title.text = title;
    Description.text = desc;
	Price.text = price.ToString();
	setIcon(title);

    btn = this.GetComponentInChildren<Button>();
		btn.onClick.AddListener(delegate { cl.selectRoom(this.Title.text, this.Description.text, int.Parse(this.Price.text)); });
  }

/*  public void setRoom(Room room) {
    Title.text = source.type;
    Description.text = source.component.ToString();
  }*/
}
