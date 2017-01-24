using UnityEngine;
using System.Collections;

public class HasLife : MonoBehaviour {
    protected int life = 100;

    // Use this for initialization
    void Start () {
    }
    
    // Update is called once per frame
    protected void Update () {
        if (life <= 0) {
            GetComponentInChildren<Canon>().destroyCanon();
            Destroy(GetComponentInChildren<Canon>());
            print("Destroyed !");
        }
    }

    public int getLife() {
        return life;
    }

    public void setLife(int newLife) {
        life = newLife;
    }

}
