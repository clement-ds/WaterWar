﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneratorLauncher : MonoBehaviour {

	// Use this for initialization
	void Start () {
		MapGenerator mg = new MapGenerator();
 		mg.spawnMap();
 		mg.displayMap(gameObject);

	}
	
}