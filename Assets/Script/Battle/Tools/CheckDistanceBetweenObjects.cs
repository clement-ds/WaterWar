using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CheckDistanceBetweenObjects : MonoBehaviour
{

    private GameObject obj1 = null;
    private GameObject obj2 = null;
    public Text distance;

    private Battle_Ship ship1 = null;
    private Battle_Ship ship2 = null;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (obj1 != null && obj2 != null)
        {
            distance.text = Vector3.Distance(obj1.transform.position, obj2.transform.position).ToString("F1");

            if (ship1 != null && ship2 != null)
            {
                ship1.distanceWith(ship2, float.Parse(distance.text));
                ship2.distanceWith(ship1, float.Parse(distance.text));
            }
        }
    }

    public void init(GameObject obj1, GameObject obj2)
    {
        this.obj1 = obj1;
        this.obj2 = obj2;

        this.ship1 = this.obj1.GetComponent<Battle_Ship>();
        this.ship2 = this.obj2.GetComponent<Battle_Ship>();
    }

    public GameObject getFirst()
    {
        return this.obj1;
    }

    public GameObject getSecond()
    {
        return this.obj2;
    }
}
