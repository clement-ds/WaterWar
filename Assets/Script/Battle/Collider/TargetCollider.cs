using System;
using UnityEngine;

public class TargetCollider : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Battle_CanonBall canonBall = col.gameObject.GetComponent<Battle_CanonBall>();
        if (canonBall)
        {
            float value = UnityEngine.Random.value;
            //Debug.Log(this + " (" + this.transform.GetInstanceID() + ") == (" + canonBall.getTarget().transform.GetInstanceID() + ")  --> " + canonBall.getHitStatus());
            if ((canonBall.getHitStatus() == HitStatus.HIT && canonBall.getTarget().transform.GetInstanceID() == this.transform.GetInstanceID())
                || (canonBall.getHitStatus() == HitStatus.FAIL && canonBall.getTarget().transform.GetInstanceID() != this.transform.GetInstanceID()))
            {
                ShipElement target = this.transform.GetComponent<ShipElement>();
                if (target != null)
                {
                    target.receiveDamage(canonBall);
                }
                else
                {
                    Battle_Ship target2 = this.transform.GetComponent<Battle_Ship>();
                    if (target2 != null)
                    {
                        target2.receiveDamage((canonBall.getAmmunition().getDamage() / 3));
                    }
                }
                Destroy(col.gameObject);
            }
        }
    }

}