using UnityEngine;
using System.Collections;

public class testManager : MonoBehaviour {

    PlayerManager pm = null;

	// Use this for initialization
	void Start () {
        pm = PlayerManager.GetInstance();    
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
