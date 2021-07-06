using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damagable : MonoBehaviour
{
    Living living;
    [SerializeField] 
    private bool SK = false;
    [SerializeField] 
    private bool mole = false;
    [SerializeField] 
    private bool mong = false;
    [SerializeField] 
    private bool bomby = false;
    [SerializeField]
    private ParticleSystem littleBlood;
    [SerializeField]
    private float stuntime = 3f;

    [Header("Damaged By: ")]
    [SerializeField]
    private bool Living = true;  
    [SerializeField]
    private bool PickUpItems = true;
    [SerializeField]
    private bool Pick = true;
    [SerializeField]
    private bool Bombs = true;
    [SerializeField]
    private bool Traps = true;
    [SerializeField]
    private GameObject Ducks;
    Coroutine coroutine;
    PickUpEnemy pickupenemy;

    private void Start()
    {
        living = GetComponent<Living>();
        if (TryGetComponent(out PickUpEnemy pue)) pickupenemy = pue;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((Living && collision.gameObject.CompareTag("Living")) ||
            (PickUpItems && collision.gameObject.CompareTag("PickupItems")) ||
            (Pick && collision.gameObject.CompareTag("Pick")) ||
            (Bombs && collision.gameObject.name == "Bomb(Clone)") || 
            (Traps && collision.gameObject.CompareTag("Trap")))
        {
                DoDamage(collision.gameObject);
        }

        //JUST ADDED THIS SO THAT YOU CAN'T BUM RUSH SK 
        if (collision.gameObject.CompareTag("SK"))
        {
            if (collision.gameObject.GetComponent<SKMovement>().angered)
            {
                DoDamage(collision.gameObject);
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((Living && collision.gameObject.CompareTag("Living")) ||
            (Pick && collision.gameObject.CompareTag("Pick")) ||
            (Bombs && collision.gameObject.name == "Bomb(Clone)") ||
            (Traps && collision.gameObject.CompareTag("Trap")))
        {
            DoDamage(collision.gameObject);
        }

        if (PickUpItems)
        {
            if (collision.gameObject.CompareTag("PickupItems"))
            {
                if (SK)
                {
                    if (collision.gameObject.GetComponent<DamagesPlayer>().canHurt)
                    {
                        DoDamage(collision.gameObject);
                    }
                }
                else
                {
                    DoDamage(collision.gameObject);
                }
            }
        }
    }

    private void Anger(GameObject target)
    {
        if (SK)
        {
            if (target.CompareTag("Pick"))
            {
                target = target.transform.root.gameObject;
            }
            GetComponent<SKMovement>().target = target; 
            GetComponent<SKMovement>().angered = true;
            GetComponent<SKPickUp>().Drop();
        }
        living.angered = true;
    }

    private void DoDamage(GameObject collisionObj)
    {
        if (bomby)
        {
            GetComponent<Bomby>().triggered = true;
        }
        else
        {
            living.health -= collisionObj.GetComponent<DamageThisDoes>().damage;
            Instantiate(littleBlood, transform.position, Quaternion.identity);
            Anger(collisionObj);
            StartCoroutine(Dizzy());
        }
    }

    private IEnumerator Dizzy()
    {
        if (!living.stunnable) yield break;
        if (living.health < 1) living.Die();
        FindObjectOfType<LevelGenerator>().itemsForPickUp.Add(gameObject);
        living.stunned = true;
        living.attacking = false;
        if(TryGetComponent(out DamagesPlayer damplay)) 
        {
            damplay.canHurt = false;
        }
        if (transform.childCount > 0)
        {
            if (transform.GetChild(transform.childCount-1).gameObject.name.Contains("Duck"))
            {
                yield break;
            }
        }
        GameObject ducko = Instantiate(Ducks, transform.position, Quaternion.identity);
        ducko.transform.parent = transform;
        if (SK) 
        {
            transform.GetChild(0).GetComponent<Animator>().enabled = false;
            GetComponent<SKMovement>().canMove = false;
            yield return new WaitForSeconds(stuntime);
            GetComponent<SKMovement>().canMove = true;
            transform.GetChild(0).GetComponent<Animator>().enabled = true;
        }
        else if(TryGetComponent(out Living liv))
        {
            GetComponent<Animator>().enabled = false;
            liv.enabled = false;
            yield return new WaitForSeconds(stuntime);
            liv.enabled = true;
            GetComponent<Animator>().enabled = true;
        }
            
        if (TryGetComponent(out DamagesPlayer damplay2))
        {
            damplay2.canHurt = true;
        }
        if (transform.parent != null) 
        {
            if (living.pickupable) GetComponent<PickUpEnemy>().EnableCollision();
        }
        Destroy(ducko);
        living.stunned = false;
        if (living.pickupable)
        {
            FindObjectOfType<LevelGenerator>().itemsForPickUp.Remove(gameObject);
        }
        living.attacking = false;
    }
}
