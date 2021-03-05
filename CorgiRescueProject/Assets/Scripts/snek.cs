using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snek : MonoBehaviour
{
    private GameObject player;
    [SerializeField]
    private bool triggered;
    [SerializeField]
    private float speed;
    private Animator ani;
    private string currentState;
    private Quaternion rot;
    // Start is called before the first frame update
    private float timer;
    [SerializeField]
    private float rotateTime;
    private bool hissed = false;
    Coroutine coroutine;
    private RaycastHit2D hit;


    void Start()
    {        
        player = GameObject.FindGameObjectWithTag("Player");
        ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        //
        if (!triggered)
        {
            RaycastHit2D closestHitR = ClosestRaycast(transform.right);
            if (closestHitR.collider.gameObject.CompareTag("Player"))
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                hit = closestHitR;
                triggered = true;
            }
            //
            RaycastHit2D closestHitL = ClosestRaycast(-transform.right);
            if (closestHitL.collider.gameObject.CompareTag("Player"))
            {
                transform.rotation = Quaternion.Euler(0, 0, 180);
                hit = closestHitL;
                triggered = true;
            }
            //
            RaycastHit2D closestHitU = ClosestRaycast(transform.up);
            if (closestHitU.collider.gameObject.CompareTag("Player"))
            {
                transform.rotation = Quaternion.Euler(0, 0, 90);
                hit = closestHitU;
                triggered = true;
            }
            //
            RaycastHit2D closestHitD = ClosestRaycast(-transform.up);
            if (closestHitD.collider.gameObject.CompareTag("Player"))
            {
                transform.rotation = Quaternion.Euler(0, 0, 270);
                hit = closestHitD;
                triggered = true;
            }
        }        
        //
        else
        {
            if(Vector2.Distance(hit.point, transform.position) > 0.2)
            {
                if (!hissed)
                {
                    StartCoroutine("Hiss");
                    hissed = true;
                }
                transform.position = Vector2.MoveTowards(transform.position, hit.point, speed*Time.deltaTime);
                ChangeAnimationState("Snek");
               
            }
            else
            {
                transform.rotation = Quaternion.identity;
                
                ChangeAnimationState("snekIdle");
       
                triggered = false;
            }
        }       
    }
    void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        ani.Play(newState);
        currentState = newState;
    }

    private IEnumerator Hiss()
    {        
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(3);
        hissed = false;
    }

    private RaycastHit2D ClosestRaycast(Vector2 direction)
    {
        RaycastHit2D[] hitright = Physics2D.RaycastAll(transform.position, direction);
        RaycastHit2D closestValidHit = new RaycastHit2D();
        foreach (RaycastHit2D hit in hitright)
        {
            if (hit.transform.gameObject != gameObject && (closestValidHit.collider == null || closestValidHit.distance > hit.distance))
            {
                closestValidHit = hit;
            }
        }
        return closestValidHit;
    }
}
