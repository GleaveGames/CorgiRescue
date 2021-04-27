﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beetle : MonoBehaviour
{
    bool idle, turning, midturn;
    float bumprange;
    float movespeed;
    Coroutine coroutine;
    int RotationDir;
    Animator ani;
    string currentState;



    [SerializeField]
    bool Big;

    [Header("BIG BEATLE STATS")]
    [SerializeField]
    float moveSpeedBig;
    [SerializeField]
    int bigHealth = 3;
    [SerializeField]
    RuntimeAnimatorController bigAni;
    [SerializeField]
    float bumpRangeBig = 0.5f;

    [Header("LITTLE BEATLE STATS")]
    [SerializeField]
    float moveSpeedLittle;
    [SerializeField]
    int littleHealth = 1;
    [SerializeField]
    RuntimeAnimatorController littleAni;
    [SerializeField]
    float bumpRangeLittle = 0.2f;
    AudioSource crawlsound;

    private void Start()
    {
        crawlsound = GetComponent<AudioSource>();
        if(Random.Range(0,2) < 1) Big = true; 
        ani = GetComponent<Animator>();
        idle = true;
        Rerotate();
        if (Big) 
        {
            BigStats();
        }
        else 
        {
            LittleStats();
        }
    }

    private void BigStats() 
    {
        movespeed = moveSpeedBig;
        GetComponent<CharacterStats>().health = bigHealth;
        ani.runtimeAnimatorController = bigAni;
        bumprange = bumpRangeBig;
    }

    private void LittleStats() 
    {
        movespeed = moveSpeedLittle;
        GetComponent<CharacterStats>().health = littleHealth;
        ani.runtimeAnimatorController = littleAni;
        bumprange = bumpRangeLittle;
    }


    private void FixedUpdate()
    {
        if (idle) 
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up);
            RaycastHit2D hit1 = Physics2D.Raycast(transform.position - transform.right/3, transform.up);
            RaycastHit2D hit2 = Physics2D.Raycast(transform.position + transform.right/3, transform.up);
            if (Vector2.Distance(hit.point, transform.position) > bumprange && Vector2.Distance(hit1.point, transform.position) > bumprange && Vector2.Distance(hit2.point, transform.position) > bumprange && !turning)
            {
                //move forward;
                transform.position = Vector2.MoveTowards(transform.position, hit.point, movespeed * Time.deltaTime);
                midturn = false;
            }
            else
            //pause, rotate 90 left or right;
            {
                turning = true;
                if (!midturn) 
                {
                    StartCoroutine(Turn());
                }
            }
        }
    }

    private IEnumerator Turn()
    {
        crawlsound.Stop();
        if (Big) ChangeAnimationState("BeetleIdleBig");
        else ChangeAnimationState("BeetleIdleSmall");
        midturn = true;
        yield return new WaitForSeconds(1);
        Rerotate();
        yield return new WaitForSeconds(0.5f);
        turning = false;
        midturn = false;
        if (Big) ChangeAnimationState("BeetleMoveBig");
        else ChangeAnimationState("BeetleMoveSmall");
        crawlsound.Play();
    }

    private void Rerotate() 
    {
        RotationDir = Random.Range(1, 5);
        if (RotationDir == 1) transform.up = Vector2.up;
        else if (RotationDir == 2) transform.up = Vector2.right;
        else if (RotationDir == 3) transform.up = Vector2.down;
        else if (RotationDir == 4) transform.up = Vector2.left;
    }

    void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;

        ani.Play(newState);

        currentState = newState;
    }

}
