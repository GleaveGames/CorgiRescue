using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Penguin : Living
{
    public List<GameObject> penguinsInRange;
    [SerializeField]
    float penguinRange;
    public List<GameObject> penguinsInWarmthRange;
    [SerializeField]
    float penguinWarmthRange;
    public float warmth;

    private void Update()
    {
        for (int i = penguinsInRange.Count - 1; i >= 0; i--) penguinsInRange.RemoveAt(i);
        for (int i = penguinsInWarmthRange.Count - 1; i >= 0; i--) penguinsInWarmthRange.RemoveAt(i);
        foreach(Collider2D col in Physics2D.OverlapCircleAll(transform.position, penguinRange)) 
        {
            if (col.gameObject.tag == "Penguin") penguinsInRange.Add(col.gameObject);
        }
        foreach(Collider2D col in Physics2D.OverlapCircleAll(transform.position, penguinWarmthRange)) 
        {
            if (col.gameObject.tag == "Penguin") penguinsInWarmthRange.Add(col.gameObject);
        }
        Debug.Log(penguinsInWarmthRange.Count);
        warmth = penguinsInWarmthRange.Count / 6f;
        //if (warmth > 1) warmth = 1;
    }
}
