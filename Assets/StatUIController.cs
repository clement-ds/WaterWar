using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class StatUIController : MonoBehaviour
{

    public RectTransform panel;
    public GameObject rowPrefab;
    private List<CrewMember> crewList;
    private Dictionary<string, Func<int>> itemFillerDic;


    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void Populate()
    {
        ClearPanel();
        crewList = PlayerManager.GetInstance().player.crew.crewMembers;

        int turn = 0;
        foreach (CrewMember member in crewList)
        {
            GameObject crewRow = (GameObject)Instantiate(rowPrefab);

            foreach (Transform child in crewRow.transform)
            {
                if (child.name == "MemberImage")
                {
                    Debug.Log("In MemberImage");
                    Image img = (Image)child.GetComponent<Image>();
                    img.sprite = new Sprite(); // TODO: GET IMG
                }
                else if (child.name == "MemberName")
                {
                    Debug.Log("In MemberName");
                    InputField nameText = (InputField)child.GetComponent<InputField>();
                    nameText.text = member.memberName;

                }
                else if (child.name == "MemberType")
                {
                    Text text = (Text)child.GetComponent<Text>();
                    text.text = member.type;
                }
                else if (child.name == "WageField")
                {
                    Debug.Log("In WageField");
                    InputField wageField = (InputField)child.GetComponent<InputField>();
                    wageField.text = member.wage + "£";
                }
                else if (child.name == "SackButton")
                {
                    Debug.Log("In SackButton");
                    Button sackButton = (Button)child.GetComponent<Button>();
                    // DO STUFF TO SACK
                }
            }
            crewRow.transform.SetParent(panel.transform, false);
            crewRow.transform.Translate(new Vector3(0, -50 * turn++));


        }
    }

    public void TogglePanel()
    {
        panel.gameObject.SetActive(!panel.gameObject.active);
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
