using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class CrewMember_Engineer : CrewMember {

	public CrewMember_Engineer (string id) : base(id, CrewMember_Job.Engineer)
    {
        attackStrength = .5f;
		walkSpeed = .75f;
		wage = 4f;
        this.assignedRoom = Ship_Item.CANTEEN;
        memberImage = "Sprites/loupe";
    }
	
}
