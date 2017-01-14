using System;
using UnityEngine;
using Object = UnityEngine.Object;


namespace UnityStandardAssets.Utility
{

    public class TavernMenu : MonoBehaviour
    {

        void Start()
        {
            inventory.enabled = false;
        }

        public Canvas inventory;

        void OnMouseDown()
        {
            inventory.enabled = (inventory.enabled == false);
        }
    }
}
