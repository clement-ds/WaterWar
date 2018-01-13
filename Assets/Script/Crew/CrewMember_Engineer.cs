using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class CrewMember_Engineer : CrewMember {

	public CrewMember_Engineer (string id) : base(id, CrewMember_Job.Engineer)
    {
		this.wage = 4f;
        this.useRangedWeapon = true;
        this.assignedRoom = Ship_Item.WAREHOUSE;
        this.memberImage = "Sprites/loupe";

        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.RCanonTime, 70f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.ShootCanonValue, 70f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.RepairTime, 200f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.RepairValue, 200f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.WalkValue, 100f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.AttackTime, 100f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.AttackValue, 50f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.HealTime, 160f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.HealValue, 60f));
    }
	
}
