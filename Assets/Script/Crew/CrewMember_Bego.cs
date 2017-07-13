using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class CrewMember_Bego : CrewMember {

    public CrewMember_Bego(string id) : base(id, CrewMember_Job.Pirate)
    {
        //attackSpeed.timeLeft = 10f;
        //canonReloadSpeed.timeLeft = 10f;
        canonReloadSpeed = 30;
        repairSpeed = 0;
        attackStrength = 0.1f;
        walkSpeed = 0.1f;
        wage = 0f;
        maxHunger = 15f;
        satiety = 15f;
        maxLife = 10f;
        life = 10f;
        this.assignedRoom = Ship_Item.CANTEEN;
    }

}
