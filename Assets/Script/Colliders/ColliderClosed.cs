using UnityEngine;
using System.Collections;

public class ColliderClosed : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerExit2D(Collider2D other)
	{
		print("exit: " + other.transform.gameObject);
        other.transform.gameObject.GetComponent<Battle_CrewMember>().stopMove();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		print("enter: " + other.transform.gameObject);
	}
}
