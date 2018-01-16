using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TypeRoomCard : MonoBehaviour {
  public Image Icon;
  public Text Title;
  public Text Description;
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
    Title.text = source.component;
    Description.text = source.type.ToString();

    btn = this.GetComponent<Button>();
    btn.onClick.AddListener(delegate { cl.printType(source); });
    setIcon(source.component);
  }

  public void setRoom(Room room) {
    Title.text = source.type;
    Description.text = source.component.ToString();
  }

  void Update() {
  }
}
