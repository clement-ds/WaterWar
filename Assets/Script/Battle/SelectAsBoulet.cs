using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SelectAsBoulet : MonoBehaviour {
    Tooltip tooltip;
    public string bouletname;
    // Use this for initialization
    void Start () {
        tooltip = GameObject.Find("Tooltip").GetComponent<Tooltip>();
    }

    public void setAsBoulet()
    {
       // tooltip.canon.setBoulet(this.gameObject);
        tooltip.hide();
    }
}
 