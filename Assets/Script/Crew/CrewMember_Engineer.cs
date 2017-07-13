using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class CrewMember_Engineer : CrewMember {

	public CrewMember_Engineer (string id) : base(id, CrewMember_Job.Engineer)
    {
        attackStrength = .5f;
		walkSpeed = .75f;
		wage = 4f;
        this.assignedRoom = Ship_Item.CANTEEN;
        memberImage = "Sprites/loupe";

        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.RCanonTime, 70f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.ShootCanonValue, 70f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.RepairTime, 200f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.RepairValue, 200f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.WalkValue, 100f));
    }
	
}
