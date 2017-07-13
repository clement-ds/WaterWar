using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class CrewMember_FastUnit : CrewMember {

	public CrewMember_FastUnit (string id) : base(id, CrewMember_Job.Pirate)
    {
        attackSpeed.timeLeft = .2f;
        //canonReloadSpeed.timeLeft = 4f;
        repairSpeed = 0;
        canonReloadSpeed = 100;
        attackStrength = .25f;
        walkSpeed = 5f;
        wage = 3f;
        maxHunger = 1f;
        satiety = 1f;
        memberImage = "Sprites/star";
    }
	
}
