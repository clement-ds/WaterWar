using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class CrewMember_Fighter : CrewMember {

	public CrewMember_Fighter (string id) : base(id)
    {
        attackSpeed.timeLeft = .5f;
        canonReloadSpeed.timeLeft = 5f;
        repairSpeed.timeLeft = 7f;
        attackStrength = 3f;
        walkSpeed = 1f;
        wage = 2f;
        maxHunger = .5f;
        satiety = .5f;
        maxLife = 150f;
        life = 150f;
    }
	
}
