using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class CrewMember_Engineer : CrewMember {

	public CrewMember_Engineer () {
        attackSpeed.timeLeft = 3f;
        canonReloadSpeed.timeLeft = 2f;
        repairSpeed.timeLeft = 2f;
        attackStrength = .5f;
        walkSpeed = .75f;
        wage = 4f;
    }
	
}
