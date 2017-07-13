using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class CrewMember_FastUnit : CrewMember {

	public CrewMember_FastUnit (string id) : base(id, CrewMember_Job.Pirate)
    {
        attackStrength = .25f;
        walkSpeed = 5f;
        wage = 3f;
        maxHunger = 1f;
        satiety = 1f;
        this.assignedRoom = Ship_Item.CANON;
        memberImage = "Sprites/star";
    }
	
}
