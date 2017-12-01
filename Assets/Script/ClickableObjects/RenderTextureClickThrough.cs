using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RenderTextureClickThrough : MonoBehaviour {
    public EventSystem eventSystem;
    private Camera renderCamera, mainCamera;

	void Start () {
		mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
		renderCamera = GameObject.Find("RenderMapCamera").GetComponent<Camera>();
	}
	

    void OnMouseDown() {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Vector2 pointOnMap = hit.textureCoord;
                Ray portalRay = renderCamera.ScreenPointToRay(new Vector2(pointOnMap.x * renderCamera.pixelWidth, pointOnMap.y * renderCamera.pixelHeight));
                RaycastHit portalHit;
                if (Physics.Raycast(portalRay, out portalHit, Mathf.Infinity))
                {
                    GameObject obj = portalHit.collider.gameObject;

                    Button b = obj.GetComponent<Button>();
                    if (b) { // TODO: marche pas
                        b.onClick.Invoke();
                    } else {
                        Debug.Log("in else");
                        obj.SendMessage("OnMouseDown");
                       /* eventSystem.SetSelectedGameObject(obj);
                        Debug.Log("selected item: " + eventSystem.currentSelectedGameObject);
                        */
                    }

                }
            }
        }

    }
}
