using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;


// works with ListPanelRoot/ListReceiverPanel/ListRowPanel
public class RecruitUIController : UIController
{
    private List<CrewMember> crewList;


    public override void Populate()
    {
        base.Populate();
        FillItems();
    }


    private void FillItems()
    {
        crewList = IslandManager.GetInstance().islands[PlayerManager.GetInstance().player.currentIsland].crew;

        foreach (CrewMember member in crewList)
        {
            GameObject crewRow = (GameObject)Instantiate(rowPrefab);

            foreach (Transform child in crewRow.transform)
            {
                if (child.name == "MemberImage")
                {
                    Image img = (Image)child.GetComponent<Image>();
                    img.sprite = new Sprite(); // TODO: GET IMG
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
                    price.text = member.wage + "£";
                }
                else if (child.name == "RecruitButton")
                {
                    Button recruitButton = (Button)child.GetComponent<Button>();
                    CreateClosureForRecruit(member, recruitButton);
                }
            }
            crewRow.transform.SetParent(panel.transform, false);

        }
    }

    // Necessary because of unity bug in lambda
    void CreateClosureForRecruit(CrewMember member, Button button)
    {
        button.onClick.AddListener(() => PreRemoveCrew(member));
    }
    // -----------------------------------------

    private void PreRemoveCrew(CrewMember member)
    {
        IslandManager.GetInstance().islands[PlayerManager.GetInstance().player.currentIsland].removeCrewMember(member);
        PlayerManager.GetInstance().player.crew.AddCrew(member);
        Populate();
    }


    public void OnClick()
    {
        TogglePanel();
    }

}
