using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour {

	// Use this for initialization
	public void show (GameObject obj) {
        this.gameObject.SetActive(true);
        Text tooltipText = GameObject.Find("TooltipText").GetComponent<Text>();
        Text tooltipSize = GameObject.Find("TooltipSize").GetComponent<Text>();

        tooltipText.text = getTooltip();
        tooltipSize.text = getTooltip();

        float xPos = obj.transform.position.x + 42;
        float yPos = obj.transform.position.y - obj.GetComponent<RectTransform>().sizeDelta.y - 42;

        this.transform.position = new Vector2(xPos, yPos);
    }
	
	// Update is called once per frame
	public void hide () {
        this.gameObject.SetActive(false);
	}

    string getTooltip()
    {
        return ("Coucou!!");
    }
}
