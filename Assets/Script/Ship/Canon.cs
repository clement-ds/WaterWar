using UnityEngine;
using System.Collections;

public class Canon : HasLife {
    private int power = 5;       // Random number
    private int damage = 5;      // Random number
    private int viewFinder = 0;  // Random number

    // Use this for initialization
    void Start () {
        life = 50;
    }

    public void destroyCanon() {
        if (GetComponent<SpriteRenderer>())
            Destroy(GetComponent<SpriteRenderer>());
        if (GetComponent<Target>())
            Destroy(GetComponent<Target>());
    }

    // Update is called once per frame
   	void Update () {
        base.Update();
    }

    string getName() {
        return name;
    }

    int getPower() {
        return power;
    }

    int getDamage() {
        return damage;
    }

    int getViewFinder() {
        return viewFinder;
    }
}
