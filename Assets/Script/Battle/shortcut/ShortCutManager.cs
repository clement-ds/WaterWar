using UnityEngine;
using System.Collections.Generic;

public class ShortCutManager : MonoBehaviour {

    Dictionary<KeyCode, GroupElement> groups;

	// Use this for initialization
	void Start () {
        this.groups = new Dictionary<KeyCode, GroupElement>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void resetActionsGroupForInput(KeyCode code)
    {
        List<Battle_CrewMember> crewMembers = this.transform.GetComponentInParent<Battle_Player>().getSelectedCrewMembers();
        groups[KeyCode.Alpha1].initActions();
        foreach (Battle_CrewMember item in crewMembers)
        {
            groups[KeyCode.Alpha1].addActionList(item.getActionList());
            groups[KeyCode.Alpha1].addActionList(item.getParentActionList());
        }
        groups[KeyCode.Alpha1].StartMyself();
        groups[KeyCode.Alpha1].focus();
    }

    void OnGUI()
    {
        Event e = Event.current;
        if (e.type == EventType.KeyDown && e.control && e.keyCode == KeyCode.Alpha1)
        {
            if (!groups.ContainsKey(KeyCode.Alpha1))
            {
                groups.Add(KeyCode.Alpha1, new GroupElement());
            }
            resetActionsGroupForInput(KeyCode.Alpha1);
        }
        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Alpha1)
        {
            if (groups.ContainsKey(KeyCode.Alpha1))
            {
                if (groups[KeyCode.Alpha1].isFocused())
                {
                    groups[KeyCode.Alpha1].unfocus();
                } else
                {
                    resetActionsGroupForInput(KeyCode.Alpha1);
                }
            }
        }
    }
}
