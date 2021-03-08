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

    private IEnumerator Triggered()
    {
        triggered = true;
        while(transform.localScale.x < 1.5)
        {
            transform.localScale = new Vector2(transform.localScale.x + 0.04f, transform.localScale.y + 0.04f);
            yield return null;
        }
        while(transform.localScale.x > 1)
        {
            transform.localScale = new Vector2(transform.localScale.x - 0.04f, transform.localScale.y - 0.04f);
            yield return null;
        }
        transform.localScale = new Vector2(1, 1);
        while (Vector2.Distance(hit.point, transform.position) > 0.2)
        {
            if (!hissed)
            {
                StartCoroutine("Hiss");
                hissed = true;
            }
            transform.position = Vector2.MoveTowards(transform.position, hit.point, speed * Time.deltaTime);
            ChangeAnimationState("Snek");
            yield return null;
        }
        transform.rotation = Quaternion.identity;
        ChangeAnimationState("snekIdle");
        triggered = false;
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
            RaycastHit2D closestHitR = ClosestRaycast(Vector2.right);
            if (closestHitR.collider.gameObject.CompareTag("Player"))
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                hit = ClosestWall(Vector2.right);
                StartCoroutine("Triggered");
            }
            //
            RaycastHit2D closestHitL = ClosestRaycast(-Vector2.right);
            if (closestHitL.collider.gameObject.CompareTag("Player"))
            {
                transform.rotation = Quaternion.Euler(0, 0, 180);
                hit = ClosestWall(-Vector2.right);
                StartCoroutine("Triggered");
            }
            //
            RaycastHit2D closestHitU = ClosestRaycast(Vector2.up);
            if (closestHitU.collider.gameObject.CompareTag("Player"))
            {
                transform.rotation = Quaternion.Euler(0, 0, 90);
                hit = ClosestWall(Vector2.up);
                StartCoroutine("Triggered");
            }
            //
            RaycastHit2D closestHitD = ClosestRaycast(-Vector2.up);
            if (closestHitD.collider.gameObject.CompareTag("Player"))
            {
                transform.rotation = Quaternion.Euler(0, 0, 270);
                hit = ClosestWall(-Vector2.up);
                StartCoroutine("Triggered");
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
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction);
        RaycastHit2D closestValidHit = new RaycastHit2D();
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.gameObject != gameObject && (closestValidHit.collider == null || closestValidHit.distance > hit.distance))
            {
                closestValidHit = hit;
            }
        }
        return closestValidHit;
    }

    private RaycastHit2D ClosestWall(Vector2 direction)
    {
        RaycastHit2D[] hitswall = Physics2D.RaycastAll(transform.position, direction);
        RaycastHit2D closestValidHit = new RaycastHit2D();
        Debug.DrawRay(transform.position, direction);
        foreach (RaycastHit2D hit in hitswall)
        {
            if ((hit.transform.gameObject.tag == "Wall" | hit.transform.gameObject.tag == "Obsidian" | hit.transform.gameObject.tag == "Rock") && (closestValidHit.collider == null || closestValidHit.distance > hit.distance))
            {
                closestValidHit = hit;
            }
        }
        return closestValidHit;
    }
}
