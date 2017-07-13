using UnityEngine;
using System.Collections;

public class unit : MonoBehaviour {

    public GameObject Selected;
    public GameObject Unselected;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown()
    {
        Vector3 clickedPosition = Input.mousePosition;
        print("CLICK : " + clickedPosition);
        Selected.SetActive(Unselected.activeSelf);
        Unselected.SetActive(!Selected.activeSelf);
    }

}
