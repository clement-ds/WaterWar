using UnityEngine;
using System.Collections;

public class CrewMember_Captain : CrewMember {

	public CrewMember_Captain(string id) : base(id, CrewMember_Job.Captain)
    {
        attackStrength = 5f;
        walkSpeed = 1f;
        wage = 5f;
        maxHunger = 1.5f;
        satiety = 1.5f;
        maxLife = 300f;
        life = 300f;
        this.assignedRoom = Ship_Item.HELM;
        memberImage = "Sprites/capt";
    }
	
}
