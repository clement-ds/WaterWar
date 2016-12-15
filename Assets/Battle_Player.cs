using UnityEngine;
using System.Collections;

public class Battle_Player : MonoBehaviour {

	public int life = 100;
    public int position = 2;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public int getCurrentLife()
    {
        return life;
    }
}
