using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneratorLauncher : MonoBehaviour {

	// Use this for initialization
	void Start () {
 		GameManager.Instance.displayMap(gameObject);
        GameManager.Instance.spawnShips(gameObject);
    }
	
}
