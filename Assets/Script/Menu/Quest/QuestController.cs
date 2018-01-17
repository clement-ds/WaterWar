using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using TMPro;


// works with ListPanelRoot/ListReceiverPanel/ListRowPanel
public class QuestController : UIController
{
    private List<PlayerQuest> questList;
    [SerializeField] private  Button nextPage;
    [SerializeField] private  Button previousPage;
    public int nbrQuestByPage = 10;
    private int currentPage = 0;

    [Header("Info UI elements")]
	public GameObject UICanvas;
	public TextMeshProUGUI title;
	public TextMeshProUGUI description;
	public TextMeshProUGUI rewards;
    private QuestInfosUIDisplayer UIDisplayer;
    [SerializeField] private IntroSceneManager sceneManager;


    void Start() {
        Populate();
        nextPage.onClick.AddListener(NextPage);
        previousPage.onClick.AddListener(PrevPage);
    }

    public override void Populate()
    {
        base.Populate();
        FillItems();
    }


    private void FillItems()
    {
        questList = PlayerManager.GetInstance().player.questLog.quests;

        int i = 0;

        foreach (PlayerQuest quest in questList)
        {
            i++;
            if (i >= (nbrQuestByPage * currentPage) && i < (nbrQuestByPage * currentPage + nbrQuestByPage))
            {
                GameObject questRow = (GameObject)GameObject.Instantiate(rowPrefab);

                foreach (Transform child in questRow.transform)
                {
                    if (child.name == "MemberTitle")
                    {
                        TextMeshProUGUI text = child.GetComponent<TextMeshProUGUI>();
                        text.SetText(quest.title);
                    }
                    else if (child.name == "MemberDescription")
                    {
                        TextMeshProUGUI text = child.GetComponent<TextMeshProUGUI>();
                        text.SetText(quest.description);

                    }
                    else if (child.name == "MemberItemReward")
                    {
                        Image img = (Image)child.GetComponent<Image>();
                        img.sprite = Resources.Load<Sprite>("Sprites/quest");
                    }
                    else if (child.name == "MemberDescription")
                    {
                        TextMeshProUGUI text = child.GetComponent<TextMeshProUGUI>();
                        text.SetText(quest.description);
                    }
                    else if (child.name == "MemberMoneyReward")
                    {
                        TextMeshProUGUI text = child.GetComponent<TextMeshProUGUI>();
                        text.SetText("+" + quest.moneyReward + "£");
                    }
                    else if (child.name == "AcceptButton")
                    {
                        Button AcceptButton = (Button)child.GetComponent<Button>();
                        QuestGenerator qgen = new QuestGenerator();
                        Player player = PlayerManager.GetInstance().player;
                        Island island = IslandManager.GetInstance().islands[player.currentIsland];
                        AcceptButton.gameObject.SetActive(qgen.CheckQuest(quest, player, island));
                        CreateClosureForAccept(quest, AcceptButton);
                    }
                }
                UIDisplayer = questRow.GetComponent<QuestInfosUIDisplayer>();
                UIDisplayer.SetQuest(quest);
                UIDisplayer.SetObjectsReferences(UICanvas, title, description, rewards);
                questRow.transform.SetParent(panel.transform, false);
                questRow.SetActive(true);
            }
        }
        if (i > 0 && currentPage < (i / nbrQuestByPage))
            nextPage.gameObject.SetActive(true);
        else 
            nextPage.gameObject.SetActive(false);

        if (currentPage > 0)
            previousPage.gameObject.SetActive(true);
        else 
            previousPage.gameObject.SetActive(false);

    }

    // Necessary because of unity bug in lambda
    void CreateClosureForAccept(PlayerQuest quest, Button button)
    {
        button.onClick.AddListener(() =>
        {
            Player player = PlayerManager.GetInstance().player;
            Island island = IslandManager.GetInstance().islands[player.currentIsland];
            QuestGenerator qgen = new QuestGenerator();
            if (qgen.ValidateQuest(quest, player, island)) {
                sceneManager.PlaySound("valid_quest");
                UIDisplayer.hideHud();
            }
            Populate();
        });
    }
    // -----------------------------------------

    public void NextPage()
    {
        ++currentPage;
        Populate();
    }

    public void PrevPage()
    {
        --currentPage;
        Populate();
    }

}
