using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Penguin : Living
{
    public float warmth;
    public List<GameObject> penguinsInRange;
    [SerializeField]
    float penguinRange;
    public List<GameObject> penguinsInWarmthRange;
    [SerializeField]
    float penguinWarmthRange;
    public float currentWarmth;
    public bool hasEgg;
    public float eggSpeed;
    public GameObject egg;
    public float normalSpeed;
    public float pickupRange;
    public LayerMask eggs;
    private SpriteRenderer snow;
    [SerializeField]
    float warmthLossSpeed;
    [SerializeField]
    Sprite[] snowSprites;

    protected override void Start()
    {
        base.Start();
        if (hasEgg) moveSpeed = eggSpeed;
        snow = transform.Find("Snow").GetComponent<SpriteRenderer>();
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
        currentWarmth = penguinsInWarmthRange.Count / 6f;
        warmth += (currentWarmth - 0.5f)*warmthLossSpeed;
        ani.SetBool("hasEgg", hasEgg);
        SnowOnHead();
    }

    private void SnowOnHead() 
    {
        if(warmth > 0.5f) 
        {
            warmth = 0.5f;
        }
        if(warmth > 0) 
        {
            snow.sprite = snowSprites[0];
        }
        else if (warmth < 0 && warmth > -0.2f) 
        {
            snow.sprite = snowSprites[1];
        }
        else if (warmth < -0.2 && warmth > -0.4f) 
        {
            snow.sprite = snowSprites[2];
        }
        else if (warmth < -0.4 && warmth > -0.6f) 
        {
            snow.sprite = snowSprites[3];
        }
        else if (warmth < -0.6 && warmth > -1.2f) 
        {
            snow.sprite = snowSprites[4];
        }
        if(warmth < -1.2f) 
        {
            //die;
            ani.Play("PenguinDeath");
            GetComponent<Living>().enabled = false;
            rb.isKinematic = true;
            rb.velocity = Vector2.zero;
            gameObject.tag = "Untagged";
        }

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
