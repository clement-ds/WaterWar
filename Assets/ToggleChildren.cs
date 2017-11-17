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


    public GameObject obj, obj2;
    public void ToggleVisibility()
    {
        obj.SetActive(!obj.activeSelf);
        obj2.SetActive(!obj2.activeSelf);

    }
}
