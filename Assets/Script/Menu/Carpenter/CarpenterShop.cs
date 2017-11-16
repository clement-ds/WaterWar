using UnityEngine;
using System.Collections;

public class CarpenterShop1 : MonoBehaviour {
  public string current;

  // Use this for initialization
  void Start() {
      this.current = "Untouched";
  }

  public void setCurrent(string room) {
    current = room;
  }
}
