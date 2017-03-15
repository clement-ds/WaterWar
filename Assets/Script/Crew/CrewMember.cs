using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class CrewMember {

    public string id;
    public string type;
    protected float attackStrength = 1f;
    protected bool useRangedWeapon = false;
    protected float walkSpeed = 1f;
    protected float wage = 1f;
    protected float maxHunger = 1f;
    protected float maxLife = 100f;
    public float life = 10f;
    public float satiety = 1f;

    [NonSerialized]
    protected Cooldown attackSpeed;
    [NonSerialized]
    protected Cooldown canonReloadSpeed;
    [NonSerialized]
    protected Cooldown steerSpeed;
    [NonSerialized]
    protected Cooldown repairSpeed;

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

    /** GETTERS **/
    public string getId()
    {
        return this.id;
    }
}
