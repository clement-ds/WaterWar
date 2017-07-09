using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class StatUIController : MonoBehaviour
{

    public RectTransform panel;
    public GameObject rootPanel;
    public GameObject rowPrefab;
    private List<CrewMember> crewList;


    public void Populate()
    {
        ClearPanel();
        crewList = PlayerManager.GetInstance().player.crew.crewMembers;

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
                    InputField nameText = (InputField)child.GetComponent<InputField>();
                    nameText.text = member.memberName;
                    CreateClosureForName(member, nameText);

                }
                else if (child.name == "MemberType")
                {
                    Text text = (Text)child.GetComponent<Text>();
                    text.text = member.type;
                }
                else if (child.name == "WageField")
                {
                    InputField wageField = (InputField)child.GetComponent<InputField>();
                    wageField.text = member.wage + "£";
                    CreateClosureForWage(member, wageField);
                }
                else if (child.name == "SackButton")
                {
                    Button sackButton = (Button)child.GetComponent<Button>();
                    CreateClosureForSack(member, sackButton);
                }
            }
            crewRow.transform.SetParent(panel.transform, false);

        }
    }

    // Necessary because of unity bug in lambda
    void CreateClosureForName(CrewMember member, InputField field)
    {
        field.onEndEdit.AddListener((string txt) => { member.memberName = txt; });
    }
    void CreateClosureForWage(CrewMember member, InputField field)
    {
        field.onEndEdit.AddListener((string txt) => { member.wage = float.Parse(txt); });
    }
    void CreateClosureForSack(CrewMember member, Button button)
    {
        button.onClick.AddListener(() => PreRemoveCrew(member));
    }
    // -----------------------------------------

    private void PreRemoveCrew(CrewMember member)
    {
        PlayerManager.GetInstance().player.crew.RemoveCrew(member.id);
        Populate();
    }

    public void TogglePanel()
    {
        rootPanel.SetActive(!rootPanel.active);
        panel.gameObject.SetActive(rootPanel.active);
        if (panel.gameObject.active)
            Populate();
    }

    private void ClearPanel()
    {
        foreach (Transform child in panel)
        {
            if (child != panel.transform)
                Destroy(child.gameObject);
        }
    }


}
