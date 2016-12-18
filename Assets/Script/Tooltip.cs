using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour {
    // Use this for initialization
    public SetAsCanonOnClick canon;

    void Start()
    {
        this.gameObject.SetActive(false);
    }

    public void show (GameObject obj) {
        this.gameObject.SetActive(true);

        float xPos = obj.transform.position.x + 42;
        float yPos = obj.transform.position.y - obj.GetComponent<RectTransform>().sizeDelta.y - 42;

        canon = obj.GetComponent<SetAsCanonOnClick>();
        //this.transform.position = new Vector2(xPos, yPos);
    }
	
	// Update is called once per frame
	public void hide () {
        this.gameObject.SetActive(false);
	}
}
