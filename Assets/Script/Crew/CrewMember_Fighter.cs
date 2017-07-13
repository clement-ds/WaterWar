using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

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

        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.RCanonTime, 100f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.ShootCanonValue, 100f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.RepairTime, 80f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.RepairValue, 50f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.WalkValue, 100f));
    }
	
}
