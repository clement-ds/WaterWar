using UnityEngine;
using System.Collections;

public class Island1 : MonoBehaviour {

    public GameObject dockingTrigger;
    public GameObject playerShip;

    private Rigidbody2D playerRb2d;

	// Use this for initialization
	void Start () {
        playerRb2d = playerShip.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown()
    {
        //print("dockingtrigger : " + dockingTrigger.transform.localPosition);
        //Vector2 dock;
        //dock.x = dockingTrigger.transform.localPosition.x;
        //dock.y = dockingTrigger.transform.localPosition.y;
        //print("dock : " + dock);
        //playerRb2d.MovePosition(dock);
        playerShip.transform.position = Vector3.MoveTowards(playerShip.transform.position, dockingTrigger.transform.position, 10000);
    }

}
