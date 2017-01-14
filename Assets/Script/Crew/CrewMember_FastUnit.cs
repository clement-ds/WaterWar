using UnityEngine;
using System.Collections;

public class CrewMember_FastUnit : CrewMember {

	void Start () {
        attackSpeed.timeLeft = .2f;
        canonReloadSpeed.timeLeft = 4f;
        attackStrength = .25f;
        walkSpeed = 5f;
        wage = 3f;
        maxHunger = 1f;
        satiety = 1f;
    }
	
}
