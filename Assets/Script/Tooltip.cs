using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour {
    // Use this for initialization
    public SetAsCanonOnClick canon;
    public GameObject next = null;
    private bool clicked = false;
    private GameObject visibility;

    void Start()
    {
        visibility = this.transform.GetChild(0).gameObject;
        visibility.SetActive(false);
    }

    private void show (GameObject obj) {
        visibility.SetActive(true);

        canon = obj.GetComponent<SetAsCanonOnClick>();

        Debug.Log(obj.transform.position);

         float xPos = obj.transform.position.x / 4.5f * (679 / 2) + 10;
         float yPos = obj.transform.position.y / 9.36450662739f * (1413 / 2) + 10; //SA MERE LA PUUUUUUUTTTE

         this.transform.localPosition = new Vector3(xPos + 42, yPos - 42, 0);
    }
	
    public void showOn(GameObject obj)
    {
        next = obj;
    }

    // Update is called once per frame
    public void hideOn(GameObject obj)
    {
        if (next == obj)
        {
            next = null;
        }
    }

    public void hide()
    {
        visibility.SetActive(false);
    }

    void Update()
    {
        if (clicked == true)
        {
            if (next != null)
            {
                show(next);
            }
            else
            {
                hide();
            }
            next = null;
            clicked = false;
        }
        if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0))
        {
            clicked = true;
        }
    }
}
