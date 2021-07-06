using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snek : Living
{
    [SerializeField]
    Coroutine coroutine;
    private RaycastHit2D hit;
    [SerializeField]
    AnimationCurve telegraphJuice;
    [SerializeField]
    float telegrpahTime;

    private IEnumerator Triggered()
    {
        //could clean this up lots with an animation curve
        attacking = true;
        float counter = 0;
        while(counter <= telegrpahTime) 
        {
            transform.localScale = new Vector2(telegraphJuice.Evaluate(counter / telegrpahTime), telegraphJuice.Evaluate(counter / telegrpahTime));
            counter += Time.deltaTime;
            yield return null;
        }
        transform.localScale = new Vector2(1, 1);
        while (Vector2.Distance(hit.point, transform.position) > 0.2 && this.enabled)
        {
            if (!attackSoundPlayed)
            {
                StartCoroutine(AttackSound());
                attackSoundPlayed = true;
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
        if (!attacking && !stunned)
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

    
}
