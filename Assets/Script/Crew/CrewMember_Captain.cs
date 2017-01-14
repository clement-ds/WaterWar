using UnityEngine;
using System.Collections;

public class CrewMember_Captain : CrewMember {

	void Start () {
        steerSpeed.timeLeft = 5f;
        attackSpeed.timeLeft = 2f;
        canonReloadSpeed.timeLeft = 5f;
        attackStrength = 5f;
        walkSpeed = 1f;
        wage = 5f;
        maxHunger = 1.5f;
        satiety = 1.5f;
        maxLife = 300f;
        life = 300f;
    }
	
}
