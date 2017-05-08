using UnityEngine;
using System.Collections;

public class testManager : MonoBehaviour {

    PlayerManager pm = null;
    IslandManager im = null;

	// Use this for initialization
	void Start () {
        //pm = PlayerManager.GetInstance();
        im = IslandManager.GetInstance();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
