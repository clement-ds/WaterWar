using UnityEngine;
using System.Collections;

public class CarpenterShop : MonoBehaviour
{

    public string current;
    // Use this for initialization
    void Start()
    {
        current = "Untouched";
    }

    public void setCurrent(string room)
    {
        current = room;
    }
}
