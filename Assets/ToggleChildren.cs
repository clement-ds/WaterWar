using UnityEngine;
using System.Collections;

public class ToggleChildren : MonoBehaviour {
    public void ToggleAllChildren()
    {
        bool activate = this.gameObject.activeSelf == false;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(activate);
        }
        this.gameObject.SetActive(activate);
    }
}
