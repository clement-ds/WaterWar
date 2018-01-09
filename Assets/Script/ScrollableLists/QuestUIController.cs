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
        checkButton.gameObject.SetActive(checkButton.gameObject.active);
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
        closeButton.gameObject.SetActive(panel.gameObject.active);
    }

    public void checkQuest() {
        List<PlayerQuest> questList = PlayerManager.GetInstance().GetQuest();
        QuestGenerator gen = new QuestGenerator();
        foreach (PlayerQuest quest in questList) {
            Player player = PlayerManager.GetInstance().player;
            if (gen.CheckQuest(quest, player)) {
                if (quest.reward.type == Reward.REWARD.INFLUENCE) {
                    IslandManager.GetInstance().islands[PlayerManager.GetInstance().player.currentIsland].influence = quest.reward.amount;
                } else if (quest.reward.type == Reward.REWARD.MONEY) {
                    player.money = quest.reward.amount;
                } else if (quest.reward.type == Reward.REWARD.OBJECT) {
                    player.money = quest.reward.amount;
//                    player.inventory.food. quest.reward.id = UnityEngine.Random.Range(0, objects.Count);
//                    quest.reward.amount = UnityEngine.Random.Range(1, 10);
                }
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
