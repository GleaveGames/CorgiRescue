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
    AudioSource step;
    [SerializeField]
    bool isPlayer;

    protected override void Start()
    {
        base.Start();
        if (hasEgg) moveSpeed = eggSpeed;
        snow = transform.Find("Snow").GetComponent<SpriteRenderer>();
        if (isPlayer)
        {
            step = GetComponent<AudioSource>();
        }
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
        warmth += (currentWarmth - 0.65f)*warmthLossSpeed*Time.deltaTime;
        ani.SetBool("hasEgg", hasEgg);
        SnowOnHead();

        if(Vector2.Distance(transform.position, Vector2.zero) > 15) 
        {
            if (isPlayer)
            {
                FindObjectOfType<Canvas>().GetComponent<Animator>().Play("CanvasSeal");
            }
            ani.speed = 1;
            dead = true;
            ani.SetTrigger("Dead");
            rb.isKinematic = true;
            rb.velocity = Vector2.zero;
            gameObject.tag = "Untagged";
            FindObjectOfType<GameManager>().penguins.Remove(gameObject);
            this.enabled = false;
        }
    }

    private void SnowOnHead() 
    {
        if (!dead) 
        {
            if (warmth > 0.5f)
            {
                warmth = 0.5f;
            }
            if (warmth > 0)
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
            if (warmth < -1.2f)
            {
                if (isPlayer) 
                {
                    FindObjectOfType<Canvas>().GetComponent<Animator>().Play("CanvasYouFroze");
                }
                //die;
                ani.speed = 1;
                dead = true;
                ani.SetTrigger("Dead");
                rb.isKinematic = true;
                rb.velocity = Vector2.zero;
                gameObject.tag = "Untagged";
                FindObjectOfType<GameManager>().penguins.Remove(gameObject);
                this.enabled = false;
            }
        }
    }

    public void PickUpEgg()
    {
        if (Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 0.4f), pickupRange, eggs))
        {
            Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 0.4f), pickupRange, eggs).GetComponent<Egg>().StopAllCoroutines();
            Destroy(Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 0.4f), pickupRange, eggs).gameObject);
            moveSpeed = eggSpeed;
            hasEgg = true;
        }
    }

    public GameObject DropEgg() 
    {
        GameObject newEgg = null;
        if (hasEgg)
        {
            //dropEgg;
            newEgg = Instantiate(egg, transform.GetChild(0).position, Quaternion.identity);
            hasEgg = false;
            moveSpeed = normalSpeed;
        }
        return newEgg;
    }

    public void PlayStep()
    {
        if (isPlayer) 
        {
            step.pitch = Random.Range(0.9f, 1.1f);
            step.Play();
        }
    }
}
