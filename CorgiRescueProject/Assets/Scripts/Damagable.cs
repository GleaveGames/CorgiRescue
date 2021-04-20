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
            cs.health -= collisionObj.GetComponent<DamageThisDoes>().damage;
            Instantiate(littleBlood, transform.position, Quaternion.identity);
            Anger(collisionObj);
            StartCoroutine(Dizzy());
        }
    }

    private IEnumerator Dizzy()
    {
        if (transform.childCount > 0)
        {
            if (transform.GetChild(transform.childCount-1).gameObject.name.Contains("Duck"))
            {
                yield break;
            }
        }
        GameObject ducko = Instantiate(Ducks, transform.position, Quaternion.identity);
        ducko.transform.parent = transform;
        if (!SK) 
        {
            GetComponent<Animator>().enabled = false;
        }
        else 
        {
            transform.GetChild(0).GetComponent<Animator>().enabled = false;
        }
        if (mole)
        {
            GetComponent<Mole>().enabled = false;
            yield return new WaitForSeconds(stuntime);
            GetComponent<Mole>().enabled = true;
        }
        else if (TryGetComponent(out snek s))
        {
            s.enabled = false;
            yield return new WaitForSeconds(stuntime);
            s.enabled = true; 
        }
        else if (TryGetComponent(out Bat b)) 
        {
            b.enabled = false;
            transform.position = transform.position;
            yield return new WaitForSeconds(stuntime);
            b.enabled = true;
        }
        else if(TryGetComponent(out Penguin pen))
        {
            pen.enabled = false;
            yield return new WaitForSeconds(stuntime);
            pen.enabled = true;
        }
        else if (SK) 
        {
            GetComponent<SKMovement>().canMove = false;
            yield return new WaitForSeconds(stuntime);
            GetComponent<SKMovement>().canMove = true;
        }
        if (!SK)
        {
            GetComponent<Animator>().enabled = true;
        }
        else
        {
            transform.GetChild(0).GetComponent<Animator>().enabled = true;
        }
        Destroy(ducko);
    }
}
