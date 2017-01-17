using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class CrewMember {

    public string type;
    [NonSerialized]
    protected Cooldown attackSpeed;
    [NonSerialized]
    protected Cooldown canonReloadSpeed;
    [NonSerialized]
    protected Cooldown steerSpeed;
    [NonSerialized]
    protected Cooldown repairSpeed;
    protected float attackStrength = 1f;
    protected bool useRangedWeapon = false;
    protected float walkSpeed = 1f;
    protected float wage = 1f;
    protected float maxHunger = 1f;
    protected float maxLife = 100f;
    protected float life = 10f;

    protected float satiety = 1f;

    //private Room assignedRoom ?

    public CrewMember()
    {
        //attackSpeed.timeLeft = 1f;
        //canonReloadSpeed.timeLeft = 5f;
        //steerSpeed.timeLeft = 10f;
        //repairSpeed.timeLeft = 5f;
    }

    // Use this for initialization
 //   void Start () {
        
	//}
	
	// Update is called once per frame
	//void Update () {
	
	//}

    public void attack(GameObject target)
    {
        if (attackSpeed.getPossibility())
        {
            ;
            // target.hit();
        }
    }

    public void reloadCanon(GameObject canon)
    {
        if (canonReloadSpeed.getPossibility())
        {
            ;
            //canon.reload;
        }
    }

    public void repair()
    {
        if (repairSpeed.getPossibility())
        {
            ;
            // assignedRoom.repair();
        }
    }


}
