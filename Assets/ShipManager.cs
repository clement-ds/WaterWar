using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShipManager : MonoBehaviour {

    public ShipDisposition shipD;

    // NEED HELP HERE, HOW TO TEST
    void Start () {
        PlayerManager managerP = PlayerManager.GetInstance();
        shipD = managerP.player.ship;

        for (int i = 0; i < shipD.rooms.Count; ++i)
        {
            if (shipD.rooms[i].type != "None")
            {
                GameObject Icon = GameObject.Find("Icon Room " + i.ToString());
                if (shipD.rooms[i].type == "Food")
                {
                    Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Apple");
                }
            }
        }
    }
}
