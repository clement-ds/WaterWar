using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

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

        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.RCanonTime, 100f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.ShootCanonValue, 100f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.RepairTime, 50f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.RepairValue, 50f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.WalkValue, 100f));
    }
	
}
