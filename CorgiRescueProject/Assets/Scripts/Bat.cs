using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : Living
{
    [SerializeField]
    private float swoopDist;
    private bool swooping;
    private bool swoopend;

    public LayerMask IgnoreMe;
    public bool inSwarm;
    public bool triggered;
    [SerializeField]
    private Quaternion rot;
    private Vector3 targetPos;
    private float angle;
    [SerializeField]
    private float offset;
    private float timer = 0;
    [SerializeField]
    private float rotateTime;
    private bool flapPlayed;
    [SerializeField]
    private bool coreBat;
    private Quaternion quaternion;
    private Vector3 swooppos;
    private Vector3 swooppos2;
    private bool triggerpause = true;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        quaternion = Quaternion.Euler(new Vector3(0, 0, Random.Range(1, 5) * 90 + angle + offset));
    }

    protected override void Update()
    {
        base.Update();
        if (inSwarm)
        {
            if (coreBat)
            {
                ChangeAnimationState("CoreBatFly");
            }
            else
            {
                ChangeAnimationState("BatFly");
            }
        }
        else
        {
            if (triggered) 
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up);
                if (Vector2.Distance(hit.point, transform.position) > 0.2 || triggerpause)
                {
                    if (!swooping)
                    {
                        if (Vector2.Distance(transform.position, player.transform.position) < swoopDist)
                        {
                            //swoop
                            StartCoroutine(Swoop());
                        }
                        else
                        {
                            if (coreBat)
                            {
                                ChangeAnimationState("CoreBatFly");
                            }
                            else
                            {
                                ChangeAnimationState("BatFly");
                            }
                            transform.up = player.transform.position - transform.position;
                            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                        }
                    }
                    else
                    {
                        if (Vector2.Distance(transform.position, swooppos2) < 0.2f)
                        {
                            swoopend = true;
                        }
                    }
                }
                else
                {
                    swoopend = true;
                    swooping = false;
                    triggered = false;
                    if (coreBat)
                    {
                        ChangeAnimationState("CoreBatIdle");
                    }
                    else
                    {
                        ChangeAnimationState("BatIdle");
                    }

                    Quaternion quaternion = Quaternion.LookRotation(-transform.forward, Vector3.up);
                    rot = Quaternion.Lerp(transform.rotation, quaternion, 0.3f);
                    //child.transform.eulerAngles = newRot;
                    transform.localRotation = rot;
                }
            }
            else
            {
                transform.rotation = Quaternion.identity;
                RaycastHit2D closestHitR = ClosestRaycast(Vector2.right);
                if (closestHitR.collider.gameObject.CompareTag("Player"))
                {
                    StartCoroutine(TriggerPause());
                }
                //
                RaycastHit2D closestHitL = ClosestRaycast(-Vector2.right);
                if (closestHitL.collider.gameObject.CompareTag("Player"))
                {
                    StartCoroutine(TriggerPause());
                }
                //
                RaycastHit2D closestHitU = ClosestRaycast(Vector2.up);
                if (closestHitU.collider.gameObject.CompareTag("Player"))
                {
                    StartCoroutine(TriggerPause());
                }
                //
                RaycastHit2D closestHitD = ClosestRaycast(-Vector2.up);
                if (closestHitD.collider.gameObject.CompareTag("Player"))
                {
                    StartCoroutine(TriggerPause());
                }
            }
        }
    }

    private IEnumerator TriggerPause() 
    {
        triggerpause = true;
        triggered = true;
        yield return new WaitForSeconds(0.3f);
        triggerpause = false;
    }

    private IEnumerator Swoop()
    {
        //Can use animation curve to clean this up too
        if (coreBat)
        {
            ChangeAnimationState("CoreBatIdle");
        }
        else
        {
            ChangeAnimationState("BatIdle");
        }
        swooping = true;
        swoopend = false;
        swooppos = player.transform.position;
        swooppos2 = player.transform.position + (player.transform.position-transform.position).normalized;
        while (transform.localScale.x < 1.5)
        {
            transform.localScale = new Vector2(transform.localScale.x + 0.04f, transform.localScale.y + 0.04f);
            yield return null;
        }
        while (transform.localScale.x > 1)
        {
            transform.localScale = new Vector2(transform.localScale.x - 0.04f, transform.localScale.y - 0.04f);
            yield return null;
        }
        transform.localScale = new Vector2(1, 1);
        AttackSound();
        if (coreBat)
        {
            ChangeAnimationState("CoreBatFly");
        }
        else
        {
            ChangeAnimationState("BatSwoop");
        } 
        while (!swoopend) 
        {

            transform.up = swooppos2 - transform.position;
            transform.position = Vector2.MoveTowards(transform.position, swooppos2, 2* speed * Time.deltaTime);
            yield return null;
        }
        if (coreBat)
        {
            ChangeAnimationState("CoreBatIdle");
        }
        else
        {
            ChangeAnimationState("BatIdle");
        }
        yield return new WaitForSeconds(1);
        swooping = false;
    }

}
