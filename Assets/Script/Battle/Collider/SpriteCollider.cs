using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;


public class SpriteCollider : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        //this.transform.gameObject.AddComponent<BoxCollider>();
        //this.transform.gameObject.AddComponent<Rigidbody2D>();
        // this.transform.localScale = new Vector3(this.GetComponent<BoxCollider2D>().size.x * 100 / 10, this.GetComponent<BoxCollider2D>().size.x * 100 / 10, 0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseEnter()
    {
        this.GetComponent<SpriteRenderer>().enabled = true;
    }

    void OnMouseExit()
    {
        this.GetComponent<SpriteRenderer>().enabled = false;
    }
}