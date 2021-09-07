using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SKMovement : Living
{
    private skState _currentState;

    Vector2 move;
    public float runsp = 1.5f;
    public float patrolspeed = 1f;
    public float angeredspeed = 1.5f;
    private GameObject child;
    public GameObject target;
    private Vector3 targetPos;
    private float angle;
    private SKPickUp pickup;
    [SerializeField]
    private float viewRange;
    public List<GameObject> livingthings;
    [SerializeField]
    private bool[] walls;
    private int targetDirection;
    [SerializeField]
    private float wallRange;
    private Vector2 tempdir;
    private float timer;
    private GameObject closestItem;
    public bool findNewItem = true;
    [SerializeField]
    private float invincTime;
    public bool invinc;
    private bool canSeeTarget;
    private float patrolCount;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        child = transform.GetChild(0).gameObject;
        ani = child.GetComponent<Animator>();
        ChangeAnimationState("SKIdle");
        pickup = GetComponent<SKPickUp>();
        //livingthings = FindObjectOfType<LevelGenerator>().livingThings;        
        pickup.viewRange = viewRange;
        if (FindObjectOfType<playerStats>().wanted)
        {
            angered = true;
        }
    }

    protected override void Update()
    {
        base.Update();
        if (canMove)
        {
            switch (_currentState)
            {
                case skState.Wander:
                {
                        if (pickup.item == null)
                        {
                            _currentState = skState.Arm;
                        }
                        if (angered)
                        {
                            _currentState = skState.Chase;
                        }




                        if (patrolCount < 2)
                        {
                            RandomlyMove();
                            runsp = patrolspeed;
                            ani.speed = 0.7f;
                            patrolCount += Time.deltaTime;
                            ChangeAnimationState("SKmove");
                        }
                        else if (patrolCount > 4)
                        {
                            patrolCount = 0;
                        }
                        else
                        {
                            patrolCount += Time.deltaTime;
                            ChangeAnimationState("SKIdle");
                        }
                        
                    break;
                }
                case skState.Arm:
                {
                        if(pickup.item == null && findNewItem)
                        {
                            runsp = angeredspeed;
                            ani.speed = 1;
                            ChangeAnimationState("SKmove");
                            closestItem = pickup.GetItem();
                            if (closestItem != null)
                            {
                                transform.position = Vector2.MoveTowards(transform.position, closestItem.transform.position, runsp * Time.deltaTime);
                                if (Vector2.Distance(transform.position, closestItem.transform.position) <= pickup.pickUpRange)
                                {
                                    closestItem.GetComponent<PickUpBase>().PickUp(pickup.leftHand);
                                    pickup.item = closestItem;
                                    pickup.item.GetComponent<DamagesPlayer>().canHurt = false;
                                    Quaternion itemrot = Quaternion.LookRotation(transform.forward, transform.up);
                                    pickup.item.transform.rotation = itemrot;
                                    //skm.findNewItem = false;
                                }
                            }
                            else
                            {
                                runsp = angeredspeed;
                                ani.speed = 1;
                                RandomlyMove();
                            }
                        }
                        else
                        {
                            _currentState = skState.Wander;
                        }
                    break;
                }
                case skState.Chase:
                {
                        if (pickup.item == null) _currentState = skState.Arm;
                        if (target == null) _currentState = skState.Wander;
                        FaceTarget();
                        if (CheckLOS()) _currentState = skState.Attack;
                        else RandomlyMove();
                        break;
                }
                case skState.Attack:
                {
                        if (pickup.item == null) { _currentState = skState.Arm; break;}
                        if (target == null) { _currentState = skState.Wander; break; }
                        if (target == null)
                        {
                            _currentState = skState.Wander;
                        }
                        if (Vector2.Distance(transform.position, target.transform.position) < 2)
                        {
                            transform.position = Vector2.MoveTowards(transform.position, 2 * transform.position - target.transform.position, runsp * Time.deltaTime);
                        }
                        else if (Vector2.Distance(transform.position, target.transform.position) > 4)
                        {
                            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, runsp * Time.deltaTime);
                        }
                        if (pickup.item.TryGetComponent(out shotgun Gun))
                        {
                            Gun.Fire();
                        }
                        else if (pickup.item.TryGetComponent(out Boomerang Boom))
                        {
                            Boom.Fire();
                            StartCoroutine("WaitAfterThrow");
                            findNewItem = false;
                        }
                        else
                        {
                            pickup.ThrowItem();
                            StartCoroutine("WaitAfterThrow");
                            findNewItem = false;
                        }
                        if (!CheckLOS())
                        {
                            _currentState = skState.Chase;
                        }
                        break;
                }
            }
        }
    }

    /*
    private void FixedUpdate()
    {
        if (canMove)
        {
            if (pickup.item == null && findNewItem)
            {
                runsp = angeredspeed;
                ani.speed = 1;
                ChangeAnimationState("SKmove");
                closestItem = pickup.GetItem();
                if(closestItem != null)
                {
                    transform.position = Vector2.MoveTowards(transform.position, closestItem.transform.position, runsp * Time.deltaTime);
                    if (Vector2.Distance(transform.position, closestItem.transform.position) <= pickup.pickUpRange) 
                    {
                        closestItem.GetComponent<PickUpBase>().PickUp(pickup.leftHand);
                        pickup.item = closestItem;
                        pickup.item.GetComponent<DamagesPlayer>().canHurt = false;
                        Quaternion itemrot = Quaternion.LookRotation(transform.forward, -transform.right);
                        pickup.item.transform.rotation = itemrot;
                        //skm.findNewItem = false;
                    }
                }
                else
                {
                    runsp = angeredspeed;
                    ani.speed = 1;
                    RandomlyMove();
                }
            }
           
            else if (!angered || !findNewItem)
            {
                //ChangeAnimationState("SKIdle");
                if (patrolCount < 2)
                {
                    RandomlyMove();
                    runsp = patrolspeed;
                    ani.speed = 0.7f;
                    patrolCount += Time.deltaTime;
                    ChangeAnimationState("SKmove");
                }
                else if (patrolCount > 4)
                {
                    patrolCount = 0;
                }
                else
                {
                    patrolCount += Time.deltaTime;
                    ChangeAnimationState("SKIdle");
                }
                return;
            }
            else if (angered)
            {
                runsp = angeredspeed;
                ani.speed = 1;
                //GetTarget();
                //Debug.Log(target.name);
                if (target != null)
                {
                    FaceTarget();
                    if (!CheckLOS())
                    {
                        RandomlyMove();
                    }
                    else
                    {
                        if (Vector2.Distance(transform.position, target.transform.position) < 2)
                        {
                            transform.position = Vector2.MoveTowards(transform.position, 2 * transform.position - target.transform.position, runsp * Time.deltaTime);
                        }
                        else if (Vector2.Distance(transform.position, target.transform.position) > 4)
                        {
                            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, runsp * Time.deltaTime);
                        }
                        if (pickup.item.TryGetComponent(out shotgun Gun))
                        {
                            Gun.Fire();
                        }
                        else if (pickup.item.TryGetComponent(out Boomerang Boom))
                        {
                            Boom.Fire();
                            StartCoroutine("WaitAfterThrow");
                            findNewItem = false;
                        }
                        else
                        {
                            pickup.ThrowItem();
                            StartCoroutine("WaitAfterThrow");
                            findNewItem = false;
                        }
                    }
                }
                else
                {
                    angered = false;                    
                }
            }
        }     
    }
    */

    private bool CheckLOS()
    {
        bool canSeeTarget = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, target.transform.position - transform.position, viewRange);
        if(hit.collider.gameObject == target)
        {
            canSeeTarget = true;
        }
        return canSeeTarget;
    }

    public void GetNearestThing()
    {
        GameObject closestThing = livingthings[0];
        float closestDistance = Vector2.Distance(transform.position, livingthings[0].transform.position);
        if (closestThing == gameObject)
        {
            closestThing = livingthings[1];
            closestDistance = Vector2.Distance(transform.position, livingthings[1].transform.position);
        }
        for (int i = 1; i < livingthings.Count; i++)
        {
            if(livingthings[i] != null)
            {
                if (livingthings[i] == gameObject)
                {
                    print("poop");
                }
                else if (Vector2.Distance(transform.position, livingthings[i].transform.position) < closestDistance)
                {
                    closestDistance = Vector2.Distance(transform.position, livingthings[i].transform.position);
                    closestThing = livingthings[i];
                }
            }
        }
        target = closestThing;
    }

    private void FaceTarget()
    {
        if (target != null)
        {
            targetPos = target.transform.position;
            targetPos.x = targetPos.x - transform.position.x;
            targetPos.y = targetPos.y - transform.position.y;
            angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + -90));
        }
    }

    public void TargetDirection()
    {
        if (target != null)
        {
            //0 is topleft // 1 is topright // 2 is bottomright // 3 is bottomleft
            if (transform.position.x < target.transform.position.x)
            {
                //target to the right
                if (transform.position.y < target.transform.position.y)
                {
                    //top
                    targetDirection = 12;
                }
                else
                {

                    targetDirection = 23;
                }
            }
            else
            {
                if (transform.position.y < target.transform.position.y)
                {
                    targetDirection = 03;
                }
                else
                {
                    targetDirection = 01;
                }
            }
        }
    }

    private IEnumerator WaitAfterThrow()
    {
        yield return new WaitForSeconds(1);
        findNewItem = true;
    }

    private GameObject[] ClosestVisibleLiving()
    {
        GameObject closestThing = livingthings[0];
        GameObject closestThing2 = livingthings[0];
        GameObject closestThing3 = livingthings[0];
        float closestDistance = Vector2.Distance(transform.position, livingthings[0].transform.position);
        if (closestThing == gameObject)
        {
            closestThing = livingthings[1];
            closestThing2 = livingthings[1];
            closestThing3 = livingthings[1];
            closestDistance = Vector2.Distance(transform.position, livingthings[1].transform.position);
        }
        for (int i = 1; i < livingthings.Count; i++)
        {
            if (livingthings[i] != null)
            {
                if (livingthings[i] != gameObject && Vector2.Distance(transform.position, livingthings[i].transform.position) < closestDistance)
                {
                    closestDistance = Vector2.Distance(transform.position, livingthings[i].transform.position);
                    closestThing3 = closestThing2;
                    closestThing2 = closestThing;
                    closestThing = livingthings[i];
                }
            }
        }
        GameObject[] three = new GameObject[3];
        three[0] = closestThing;
        three[1] = closestThing2;
        three[2] = closestThing3; 
        return three;
    }

    private void GetTarget()
    {
        target = null;
        GameObject[] closestEns = ClosestVisibleLiving();
        for (int i = 0; i < closestEns.Length; i++)
        {
            float TargetDistance = 999;
            float WallDistance = 998;
            bool HitTarget = false;
            RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, closestEns[i].transform.position - transform.position, viewRange);
            for (int j = 0; j < hit.Length; j++)
            {
                //check if hit
                if (hit[j].collider.gameObject.name == closestEns[i].name)
                {
                    HitTarget = true;
                    TargetDistance = Vector2.Distance(transform.position, hit[j].point);
                }
                else if (hit[j].collider.gameObject.CompareTag("Wall"))
                {
                    if (WallDistance > Vector2.Distance(transform.position, hit[j].point))
                    {
                        WallDistance = Vector2.Distance(transform.position, hit[j].point);
                    }
                }
                else if (hit[j].collider.gameObject.CompareTag("Rock"))
                {
                    if (WallDistance > Vector2.Distance(transform.position, hit[j].point))
                    {
                        WallDistance = Vector2.Distance(transform.position, hit[j].point);
                    }
                }              
            }
            if (HitTarget)
            {
                if (TargetDistance < WallDistance)
                {
                    target = closestEns[i];
                    return;
                }
            }
        }
    }

    private void RandomlyMove()
    {
        RaycastHit2D[] hitLeft = Physics2D.RaycastAll(transform.position, Vector2.left, wallRange);
        walls[0] = false;
        if (hitLeft.Length > 0)
        {
            for (int f = 0; f < hitLeft.Length; f++)
            {
                if (hitLeft[f].collider.gameObject.CompareTag("Wall") || hitLeft[f].collider.gameObject.CompareTag("Rock") || hitLeft[f].collider.gameObject.CompareTag("Obsidian"))
                {
                    walls[0] = true;
                }
            }
        }
        RaycastHit2D[] hitUp = Physics2D.RaycastAll(transform.position, Vector2.up, wallRange);
        walls[1] = false;
        if (hitUp.Length > 0)
        {
            for (int p = 0; p < hitUp.Length; p++)
            {
                if (hitUp[p].collider.gameObject.CompareTag("Wall") || hitUp[p].collider.gameObject.CompareTag("Rock") || hitUp[p].collider.gameObject.CompareTag("Obsidian"))
                {
                    walls[1] = true;
                }
            }
        }
        RaycastHit2D[] hitRight = Physics2D.RaycastAll(transform.position, Vector2.right, wallRange);
        walls[2] = false;
        if (hitRight.Length > 0)
        {

            for (int b = 0; b < hitRight.Length; b++)
            {
                if (hitRight[b].collider.gameObject.CompareTag("Wall") || hitRight[b].collider.gameObject.CompareTag("Rock") || hitRight[b].collider.gameObject.CompareTag("Obsidian"))
                {
                    walls[2] = true;
                }
            }
        }
        RaycastHit2D[] hitDown = Physics2D.RaycastAll(transform.position, Vector2.down, wallRange);
        walls[3] = false;
        if (hitDown.Length > 0)
        {
            for (int m = 0; m < hitDown.Length; m++)
            {
                if (hitDown[m].collider.gameObject.CompareTag("Wall") || hitDown[m].collider.gameObject.CompareTag("Rock") || hitDown[m].collider.gameObject.CompareTag("Obsidian"))
                {
                    walls[3] = true;
                }
            }
        }
        //Vector2.MoveTowards(transform.position, tempdir, runsp);
        if (walls[0] && walls[1])
        {
            tempdir = new Vector2(Random.Range(0f, 1f), Random.Range(-1f, 0f));
            timer = 0;
        }
        else if (walls[1] && walls[2])
        {
            tempdir = new Vector2(Random.Range(-1f, 0f), Random.Range(-1f, 0f));
            timer = 0;
        }
        else if (walls[2] && walls[3])
        {
            tempdir = new Vector2(Random.Range(-1f, 0f), Random.Range(0f, 1f));
            timer = 0;
        }
        else if (walls[3] && walls[0])
        {
            tempdir = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));
            timer = 0;
        }
        else if (walls[0])
        {
            tempdir = new Vector2(1, Random.Range(-1f, 1f));
            timer = 0;
        }
        else if (walls[1])
        {
            tempdir = new Vector2(Random.Range(-1f, 1f), -1);
            timer = 0;
        }
        else if (walls[2])
        {
            tempdir = new Vector2(-1, Random.Range(-1f, 1f));
            timer = 0;
        }
        else if (walls[3])
        {
            tempdir = new Vector2(Random.Range(-1f, 1f), 1);
            timer = 0;
        }
        else if (tempdir.magnitude < 0.1) tempdir = new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f));
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, transform.position.y) + tempdir, runsp * Time.deltaTime);
        Quaternion rot = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(transform.forward, tempdir), 0.5f);
        transform.rotation = rot;
    }
}

public enum skState
{
    Wander,
    Chase,
    Attack,
    Arm
}