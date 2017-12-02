using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewOnClick : MonoBehaviour {

    AltFight fm;

	// Use this for initialization
	void Start () {
		fm = GameObject.Find("FightManager").GetComponent<AltFight>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseDown()
    {
        fm.onSelect(this.gameObject);
    }
}
