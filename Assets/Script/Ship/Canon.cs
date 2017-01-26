using UnityEngine;
using System.Collections;
using System;

public class Canon : ShipElement {
    private int power = 5;       // Random number
    private int damage = 5;      // Random number
    private int viewFinder = 0;  // Random number

    // Use this for initialization
    void Start()
    {
        life = 50;
    }

    public void destroyCanon() {
        // Destroy all Components
        if (GetComponent<SpriteRenderer>())
            Destroy(GetComponent<SpriteRenderer>());
        if (GetComponent<Target>())
            Destroy(GetComponent<Target>());
    }

    string getName() {
        return name;
    }

    int getPower() {
        return power;
    }

    int getDamage() {
        return damage;
    }

    int getViewFinder() {
        return viewFinder;
    }


    public void onMouseOver()
    {
        print("mouse over " + name);
    }

    protected override void doRepairAction()
    {
    }

    protected override void doDamageAction()
    {
    }

    protected override void doDamageAnimation()
    {
        ParticleSystem canonShotExplosion = (ParticleSystem)transform.Find("CanonShotExplosion/PS_CanonShotExplosion").gameObject.GetComponent<ParticleSystem>();
        canonShotExplosion.Play();
    }

    protected override int receiveDamageAction(int damage)
    {
        this.setLife(this.life - damage);
        return 10;
    }

    protected override void receiveDamageAnimation()
    {
        ParticleSystem targetExplosion = transform.Find("BoatExplosion/PS_BoatExplosion").gameObject.GetComponent<ParticleSystem>();
        targetExplosion.Play();
    }
}
