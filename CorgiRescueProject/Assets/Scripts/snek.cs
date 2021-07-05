using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snek : Living
{
    private Quaternion rot;
    // Start is called before the first frame update
    private float timer;
    [SerializeField]
    private float rotateTime;
    private bool hissed = false;
    Coroutine coroutine;
    private RaycastHit2D hit;

    private IEnumerator Triggered()
    {
        attacking = true;
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
        while (Vector2.Distance(hit.point, transform.position) > 0.2 && this.enabled)
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
        attacking = false;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (!attacking)
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
