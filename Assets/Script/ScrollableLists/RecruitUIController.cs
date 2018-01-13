using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;


// works with ListPanelRoot/ListReceiverPanel/ListRowPanel
public class RecruitUIController : UIController
{
    private List<CrewMember> crewList;
    public int unitPriceMultiplier;
    public Button closeButton;


    public override void Populate()
    {
        base.Populate();
        FillItems();
    }


    private void FillItems()
    {
        Debug.Log("Current island: " + PlayerManager.GetInstance().player.currentIsland);
        crewList = IslandManager.GetInstance().islands[PlayerManager.GetInstance().player.currentIsland].crew;

        foreach (CrewMember member in crewList)
        {
            GameObject crewRow = (GameObject)GameObject.Instantiate(rowPrefab);

            foreach (Transform child in crewRow.transform)
            {
                if (child.name == "MemberImage")
                {
                    Image img = (Image)child.GetComponent<Image>();
                    if (member.memberImage != "None")
                    {
                        img.sprite = Resources.Load<Sprite>(member.memberImage);
                    }
                }
                else if (child.name == "MemberName")
                {
                    Text nameText = (Text)child.GetComponent<Text>();
                    nameText.text = member.memberName;

                }
                else if (child.name == "MemberType")
                {
                    Text text = (Text)child.GetComponent<Text>();
                    text.text = member.type;
                }
                else if (child.name == "MemberPrice")
                {
                    Text price = (Text)child.GetComponent<Text>();
                    price.text = ((int)member.wage * unitPriceMultiplier) + "£";
                }
                else if (child.name == "RecruitButton")
                {
                    Button recruitButton = (Button)child.GetComponent<Button>();
                    CreateClosureForRecruit(member, recruitButton, (int)member.wage * unitPriceMultiplier);
                }
            }
            crewRow.transform.SetParent(panel.transform, false);

        }
    }

    // Necessary because of unity bug in lambda
    void CreateClosureForRecruit(CrewMember member, Button button, int price)
    {
        button.onClick.AddListener(() => PreRemoveCrew(member, price));
    }
    // -----------------------------------------

    private void PreRemoveCrew(CrewMember member, int price)
    {
        Player player = PlayerManager.GetInstance().player;
        if (player.money >= price)
        {
            IslandManager.GetInstance().islands[PlayerManager.GetInstance().player.currentIsland].removeCrewMember(member);
            player.crew.AddCrew(member);
            player.money -= price;
        } else
        {
            //TODO : popup qui dit t'as pas de thune
        }
        Populate();
    }


    public void OnClick()
    {
        TogglePanel();
        closeButton.gameObject.SetActive(panel.gameObject.activeSelf);
    }

}
