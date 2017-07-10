using UnityEngine;
using System.Collections;

public class Battle_CanonBall : MonoBehaviour {

    Ammunition ammunition;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
    }

    public void initialize(Ammunition ammunition)
    {
        this.ammunition = ammunition;
    }

    public Ammunition getAmmunition()
    {
        return this.ammunition;
    }
}
