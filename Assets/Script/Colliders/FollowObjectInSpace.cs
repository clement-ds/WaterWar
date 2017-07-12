using UnityEngine;
using System.Collections;

public class FollowObjectInSpace : MonoBehaviour {

    public GameObject target;
    public Vector3 offset;

	// Use this for initialization
	void Start () {
        this.offset = this.transform.position - this.target.transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        this.transform.position = this.target.transform.position + offset;
    }

}
