using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpEnemy : PickUpBase
{
    private Living living;

    protected override void Start()
    {
        living = GetComponent<Living>();
    }

}
