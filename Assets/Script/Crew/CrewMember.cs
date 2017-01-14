using UnityEngine;
using System.Collections;

public class CrewMember : MonoBehaviour {
    protected Cooldown attackSpeed;
    protected Cooldown canonReloadSpeed;
    protected Cooldown steerSpeed;
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

    // Use this for initialization
    void Start () {
        attackSpeed.timeLeft = 1f;
        canonReloadSpeed.timeLeft = 5f;
        steerSpeed.timeLeft = 10f;
        repairSpeed.timeLeft = 5f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

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
