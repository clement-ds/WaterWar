using UnityEngine;
using System.Collections;

public enum HitStatus { HIT, FAIL, MISS };

public class Battle_CanonBall : MonoBehaviour {

    protected Ammunition ammunition;
    protected ShipElement target;
    private Vector3 pathSuccess;
    private HitStatus hitStatus;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
    }

    public void initialize(Ammunition ammunition, ShipElement target, Vector3 pathSuccess)
    {
        this.ammunition = ammunition;
        this.pathSuccess = pathSuccess;
        this.target = target;

        float value = UnityEngine.Random.value;
        if (value < pathSuccess.x)
        {
            this.hitStatus = HitStatus.HIT;
        } else if (value < pathSuccess.y)
        {
            this.hitStatus = HitStatus.FAIL;
        } else
        {
            this.hitStatus = HitStatus.MISS;
        }
    }

    /** GETTERS **/
    public Ammunition getAmmunition()
    {
        return this.ammunition;
    }

    public ShipElement getTarget()
    {
        return this.target;
    }

    public Vector3 getPathSuccess()
    {
        return this.pathSuccess;
    }

    public HitStatus getHitStatus()
    {
        return this.hitStatus;
    }
}
