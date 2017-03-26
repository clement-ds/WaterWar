using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BoatSetup : MonoBehaviour {

    public GameObject pCanon;
    public List<GameObject> canons = new List<GameObject>();
    // Use this for initialization
    void Start () {
        addCanon(new Vector2(0.21f, -0.6f));
        addCanon(new Vector2(-0.7f, -0.6f));
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void addCanon(Vector2 pos)
    {
        canons.Add(Instantiate(pCanon));
        canons[canons.Count - 1].transform.SetParent(this.gameObject.transform);
        canons[canons.Count - 1].transform.localPosition = pos;
        canons[canons.Count - 1].transform.localRotation = new Quaternion(0.0f, 0.0f, 180.0f, 0);
    }
}
