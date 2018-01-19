using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CrewMember_Captain : CrewMember {

	public CrewMember_Captain(string id) : base(id, CrewMember_Job.Captain)
    {
        this.wage = 5f;
        this.maxHunger = 1.5f;
        this.satiety = 1.5f;
        this.maxLife = 200f;
        this.life = this.maxLife;
        this.useRangedWeapon = true;
        this.assignedRoom = Ship_Item.CANON;
        this.memberImage = "Sprites/capt";

        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.RCanonTime, 100f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.ShootCanonValue, 100f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.RepairTime, 110f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.RepairValue, 100f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.WalkValue, 100f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.AttackTime, 100f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.AttackValue, 200f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.HealTime, 110f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.HealValue, 80f));
    }
}
