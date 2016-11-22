using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {
    public float speed = 1.5f;
    private Vector3 target;
    public DetectClick other;

    void Start()
    {
        //target = transform.position;
    }

    void Update()
    {
        //if (Input.GetMouseButtonDown(0) && other.getInside())
        //{
        //    target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    target.z = transform.position.z;
        //}
        //transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        print("JDIDIDIDIDI");
    }
}

