using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ListItemController : MonoBehaviour {

  public Image Icon;
  public Text Count, Name, Price;
  private bool toExchange = false;

  private Button btn;

	// Use this for initialization
	void Start () {
    btn = this.GetComponent<Button>();
    btn.onClick.AddListener(TaskOnClick);
	}
	
	// Update is called once per frame
	void Update () {
	}

  void TaskOnClick() {
    toExchange = true;
    Debug.Log("Click ! ");
  }

  bool getExchange() {
    return toExchange;
  }
}
