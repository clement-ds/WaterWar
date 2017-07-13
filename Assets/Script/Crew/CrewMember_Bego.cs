using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class CrewMember_Bego : CrewMember {

    public CrewMember_Bego(string id) : base(id, CrewMember_Job.Pirate)
    {
        attackStrength = 0.1f;
        walkSpeed = 0.1f;
        wage = 0f;
        maxHunger = 15f;
        satiety = 15f;
        maxLife = 10f;
        life = 10f;
        this.assignedRoom = Ship_Item.CANTEEN;
        memberImage = "Sprites/poulet";

        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.RCanonTime, 50f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.ShootCanonValue, 50f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.RepairTime, 50f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.RepairValue, 50f));
        this.skills.Add(new KeyValuePair<SkillAttribute, float>(SkillAttribute.WalkValue, 100f));
    }

}
