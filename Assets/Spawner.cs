﻿using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	public Sprite coconutSprite = null;
	private GameObject coconut = null;
    public GameObject[] coconuts = new GameObject[9];

	// Use this for initialization
	void Start () {
		coconut = new GameObject();
		coconut.AddComponent<SpriteRenderer>().sprite = coconutSprite;
        coconut.name = "Coconut";

		for (int i = 0; i < 9; i++) {
			Vector3 position = new Vector3(Random.Range(0.0f, 1600.0f), Random.Range(0.0f, -1600.0f), 0);
			GameObject newCoconut = (GameObject)Instantiate(coconut, position, new Quaternion());
			newCoconut.name = "Coconut[" + i + "]";
			newCoconut.transform.localScale = new Vector3(33, 33, 0);
            coconuts[i] = newCoconut;
        }
    }

	// Update is called once per frame
	void update () {
	}
}
