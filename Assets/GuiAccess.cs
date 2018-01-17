using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class GuiAccess : MonoBehaviour {

    public Text distanceToEnemy;
    public Button escapeButton;
    public Button boardingButton;
    public Image endPanel;
    public Transform contentLootListTransform;
    public Text noLoot;
    public ScrollRect lootListView;
    public Transform ShortCutCanon;


    void Start()
    {
        GameRulesManager.GetInstance().initializeGui();
    }
}
