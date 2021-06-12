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
    public bool hasEgg;
    public float eggSpeed;
    public GameObject egg;
    public float normalSpeed;
    public float pickupRange;
    public LayerMask eggs;

    protected override void Start()
    {
        base.Start();
        if (hasEgg) moveSpeed = eggSpeed;
    }

    protected override void Update()
    {
        base.Update();
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
        warmth = penguinsInWarmthRange.Count / 6f;
        //if (warmth > 1) warmth = 1;
        ani.SetBool("hasEgg", hasEgg);
    }

    public void PickUpEgg()
    {
        if (Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 0.4f), pickupRange, eggs))
        {
            Destroy(Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 0.4f), pickupRange, eggs).gameObject);
            moveSpeed = eggSpeed;
            hasEgg = true;
        }
    }

    public void DropEgg() 
    {
        if (hasEgg)
        {
            //dropEgg;
            GameObject newEgg = Instantiate(egg, transform.GetChild(0).position, Quaternion.identity);
            hasEgg = false;
            moveSpeed = normalSpeed;
        }
    }
}
