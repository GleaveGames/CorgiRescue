using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour
{
    string currentState;
    Animator ani;
    Coroutine coroutine;
    bool triggered;
    [SerializeField]
    float walkTime;
    [SerializeField]
    float idleTime;
    [SerializeField]
    float movespeed;
    bool walking;
    //1 is upleft, 2 is upright, 3 is downright, 4 is downleft;
    int direction;
    [SerializeField]
    float bumprange;


    private void Start()
    {
        ani = GetComponent<Animator>();
        direction = Random.Range(1, 5);
        Rotate();
        StartCoroutine(Idle());
    }

    private void FixedUpdate()
    {
        if (walking) 
        {
            RaycastHit2D hitforward = Physics2D.Raycast(transform.position, transform.up);
            if (Vector2.Distance(hitforward.point, transform.position) < bumprange) 
            {
                if (hitforward.normal == Vector2.right)
                {
                    if (direction == 1) direction = 2;
                    else if (direction == 4) direction = 3;
                }
                else if (hitforward.normal == Vector2.down)
                {
                    if (direction == 1) direction = 4;
                    else if (direction == 2) direction = 3;
                }
                else if (hitforward.normal == Vector2.left)
                {
                    if (direction == 2) direction = 1;
                    else if (direction == 3) direction = 4;
                }
                else if (hitforward.normal == Vector2.up)
                {
                    if (direction == 3) direction = 2;
                    else if (direction == 4) direction = 1;
                }
                else 
                {
                    direction = Random.Range(1, 5);
                }
                Rotate();
            }
        }
    }

    private IEnumerator Walk() 
    {
        float timer = 0;
        walking = true;
        ChangeAnimationState("SpiderWalk");
        while (!triggered && timer < walkTime)
        {
            transform.position = Vector2.MoveTowards(transform.position, transform.position + transform.up, movespeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }
        if (triggered)
        {
            walking = false;
            yield break;
        }
        else 
        {
            StartCoroutine(Idle());
        }
    }

    private IEnumerator Idle() 
    {
        walking = false;
        float timer = 0;
        ChangeAnimationState("SpiderIdle");
        while (!triggered && timer < idleTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        if (triggered)
        {
            yield break;
        }
        else
        {
            StartCoroutine(Walk());
        }
    }

    private void Rotate() 
    {
        if (direction == 1)
        {
            transform.up = Vector2.up + Vector2.left;
        }
        else if (direction == 2)
        {
            transform.up = Vector2.up + Vector2.right;
        }
        else if (direction == 3)
        {
            transform.up = Vector2.down + Vector2.right;
        }
        else if (direction == 4)
        {
            transform.up = Vector2.down + Vector2.left;
        }
        else Debug.Log("invalid direction for spider");
    }




    void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;

        ani.Play(newState);

        currentState = newState;
    }


}
