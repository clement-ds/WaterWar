﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileClick : MonoBehaviour {

    public int islandID;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseDown()
    {
        print(islandID);
        //PlayerManager.GetInstance().player.currentIsland = int.Parse(tag);
        //gm.GoInteraction();
    }
}