using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TypeRoomCard : MonoBehaviour {
  public Image Icon;
  public Text Title;
  public Text Description;
  public Room source;

  private Button btn;

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

  public void initCard() {
    Title.text = source.type.ToString();
    Description.text = source.type.ToString();

    btn = this.GetComponent<Button>();
//    btn.onClick.AddListener(delegate { lc.Buy(source); });
    setIcon(source.type);
  }

  void Update() {
  }
}
