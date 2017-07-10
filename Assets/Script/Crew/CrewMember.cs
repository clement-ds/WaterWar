using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class CrewMember {

    public string id;
    public string type;
    public string memberName;
    public float attackStrength = 1f;
    public bool useRangedWeapon = false;
    public float walkSpeed = 1f;
    public float wage = 1f;
    public float maxHunger = 1f;
    public float maxLife = 100f;
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

    // should be a class "Room"
    public string assignedRoom;

    public CrewMember(string id)
    {
        this.id = id;
        type = this.GetType().Name.Substring(this.GetType().Name.IndexOf("_") + 1);
        attackSpeed = new Cooldown();
        canonReloadSpeed = new Cooldown();
        steerSpeed = new Cooldown();
        repairSpeed = new Cooldown();
        assignedRoom = "";
        memberName = id;
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

    public void Attack(GameObject target)
    {
        if (attackSpeed.getPossibility())
        {
            ;
            // target.hit();
        }
    }

    public void ReloadCanon(GameObject canon)
    {
        if (canonReloadSpeed.getPossibility())
        {
            ;
            //canon.reload;
        }
    }

    public void Repair()
    {
        if (repairSpeed.getPossibility())
        {
            ;
            // assignedRoom.repair();
        }
    }

    public void AdjustWage(float newWage)
    {
        this.wage = newWage;
    }

}
