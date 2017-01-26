using UnityEngine;
using System.Collections;
using System;

public class Canteen : ShipElement {

	// Use this for initialization
	void Start () {
        life = 100;
	}

    protected override void doRepairAction()
    {
    }

    protected override void doDamageAction()
    {
    }

    protected override void doDamageAnimation()
    {
    }

    protected override int receiveDamageAction(int damage)
    {
        this.setLife(this.life - damage);
        return 100;
    }

    protected override void receiveDamageAnimation()
    {
        ParticleSystem targetExplosion = transform.Find("BoatExplosion/PS_BoatExplosion").gameObject.GetComponent<ParticleSystem>();
        targetExplosion.Play();
    }
}
