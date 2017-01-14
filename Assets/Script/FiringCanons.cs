using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class FiringCanons : MonoBehaviour {
    public GameObject MainCanon { get; set; }

    void Start() {
        MainCanon = null;
    }

    // Use this for initialization
    public void noCanon() {
        MainCanon = null;
    }

    public void fireOn(GameObject target) {
        if (MainCanon != null && MainCanon.GetComponent<Cooldown>().getPossibility() == true) {
            Battle_Enemy enemy = target.GetComponentInParent<Battle_Enemy>();
            print("Canon " + MainCanon.name + " fires on " + target.name + " with boulet " + MainCanon.GetComponent<SetAsCanonOnClick>().bouletname);
            if (enemy != null) {
                enemy.setCurrentLife(enemy.getCurrentLife() - 20);
                print("Aouch we loose 20 pv");
            }
        } else
            print("No Canon");
    }
}

