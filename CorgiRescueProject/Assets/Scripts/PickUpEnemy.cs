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
        base.EnableCollision();
        living.pickedUp = false;
        living.attacking = false;
    }
}
