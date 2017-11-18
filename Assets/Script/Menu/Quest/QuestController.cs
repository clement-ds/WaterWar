using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;


// works with ListPanelRoot/ListReceiverPanel/ListRowPanel
public class QuestController : UIController
{
    private List<PlayerQuest> questList;
    public Button closeButton;
    public int nbrQuestByPage = 10;
    private int currentPage = 0;

    void Start() {
        FillItems();
    }

    public override void Populate()
    {
        base.Populate();
        FillItems();
    }


    private void FillItems()
    {
        Debug.Log("----------------Nbr of quests: " + PlayerManager.GetInstance().player.questLog.quests.Count);
        questList = PlayerManager.GetInstance().player.questLog.quests;

        int i = 0;

        foreach (PlayerQuest quest in questList)
        {
            i++;
            if (i >= (nbrQuestByPage * currentPage) && i < (nbrQuestByPage * currentPage + nbrQuestByPage))
            {
                GameObject questRow = (GameObject)Instantiate(rowPrefab);

                foreach (Transform child in questRow.transform)
                {
                    if (child.name == "MemberObjectif")
                    {
                        Text text = (Text)child.GetComponent<Text>();
                        text.text = quest.objective;
                    }
                    else if (child.name == "MemberDescription")
                    {
                        Text text = (Text)child.GetComponent<Text>();
                        text.text = quest.description;

                    }
                    else if (child.name == "MemberItemReward")
                    {
                        Image img = (Image)child.GetComponent<Image>();
                        img.sprite = Resources.Load<Sprite>("Sprites/quest");
                    }
                    else if (child.name == "MemberDescription")
                    {
                        Text text = (Text)child.GetComponent<Text>();
                        text.text = quest.description;
                    }
                    else if (child.name == "MemberMoneyReward")
                    {
                        Text text = (Text)child.GetComponent<Text>();
                        text.text = "+" + quest.moneyReward + "£";
                    }
                    else if (child.name == "AcceptButton")
                    {
                        Button AcceptButton = (Button)child.GetComponent<Button>();
                        CreateClosureForAccept(quest, AcceptButton);
                    }
                }
                questRow.transform.SetParent(panel.transform, false);
                questRow.SetActive(true);
            }
        }
    }

    // Necessary because of unity bug in lambda
    void CreateClosureForAccept(PlayerQuest quest, Button button)
    {
        button.onClick.AddListener(() =>
        {
            Player player = PlayerManager.GetInstance().player;

            quest.taken = true;
            player.questLog.quests.Add(quest);
            IslandManager.GetInstance().islands[player.currentIsland].questLog.quests.Remove(quest);
            Populate();
        });
    }
    // -----------------------------------------

    public void OnClick()
    {
        TogglePanel();
        closeButton.gameObject.SetActive(panel.gameObject.active);
    }

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
