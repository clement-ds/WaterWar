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

    void OnGUI()
    {
        Event e = Event.current;
        if (e.type == EventType.KeyDown && e.control && e.keyCode == KeyCode.Alpha1)
        {
            List<Battle_CrewMember> crewMembers = this.transform.GetComponentInParent<Battle_Player>().getSelectedCrewMembers();
            Debug.Log("yeaah");
            if (!groups.ContainsKey(KeyCode.Alpha1))
            {
                Debug.Log("add new group: " + crewMembers.Count);
                groups.Add(KeyCode.Alpha1, new GroupElement());
            }
            groups[KeyCode.Alpha1].initActions();
            foreach (Battle_CrewMember item in crewMembers)
            {
                groups[KeyCode.Alpha1].addActionList(item.getActionList());
                groups[KeyCode.Alpha1].addActionList(item.getParentActionList());
            }
            groups[KeyCode.Alpha1].StartMyself();
        }
        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Alpha1)
        {
            if (groups.ContainsKey(KeyCode.Alpha1))
            {
                Debug.Log("focus all ");
                if (groups[KeyCode.Alpha1].isFocused())
                {
                    groups[KeyCode.Alpha1].unfocus();
                } else
                {
                    groups[KeyCode.Alpha1].focus();
                }
            }
        }
    }
}
