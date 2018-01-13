using UnityEngine;
using System.Collections;

public class Exit : MonoBehaviour {

    public GameManager gm;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void OnMouseDown()
    {
        gm.GoIntroMenu();
    }
}
