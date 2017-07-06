using UnityEngine;
using System.Collections;

public class ColliderClosed : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerExit(Collider other)
    {
        print("exit: " + other.transform.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        print("enter: " + other.transform.gameObject);
    }
}
