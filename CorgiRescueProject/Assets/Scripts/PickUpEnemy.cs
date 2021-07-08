using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpEnemy : PickUpBase
{
    Living living;

    protected override void Start()
    {
        base.Start();
        living = GetComponent<Living>();
    }

    public override void DisableCollision()
    {
        base.DisableCollision();
        living.pickedUp = true;
        living.attacking = false;
    }

    public override void EnableCollision()
    {
        if (living.stunned) 
        {
            GetComponent<DamagesPlayer>().canHurt = false;
            transform.parent = null;
            rb.isKinematic = false;
            cc.enabled = true;
            cc.isTrigger = false;
            lg.itemsForPickUp.Add(gameObject);
        }
        else 
        {
            base.EnableCollision();
            living.pickedUp = false;
            living.attacking = false;
        }
    }
}
