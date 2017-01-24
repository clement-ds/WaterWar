using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class CrewMember_Bego : CrewMember {

    public CrewMember_Bego(CrewMember cm)
    {
        type = "Bego";
        //attackSpeed.timeLeft = 10f;
        //canonReloadSpeed.timeLeft = 10f;
        attackStrength = 0.1f;
        walkSpeed = 0.1f;
        wage = 0f;
        maxHunger = 15f;
        satiety = cm.satiety;
        maxLife = 10f;
        life = cm.life;
    }

    public CrewMember_Bego()
    {
        type = "Bego";
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
