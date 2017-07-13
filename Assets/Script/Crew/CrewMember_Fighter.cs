using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class CrewMember_Fighter : CrewMember {

	public CrewMember_Fighter (string id) : base(id, CrewMember_Job.Pirate)
    {
        attackStrength = 3f;
        walkSpeed = 1f;
        wage = 2f;
        maxHunger = .5f;
        satiety = .5f;
        maxLife = 150f;
        life = 150f;
        this.assignedRoom = Ship_Item.CANON;
        memberImage = "Sprites/axe";
    }
	
}
