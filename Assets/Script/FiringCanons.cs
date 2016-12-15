using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class FiringCanons : MonoBehaviour {
    public GameObject MainCanon { get; set; }

    void Start()
    {
        MainCanon = null;
    }

    // Use this for initialization
    public void noCanon()
    {
        MainCanon = null;
    }

    public void fireOn(GameObject target)
    {
        print("Hellloo");
        if (MainCanon != null)
        {
            print("Canon " + MainCanon.name + " fires on " + target.name);
        }
          
        else
            print("No Canon");
    }
}

