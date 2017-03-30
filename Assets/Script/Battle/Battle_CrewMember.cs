using UnityEngine;
using System.Collections;
using System;

public class Battle_CrewMember : GuiElement
{

    ShipElement room = null;
    CrewMember member = null;

    // Use this for initialization
    protected override void StartMySelf()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }


    protected override void createActionList()
    {
    }

    /** GETTERS **/

    public string getId()
    {
        return this.member.getId();
    }
}
