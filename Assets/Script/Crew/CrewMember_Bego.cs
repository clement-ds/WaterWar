using UnityEngine;
using System.Collections;

public class CrewMember_Bego : CrewMember {

    public CrewMember_Bego()
    {
        //attackSpeed.timeLeft = 10f;
        //canonReloadSpeed.timeLeft = 10f;
        attackStrength = 0.1f;
        walkSpeed = 0.1f;
        wage = 0f;
        maxHunger = 15f;
        satiety = 15f;
        maxLife = 10f;
        life = 10f;
    }
	
}
