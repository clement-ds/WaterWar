using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

    public GameObject cameraTarget; // object to look at or follow
    public GameObject player; // player object for moving

    public float smoothTime = 0.1f;    // time for dampen
    public bool cameraFollowX = true; // camera follows on horizontal
    public bool cameraFollowY = true; // camera follows on vertical
    public bool cameraFollowHeight = true; // camera follow CameraTarget object height
    public float cameraHeight = 2.5f; // height of camera adjustable
    public Vector2 velocity; // speed of camera movement

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (cameraFollowX)
        {
            transform.position = new Vector3(Mathf.SmoothDamp(transform.position.x, cameraTarget.transform.position.x, ref velocity.x, smoothTime), transform.position.y, transform.position.z);
        }
        if (cameraFollowY)
        {
            transform.position = new Vector3(transform.position.x, Mathf.SmoothDamp(transform.position.y, cameraTarget.transform.position.y, ref velocity.y, smoothTime), transform.position.z);
        }
        if (!cameraFollowX & cameraFollowHeight)
        {
            // to do
        }
    }
}
