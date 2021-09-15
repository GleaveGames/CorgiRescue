﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagesPlayer : MonoBehaviour
{

    [SerializeField]
    private bool trigger;
    [SerializeField]
    private int damage;
    public bool canHurt = true;
    private playerStats ps;
    [SerializeField]
    private float knockbackTime = 0.6f;
    public bool canHurtPlayer = true;


    [Header("For Bullets")]
    [SerializeField]
    private bool destroyOnCollision;
    [SerializeField]
    private ParticleSystem collisionParticles;
    private cameraoptions co;
    [SerializeField]
    bool spawnsObjOnCollision;
    [SerializeField]
    GameObject collisionObject;


    private void Start()
    {
        ps = FindObjectOfType<playerStats>();
        co = FindObjectOfType<cameraoptions>();
        //just setting knockback time here cus cba to do it for everything
        knockbackTime = 0.6f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (canHurt)
        { 
            if (canHurtPlayer)
            {
                if (collision.gameObject.CompareTag("Player"))
                {
                    co.shakeDuration = 0.08f;
                    if (!collision.gameObject.GetComponent<playerMovement>().invinc)
                    {
                        FindObjectOfType<AudioManager>().Play("PlayerGrunt", collision.gameObject.transform.position, true);
                        GameObject player = collision.gameObject;
                        ps.health -= damage;
                        player.GetComponent<playerMovement>().invinc = true;
                        player.GetComponent<playerMovement>().StartInvinc();
                        player.GetComponent<playerMovement>().KnockBack(knockbackTime, (collision.gameObject.transform.position - transform.position).normalized * 7f);
                        player.GetComponent<CanPickUp>().Drop();
                    }
                }
            }            
        }
        if(destroyOnCollision)
        {
            if (spawnsObjOnCollision)
            {
                Instantiate(collisionObject, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(collisionParticles, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (trigger)
        {
            if (canHurt)
            {
                if (canHurtPlayer)
                {
                    if (collision.gameObject.CompareTag("Player"))
                    {
                        co.shakeDuration = 0.08f;
                        if (!collision.gameObject.GetComponent<playerMovement>().invinc)
                        {
                            FindObjectOfType<AudioManager>().Play("PlayerGrunt", collision.gameObject.transform.position, true);
                            GameObject player = collision.gameObject;
                            ps.health -= damage;
                            player.GetComponent<playerMovement>().invinc = true;
                            player.GetComponent<playerMovement>().StartInvinc();
                            player.GetComponent<playerMovement>().KnockBack(knockbackTime, (collision.gameObject.transform.position - transform.position).normalized * 7f);
                            player.GetComponent<CanPickUp>().Drop();
                            Debug.Log("notInvinc");
                        }
                    }
                }
            }
            if (destroyOnCollision)
            {
                if (spawnsObjOnCollision)
                {
                    Instantiate(collisionObject, transform.position, Quaternion.identity);
                }
                else
                {
                    Instantiate(collisionParticles, transform.position, Quaternion.identity);
                }
                Destroy(gameObject);
            }
        }
    }
}
