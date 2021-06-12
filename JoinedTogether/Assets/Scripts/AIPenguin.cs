using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPenguin : Penguin
{
    public override void Movement()
    {
        move = Vector2.zero;
        Vector2 penguinsCenter = Vector2.zero;
        foreach(GameObject pingu in penguinsInRange) 
        {
            penguinsCenter += (Vector2)pingu.transform.position;
        }
        penguinsCenter /= penguinsInRange.Count;
        move = (penguinsCenter - (Vector2)transform.position).normalized*moveSpeed*Time.deltaTime*(1-currentWarmth);
    }

    
}
