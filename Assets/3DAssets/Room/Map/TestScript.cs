using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TestScript : MonoBehaviour {
    public TextMeshProUGUI text;

    void OnMouseDown() {
        Debug.Log("Clicked: OnMouseDown");
        text.SetText(text.text == "Button" ? "CLICKED BIATCH" : "Button");
    }
    public void Clicked()
    {
        Debug.Log("YOU CLICKED ME :O");
    }
}
