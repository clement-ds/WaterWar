using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CheckDistanceBetweenObjects : MonoBehaviour
{

    private GameObject obj1 = null;
    private GameObject obj2 = null;
    public Text distance;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (obj1 != null && obj2 != null)
            distance.text = Vector3.Distance(obj1.transform.position, obj2.transform.position).ToString("F1");
    }

    public void init(GameObject obj1, GameObject obj2)
    {
        Debug.Log("INIT DISTANCE");
        this.obj1 = obj1;
        this.obj2 = obj2;
    }
}
