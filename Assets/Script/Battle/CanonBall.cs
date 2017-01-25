using UnityEngine;
using System.Collections;

public class CanonBall : MonoBehaviour {

    private GameObject boulet = null;
    private Transform target = null;

    // Use this for initialization
    public void Start () {
        boulet = new GameObject("CanonBall");
        boulet.AddComponent<SpriteRenderer>();
        boulet.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/boulet-de-canon");
        boulet.GetComponent<SpriteRenderer>().transform.localScale = new Vector3(.2f, .2f, .2f);
    }

    public void setTransformation(Transform transform) {
        if (boulet != null && transform != null) {
            boulet.GetComponent<SpriteRenderer>().transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
            boulet.GetComponent<SpriteRenderer>().transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        }
    }

    public void setTarget(Transform transform) {
        target = transform;
    }

    // Update is called once per frame
    void Update () {
        // 3 = speed
        if (target != null) {
            float step = 3 * Time.deltaTime;
            boulet.transform.position = Vector3.MoveTowards(boulet.transform.position, target.transform.position, step);
        }
    }
}
