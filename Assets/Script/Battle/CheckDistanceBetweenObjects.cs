using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CheckDistanceBetweenObjects : MonoBehaviour {

    public GameObject obj1;
    public GameObject obj2;
    public Text distance;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        distance.text = Vector3.Distance(obj1.transform.position, obj2.transform.position).ToString("F1");
	}
}
