using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class GuiAccess : MonoBehaviour {

    public Text distanceToEnemy;
    public Button escapeButton;
    public Button boardingButton;
    public Image endPanel;
    public Image endPanelLoot;
    public List<Text> endMessages;
    public List<Text> endMessagesLoot;
    //public GameObject contentLootList;
    public Transform contentLootListTransform;


    void Start()
    {
        GameRulesManager.GetInstance().initializeGui();
    }
}
