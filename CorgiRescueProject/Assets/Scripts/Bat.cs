using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : Living
{
    [SerializeField]
    private float swoopDist;
    private bool swooping;
    private bool swoopend;

    [SerializeField]
    LayerMask IgnoreMe;
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
    [SerializeField]
    AnimationCurve speedFlying;
    float speedFlyingCounter = 0;
    batState _currentState;
    int RotationDir;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        RotationDir = Random.Range(1, 5);
        if (RotationDir == 1) transform.up = Vector2.down;
        else if (RotationDir == 2) transform.up = Vector2.right;
        else if (RotationDir == 3) transform.up = Vector2.down;
        else if (RotationDir == 4) transform.up = Vector2.left;
        
        RaycastHit2D wall = ClosestWall(transform.up);
        transform.position = wall.point;
    }
    
    protected override void Update()
    {
        base.Update();
        if (!inSwarm)
        {
            if (canMove)
            {
                switch (_currentState)
                {
                    case batState.Idle:
                        {
                            RaycastHit2D hit = ClosestRaycast(-transform.up);
                            //RaycastHit2D wall = ClosestWall(transform.up);
                            // checking for wall isn't working atm, can't get it to work. Ideally the bat would be triggered if you destroy the thing it's attached to
                            if (hit.transform == player) { _currentState = batState.Chase; StartCoroutine(TriggerPause()); transform.up = -transform.up; }
                            ChangeAnimationState("BatIdle");
                            break;
                        }
                    case batState.Chase:
                        {
                            RaycastHit2D wall = ClosestWall(transform.up);
                            if (wall.distance > 0.2f || triggerpause)
                            {
                                ChangeAnimationState("BatFly");
                                //arbritrary turn speed below
                                transform.up = Vector2.Lerp(transform.up, player.transform.position - transform.position, 4 * Time.deltaTime);
                                if (speedFlyingCounter > 0.8f)
                                {
                                    speedFlyingCounter = 0;
                                }
                                transform.position = Vector2.MoveTowards(transform.position, player.position, (speedFlying.Evaluate(speedFlyingCounter / 0.8f) + speed) * Time.deltaTime);
                                speedFlyingCounter += Time.deltaTime;
                            }
                            else
                            {
                                _currentState = batState.Idle;
                                transform.up = -wall.normal;
                            }
                            break;
                        }
                }
            }
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
        }
    }

    





    /*
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
                //ignoring ground enemies
                RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 9999, ~IgnoreMe);
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
                            //arbritrary turn speed below
                            transform.up = Vector2.Lerp(transform.up, player.transform.position - transform.position, 4*Time.deltaTime);

                            if (speedFlyingCounter > 0.8f)
                            {
                                speedFlyingCounter = 0;
                            }
                            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speedFlying.Evaluate(speedFlyingCounter / 0.8f) + speed * Time.deltaTime);
                            speedFlyingCounter += Time.deltaTime;
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

    */

    private IEnumerator TriggerPause()
    {
        triggerpause = true;
        triggered = true;
        yield return new WaitForSeconds(0.3f);
        triggerpause = false;
    }

    public enum batState
    {
        Idle,
        Chase
    }
}
