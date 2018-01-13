using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class CrewMember_Healer : CrewMember {

	public CrewMember_Healer (string id) : base(id, CrewMember_Job.Pirate)
    {
        this.wage = 2f;
        this.maxHunger = .5f;
        this.satiety = .5f;
        this.maxLife = 100f;
        this.life = this.maxLife;
        this.assignedRoom = Ship_Item.INFIRMARY;
        this.memberImage = "Sprites/axe";

        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.RCanonTime, 150f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.ShootCanonValue, 50f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.RepairTime, 130f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.RepairValue, 80f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.WalkValue, 100f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.AttackTime, 100f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.AttackValue, 100f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.HealTime, 70f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.HealValue, 120f));
    }

}
