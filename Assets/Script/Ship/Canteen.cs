using UnityEngine;
using System.Collections;
using System;

public class Canteen : ShipElement {

	// Use this for initialization
	void Start () {
        life = 100;
        this.setCurrentLife(life);
    }

    /** ON HIT EFFECT **/
    protected override void dealDamageAsRepercution(int damage)
    {
        this.GetComponentInParent<Battle_Ship>().receiveDamage(damage / 2);
    }

    protected override void dealDamageOnDestroy()
    {
        this.GetComponentInParent<Battle_Ship>().receiveDamage(this.life);
    }

    protected override void applyMalusOnHit()
    {

    }

    protected override void applyMalusOnDestroy()
    {

    }

    /** REPAIR **/
    protected override void doRepairAction()
    {
        this.setCurrentLife(this.currentLife + 20);
    }

    /** DO DAMAGE **/
    protected override void doDamageAction()
    {
    }

    protected override void doDamageAnimation()
    {
    }

    /** RECEIVE DAMAGE **/
    protected override void receiveDamageAction(int damage)
    {
        this.setCurrentLife(this.currentLife - damage);
    }

    protected override void receiveDamageAnimation()
    {
        ParticleSystem targetExplosion = transform.Find("BoatExplosion/PS_BoatExplosion").gameObject.GetComponent<ParticleSystem>();
        targetExplosion.Play();
    }
}
