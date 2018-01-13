using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;


// works with ListPanelRoot/ListReceiverPanel/ListRowPanel
public class QuestUIController : UIController
{
    public Button closeButton;
    public Button checkButton;
    public int nbrQuestByPage = 10;
    private int currentPage = 0;


    public override void Populate()
    {
        base.Populate();
        FillItems();
        checkButton.gameObject.SetActive(checkButton.gameObject.activeSelf);
    }


    private void FillItems()
    {
        Debug.Log("Current island: " + PlayerManager.GetInstance().player.currentIsland);
        List<PlayerQuest> questList = IslandManager.GetInstance().islands[PlayerManager.GetInstance().player.currentIsland].questLog.quests;

        int i = 0;

        foreach (PlayerQuest quest in questList) {
            i++;
            if (i >= (nbrQuestByPage * currentPage) && i < (nbrQuestByPage * currentPage  + nbrQuestByPage)) {
                GameObject questRow = (GameObject)GameObject.Instantiate(rowPrefab);

                foreach (Transform child in questRow.transform) {
                    if (child.name == "MemberObjectif") {
                        Text text = (Text)child.GetComponent<Text>();
                        text.text = quest.objective;
                    } else if (child.name == "MemberDescription") {
                        Text text = (Text)child.GetComponent<Text>();
                        text.text = quest.description;

                    } else if (child.name == "MemberItemReward") {
                        Image img = (Image)child.GetComponent<Image>();
                        img.sprite = Resources.Load<Sprite>("Sprites/quest");
                    } else if (child.name == "MemberDescription") {
                        Text text = (Text)child.GetComponent<Text>();
                        text.text = quest.description;
                    } else if (child.name == "MemberMoneyReward") {
                        Text text = (Text)child.GetComponent<Text>();
                        text.text = "+" + quest.moneyReward + "£";
                    } else if (child.name == "AcceptButton") {
                        Button AcceptButton = (Button)child.GetComponent<Button>();
                        CreateClosureForAccept(quest, AcceptButton);
                    }
                }
                questRow.transform.SetParent(panel.transform, false);
                questRow.SetActive(true);
            }
        }
        checkButton.gameObject.SetActive(true);
    }

    // Necessary because of unity bug in lambda
    void CreateClosureForAccept(PlayerQuest quest, Button button)
    {
        button.onClick.AddListener(() =>
        {
            PlayerManager.GetInstance().AcceptQuest(quest);
            Populate();
        });
    }
    // -----------------------------------------

    public void OnClick()
    {
        TogglePanel();
        closeButton.gameObject.SetActive(panel.gameObject.activeSelf);
    }

    public void checkQuest() {
        List<PlayerQuest> questList = PlayerManager.GetInstance().GetQuest();
        QuestGenerator gen = new QuestGenerator();
        Player player = PlayerManager.GetInstance().player;
        for (int i = 0; i < questList.Count; i++) {
            if (gen.CheckQuest(questList[i], player, IslandManager.GetInstance().islands[PlayerManager.GetInstance().player.currentIsland])) {
                player.questLog.quests.RemoveAt(i);
            }
        }
    }

    public void NextPage() {
        ++currentPage;
        Populate();
    }

    public void PrevPage() {
        --currentPage;
        Populate();
    }

}
