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
        this.setCurrentLife(life);
    }

    public void destroyCanon() {
        // Destroy all Components
        if (GetComponent<SpriteRenderer>())
            Destroy(GetComponent<SpriteRenderer>());
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

    /** ON HIT EFFECT **/
    protected override void dealDamageAsRepercution(int damage)
    {
    }

    protected override void dealDamageOnDestroy()
    {
        this.GetComponentInParent<Battle_Ship>().receiveDamage(20);
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
        ParticleSystem canonShotExplosion = (ParticleSystem)transform.Find("CanonShotExplosion/PS_CanonShotExplosion").gameObject.GetComponent<ParticleSystem>();
        canonShotExplosion.Play();
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
