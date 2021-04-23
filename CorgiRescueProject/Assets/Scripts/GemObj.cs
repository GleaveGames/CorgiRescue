using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemObj : MonoBehaviour
{
    //Maybe do something with layer idk
    [Header("Diamond")]
    [SerializeField]
    Sprite[] DiamondSprites;
    [SerializeField]
    int DiamondWorth;
    
    [Header("Gold")]
    [SerializeField]
    Sprite[] GoldSprites;
    [SerializeField]
    int GoldWorth;
    
    [Header("Silver")]
    [SerializeField]
    Sprite[] SilverSprites;
    [SerializeField]
    int SilverWorth;

    private int worth;
    bool canSuck = false;
    Coroutine coroutine;
    public Transform Player;
    private Rigidbody2D rb;
    string sound;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(SuckDelay());
    }

    private IEnumerator SuckDelay() 
    {
        yield return new WaitForSeconds(0.6f);
        canSuck = true;
    }

    void Update()
    {
        //Gem movement 
        if (canSuck) 
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Player.position - transform.position );
            if (hit.collider.gameObject.CompareTag("Player")) 
            {
                if (Vector2.Distance(Player.position, transform.position) < 0.8)
                {
                    GetComponent<CircleCollider2D>().isTrigger = true;
                }
                rb.AddForce((Player.position - transform.position) * 100 / (Mathf.Pow(Vector2.Distance(transform.position, Player.position) + 1, 5)));
            }
        }
    }

    public void Type(int type)
    {
        if (type == 1) 
        {
            //diamond
            GetComponent<SpriteRenderer>().sprite = DiamondSprites[Random.Range(0, DiamondSprites.Length)];
            worth = DiamondWorth;
            sound = "Diamond";
        }
        else if (type == 2) 
        {
            GetComponent<SpriteRenderer>().sprite = GoldSprites[Random.Range(0, GoldSprites.Length)];
            worth = GoldWorth;
            sound = "Gold";
        }
        else if (type == 3)
        {
            GetComponent<SpriteRenderer>().sprite = SilverSprites[Random.Range(0, SilverSprites.Length)];
            worth = SilverWorth;
            sound = "Silver";
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (canSuck) 
        {
            if(collision.gameObject.CompareTag("Player"))
            {
                Collect();
            }
        }
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canSuck)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Collect();
            }
        }
    }

    private void Collect()
    {
        FindObjectOfType<AudioManager>().Play(sound, transform.position, false);
        FindObjectOfType<playerStats>().money += worth;
        Destroy(gameObject);
    }
}
