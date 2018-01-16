using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManageLootLine : MonoBehaviour {
    public Text lootName;
    public Text lootNumber;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void initLootLine(string lootName, int lootNumber)
    {
        this.lootName.text = lootName;
        this.lootNumber.text = lootNumber.ToString();
    }
}
