using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AIPenguin : Penguin
{
    [SerializeField]
    GameObject ClosestEgg;
    public override void Movement()
    {
        if (hasEgg)
        {
            move = Vector2.zero;
            Vector2 penguinsCenter = Vector2.zero;
            foreach(GameObject pingu in penguinsInRange) 
            {
                penguinsCenter += (Vector2)pingu.transform.position;
            }
            penguinsCenter /= penguinsInRange.Count;
            move = (penguinsCenter - (Vector2)transform.position).normalized*moveSpeed*Time.deltaTime*(1.3f-currentWarmth);
        }
        else 
        {
            GameObject[] eggs = GameObject.FindGameObjectsWithTag("Eggs");
            if(eggs.Length > 0) 
            {
                GameObject closestEgg = eggs[0];
                foreach (GameObject egg in eggs)
                {
                    if (Vector2.Distance(transform.position, closestEgg.transform.position) > Vector2.Distance(transform.position, egg.transform.position)) closestEgg = egg;
                }
                move = ((Vector2)closestEgg.transform.position - (Vector2)transform.position).normalized * moveSpeed * Time.deltaTime * (1.3f - currentWarmth);
                PickUpEgg();
            }
            else 
            {
                move = Vector2.zero;
                Vector2 penguinsCenter = Vector2.zero;
                foreach (GameObject pingu in penguinsInRange)
                {
                    penguinsCenter += (Vector2)pingu.transform.position;
                }
                penguinsCenter /= penguinsInRange.Count;
                move = (penguinsCenter - (Vector2)transform.position).normalized * moveSpeed * Time.deltaTime * (1.3f - currentWarmth);
            }
        }
    }
}
