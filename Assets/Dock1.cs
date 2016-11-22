using UnityEngine;
using System.Collections;

public class Dock1 : MonoBehaviour {

    public GameManager gameManager;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        print("HEELLOOOOOO");
        gameManager.GoInteraction();
    }

    void OnMouseDown()
    {
        print("onmousedownDOCK");
    }

}
