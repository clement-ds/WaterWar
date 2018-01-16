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
		case "defenseBody":
		Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Images/Rhum");
        break;
      case "blockBody":
        Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Fish");
        break;
      case "attackBody":
        Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Images/Black powder");
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
    setIcon(source.type);
  }

  public void setRoom(Room room) {
    Title.text = source.type;
    Description.text = source.component.ToString();
  }

  void Update() {
  }
}
