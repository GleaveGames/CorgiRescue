using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomby : MonoBehaviour
{

    private string currentState;
    private Animator ani;
    public bool triggered;
    private bool moving;
    private bool currentlymoving;
    Coroutine coroutine;
    private Vector2 targetPos;
    [SerializeField]
    private float speed = 2;
    void Start()
    {
        ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!triggered)
        {
            if (!moving)
            {
                StartCoroutine("WalkStop");
                moving = true;
            }
            else
            {
                if (currentlymoving)
                {
                    transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
                    if(Vector2.Distance(transform.position, targetPos) < 0.1)
                    {
                        ChangeAnimationState("BombyIdle");
                    }
                }
                else
                {
                    ChangeAnimationState("BombyIdle");
                }
            }
            if (transform.parent != null)
            {
                if (transform.parent.name.Contains("Hand"))
                {
                    triggered = true;
                }
            }            
        }
        else
        {
            ChangeAnimationState("BombyExplode");
            //explode and walk towards whatever triggered it
        }

        //when damaged trigger
        //if picked up trigger
        
    }

    void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        ani.Play(newState);
        currentState = newState;
    }

    private IEnumerator WalkStop()
    {
        ChangeAnimationState("BombyRun");
        targetPos = transform.position;
        targetPos.x += Random.Range(-2f, 2f);
        targetPos.y += Random.Range(-2f, 2f);
        currentlymoving = true;
        yield return new WaitForSeconds(Random.Range(1.5f,2.4f));
        currentlymoving = false;
        yield return new WaitForSeconds(Random.Range(1.5f, 2.4f));
        moving = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        targetPos = transform.position;
        targetPos.x += Random.Range(-2f, 2f);
        targetPos.y += Random.Range(-2f, 2f);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        targetPos = transform.position;
        targetPos.x += Random.Range(-2f, 2f);
        targetPos.y += Random.Range(-2f, 2f);
    }
}
