using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class CrewMember_Fighter : CrewMember {

	public CrewMember_Fighter (string id) : base(id, CrewMember_Job.FIGHTER)
    {
        this.wage = 2f;
        this.maxHunger = .5f;
        this.satiety = .5f;
        this.maxLife = 150f;
        this.life = this.maxLife;
        this.assignedRoom = Ship_Item.CANON;
        this.memberImage = "Sprites/axe";

        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.RCanonTime, 100f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.ShootCanonValue, 100f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.RepairTime, 100f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.RepairValue, 50f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.WalkValue, 100f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.AttackTime, 100f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.AttackValue, 120f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.HealTime, 120f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.HealValue, 20f));
    }

}
