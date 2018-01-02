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
      case "Food":
        Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Apple");
        break;
      case "Fish":
        Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Images/Fish");
        break;
      case "Powder":
        Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Images/BlackPowder");
        break;
      default:
        Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Images/Spider Web");
        break;
    }
  }

  public void initCard(CarpenterController cl) {
    Title.text = source.component;
    Description.text = source.component.ToString();

    btn = this.GetComponent<Button>();
    btn.onClick.AddListener(delegate { cl.printType(source); });
    setIcon(source.component);
  }

  public void setRoom(Room room) {
    Title.text = source.component;
    Description.text = source.component.ToString();
  }

  void Update() {
  }
}
