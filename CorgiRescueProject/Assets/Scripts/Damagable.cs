using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damagable : MonoBehaviour
{
    private CharacterStats cs;
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
    
    private void Start()
    {
        cs = GetComponent<CharacterStats>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Living)
        {
            if(collision.gameObject.CompareTag("Living"))
            {
                DoDamage(collision.gameObject);
            }
        }
        if (PickUpItems)
        {
            if (collision.gameObject.CompareTag("PickupItems"))
            {
                DoDamage(collision.gameObject);
            }
        }
        if (Pick)
        {
            if (collision.gameObject.CompareTag("Pick"))
            {
                DoDamage(collision.gameObject);
            }
        } 
        if (Bombs)
        {
            if (collision.gameObject.name == "Bomb(Clone)")
            {
                DoDamage(collision.gameObject);
            }
        }
        if (Traps)
        {
            if (collision.gameObject.CompareTag("Trap"))
            {
                DoDamage(collision.gameObject);
            }
        }        
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Living)
        {
            if(collision.gameObject.CompareTag("Living"))
            {
                DoDamage(collision.gameObject);
            }
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
        if (Pick)
        {
            if (collision.gameObject.CompareTag("Pick"))
            {
                DoDamage(collision.gameObject);
            }
        } 
        if (Bombs)
        {
            if (collision.gameObject.name == "Bomb(Clone)")
            {
                DoDamage(collision.gameObject);
            }
        }
        if (Traps)
        {
            if (collision.gameObject.CompareTag("Trap"))
            {
                DoDamage(collision.gameObject);
            }
        }        
    }

    private void Anger()
    {
        if (SK)
        {
            GetComponent<SKMovement>().angered = true;
            GetComponent<SKPickUp>().Drop();
        }
        if (mole)
        {
            GetComponent<Mole>().angered = true;
        }
        if (mong)
        {
            GetComponent<Mongoose>().angered = true;
        }
    }

    private void DoDamage(GameObject collisionObj)
    {
        if (bomby)
        {
            GetComponent<Bomby>().triggered = true;
        }
        else
        {
            print(collisionObj.name);
            cs.health -= collisionObj.GetComponent<DamageThisDoes>().damage;
            Instantiate(littleBlood, transform.position, Quaternion.identity);
            Anger();
        }
        
    }
}
