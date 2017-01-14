using UnityEngine;
using System.Collections;

public class CrewMember_Engineer : CrewMember {

	void Start () {
        attackSpeed.timeLeft = 3f;
        canonReloadSpeed.timeLeft = 2f;
        repairSpeed.timeLeft = 2f;
        attackStrength = .5f;
        walkSpeed = .75f;
        wage = 4f;
    }
	
}
