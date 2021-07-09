using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : Living
{
    bool triggered;
    [SerializeField]
    float walkTime;
    [SerializeField]
    float idleTime;
    bool walking;
    //1 is upleft, 2 is upright, 3 is downright, 4 is downleft;
    int direction;
    [SerializeField]
    float bumprange;
    [SerializeField]
    float viewRange;
    bool checking;
    [SerializeField]
    float rotationspeed;
    [SerializeField]
    GameObject WebBall;
    [SerializeField]
    float shotpower;
    Vector2 attackpoint;

    protected override void Start()
    {
        base.Start();
        direction = Random.Range(1, 5);
        Rotate();
        StartCoroutine(Idle());
    }

    protected override void Update()
    {
        base.Update();
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

        if (!checking) 
        {
            if (Vector2.Distance(transform.position, player.position) < viewRange)
            {
                RaycastHit2D playerhit = Physics2D.Raycast(transform.position, player.position - transform.position);
                if (playerhit.collider.gameObject.transform == player)
                {
                    StartCoroutine(CanSeeCheck());
                }
            }
        }
    }

    private IEnumerator CanSeeCheck() 
    {
        checking = true;
        float timer = 0;
        RaycastHit2D playerhit = Physics2D.Raycast(transform.position, player.position - transform.position);
        while (timer < 0.3f) 
        {
            
            playerhit = Physics2D.Raycast(transform.position, player.position - transform.position);
            if (playerhit.collider.gameObject.transform != player)
            {
                checking = false;
                yield break;
            }
            timer += Time.deltaTime;
            yield return null;
        }
        triggered = true;
        StartCoroutine(Attack());
    }

    private IEnumerator Attack() 
    {
        attacking = true;
        attackpoint = player.position;
        ChangeAnimationState("SpiderAttack");
        while(attacking) 
        {
            transform.up = -Vector2.MoveTowards(-transform.up, (attackpoint - new Vector2(transform.position.x, transform.position.y)).normalized, rotationspeed * Time.deltaTime);
            yield return null;
        }
        checking = false;
        StartCoroutine(Idle());
    }

    private IEnumerator Walk() 
    {
        float timer = 0;
        walking = true;
        ChangeAnimationState("SpiderWalk");
        while (!triggered && timer < walkTime)
        {
            transform.position = Vector2.MoveTowards(transform.position, transform.position + transform.up, speed * Time.deltaTime);
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

    private void AttackingEnd() 
    {
        attacking = false;
        triggered = false;
    }

    private void ShootWeb() 
    {
        GameObject web = Instantiate(WebBall, transform.position - transform.up / 2, Quaternion.identity);
        web.GetComponent<Rigidbody2D>().AddForce((attackpoint - new Vector2(transform.position.x, transform.position.y)).normalized * shotpower, ForceMode2D.Impulse);
        
    }
}
