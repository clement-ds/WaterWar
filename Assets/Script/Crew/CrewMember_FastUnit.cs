using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class CrewMember_FastUnit : CrewMember {

	public CrewMember_FastUnit (string id) : base(id, CrewMember_Job.Pirate)
    {
        this.wage = 3f;
        this.maxHunger = 1f;
        this.satiety = 1f;
        this.assignedRoom = Ship_Item.CANON;
        this.memberImage = "Sprites/star";

        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.RCanonTime, 60f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.ShootCanonValue, 100f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.RepairTime, 150f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.RepairValue, 50f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.WalkValue, 200f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.AttackTime, 100f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.AttackValue, 90f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.HealTime, 150f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.HealValue, 50f));
    }

}
