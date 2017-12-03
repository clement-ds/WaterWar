using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderCameraPadding : MonoBehaviour {

	public int ySpeed;

    public void Padding() {
        Vector3 tmp = transform.position; 
        tmp.y = tmp.y - ( Input.GetAxis("Mouse Y") * ySpeed);
        tmp.x = tmp.x - ( Input.GetAxis("Mouse X") * ySpeed);
 
        transform.position = tmp; 
    }

}
