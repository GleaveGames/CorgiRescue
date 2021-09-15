using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SKMovement : Living
{
    private skState _currentState;
    [SerializeField]
    float wallDelay;   
    [SerializeField]
    float getWeightsDelay;
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
    Vector2 lastSpotted;
    Vector2 movePoint;
    private bool canGetWeights = true;
    float[] weights;
    int[] weightsOrder;
    float weaponRange;


    protected override void Start()
    {
        //this needs to be changed at some point
        weightsOrder = new int[4] { 0, 1, 2, 3 };
        
        child = transform.GetChild(0).gameObject;
        ani = child.GetComponent<Animator>();
        ChangeAnimationState("SKIdle");
        pickup = GetComponent<SKPickUp>();
        pickup.viewRange = viewRange;
        if (FindObjectOfType<playerStats>().wanted)
        {
            angered = true;
        }
        rb = GetComponent<Rigidbody2D>();
        if (!pickupable) Destroy(GetComponent<PickUpEnemy>());
        lg = FindObjectOfType<LevelGenerator>();
        am = FindObjectOfType<AudioManager>();
        StartCoroutine(GetPlayer());
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
                            runsp = angeredspeed;
                            ani.speed = 1;
                            ChangeAnimationState("SKmove");
                            return;
                        }
                        if (angered && target != null) 
                        {
                            _currentState = skState.Chase;
                            runsp = angeredspeed;
                            ani.speed = 1;
                            ChangeAnimationState("SKmove");
                            return;
                        };

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
                        if (pickup.item == null && findNewItem)
                        {
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
                                    weaponRange = pickup.item.GetComponent<PickUpBase>().attackRange;
                                    //skm.findNewItem = false;
                                }
                            }
                            else
                            {
                                Debug.Log("No closest Item");
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
                        if (pickup.item == null) { _currentState = skState.Arm; break; }
                        if (target == null) { _currentState = skState.Wander; break; }
                        FaceTarget();
                        if (CheckLOS()) { _currentState = skState.Attack; break; }
                        else if (!CanSeeLastPosition()) _currentState = skState.Search;
                        else if (Vector2.Distance(transform.position, lastSpotted) > 0.2f)
                        {
                            transform.position = Vector2.MoveTowards(transform.position, lastSpotted, runsp * Time.deltaTime);
                            FaceTarget();
                        }
                        else _currentState = skState.Search;
                        break;
                    }
                case skState.Search:
                    {
                        if (pickup.item == null) _currentState = skState.Arm;
                        if (target == null) _currentState = skState.Wander;
                        if (CheckLOS()) _currentState = skState.Attack;
                        else
                        {
                            Search();
                        }
                        break;
                    }
                case skState.Attack:
                    {
                        FaceTarget();
                        if (pickup.item == null) { _currentState = skState.Arm; break; }
                        if (target == null) { _currentState = skState.Wander; break; }
                        if (target == null)
                        {
                            _currentState = skState.Wander;
                        }
                        if (Vector2.Distance(transform.position, target.transform.position) < 2)
                        {
                            transform.position = Vector2.MoveTowards(transform.position, 2 * transform.position - target.transform.position, runsp * Time.deltaTime);
                        }
                        else if (Vector2.Distance(transform.position, target.transform.position) > weaponRange - 0.5f)
                        {
                            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, runsp * Time.deltaTime);
                        }
                        if(Vector2.Distance(transform.position, target.transform.position) <= weaponRange)
                        {
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
                        if (!CheckLOS())
                        {
                            _currentState = skState.Chase;
                        }
                        lastSpotted = target.transform.position;
                        break;
                    }
            }
        }
    }


    private bool CheckLOS()
    {
        bool canSeeTarget = false;
        RaycastHit2D hit = Physics2D.Raycast(pickup.leftHand.position, target.transform.position - transform.position, viewRange);
        if (hit.collider.gameObject == target)
        {
            canSeeTarget = true;
        }
        return canSeeTarget;
    }

    private bool CanSeeLastPosition()
    {
        bool canSeeLastPos = false;
        int layerMask = LayerMask.GetMask("Tiles");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, lastSpotted - new Vector2(transform.position.x, transform.position.y), viewRange, layerMask);
        if (!hit)
        {
            canSeeLastPos = true;
        }
        return canSeeLastPos;
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
            if (livingthings[i] != null)
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

    private void GetWalls()
    {
        if (!walls[0])
        {
            RaycastHit2D[] hitLeft = Physics2D.RaycastAll(transform.position, Vector2.left, wallRange);
            if (hitLeft.Length > 0)
            {
                for (int f = 0; f < hitLeft.Length; f++)
                {
                    if (hitLeft[f].collider.gameObject.CompareTag("Wall") || hitLeft[f].collider.gameObject.CompareTag("Rock") || hitLeft[f].collider.gameObject.CompareTag("Obsidian"))
                    {
                        StartCoroutine(WallDelay(0));
                    }
                }
            }
        }
        if (!walls[1]) { 
            RaycastHit2D[] hitUp = Physics2D.RaycastAll(transform.position, Vector2.up, wallRange);
            if (hitUp.Length > 0)
            {
                for (int p = 0; p < hitUp.Length; p++)
                {
                    if (hitUp[p].collider.gameObject.CompareTag("Wall") || hitUp[p].collider.gameObject.CompareTag("Rock") || hitUp[p].collider.gameObject.CompareTag("Obsidian"))
                    {
                        StartCoroutine(WallDelay(1));
                    }
                }
            }
        }
        if (!walls[2])
        {
            RaycastHit2D[] hitRight = Physics2D.RaycastAll(transform.position, Vector2.right, wallRange);
            if (hitRight.Length > 0)
            {

                for (int b = 0; b < hitRight.Length; b++)
                {
                    if (hitRight[b].collider.gameObject.CompareTag("Wall") || hitRight[b].collider.gameObject.CompareTag("Rock") || hitRight[b].collider.gameObject.CompareTag("Obsidian"))
                    {
                        StartCoroutine(WallDelay(2));
                    }
                }
            }
        }
        if (!walls[3])
        {
            RaycastHit2D[] hitDown = Physics2D.RaycastAll(transform.position, Vector2.down, wallRange);
            if (hitDown.Length > 0)
            {
                for (int m = 0; m < hitDown.Length; m++)
                {
                    if (hitDown[m].collider.gameObject.CompareTag("Wall") || hitDown[m].collider.gameObject.CompareTag("Rock") || hitDown[m].collider.gameObject.CompareTag("Obsidian"))
                    {
                        StartCoroutine(WallDelay(3));
                    }
                }
            }
        }
    }

    private IEnumerator WallDelay(int wall)
    {
        walls[wall] = true;
        //custom wall delay for most desired direction;
        if (wall != weightsOrder[0]) yield return new WaitForSeconds(wallDelay);
        else yield return new WaitForSeconds(0.1f);
        walls[wall] = false;
    }

    private void RandomlyMove()
    {
        GetWalls();
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

    public float[] CardinalPlayerDirections()
    {
        /*
        Vector2 dir = Vector2.up;
        float currentMax = -Mathf.Infinity;
        Vector2 vecToPlayer = player.position - transform.position;
        if (Vector2.Dot(vecToPlayer, Vector2.up) > currentMax) { dir = Vector2.up; currentMax = Vector2.Dot(vecToPlayer, Vector2.up); } 
        if(Vector2.Dot(vecToPlayer, Vector2.right) > currentMax) { dir = Vector2.right; currentMax = Vector2.Dot(vecToPlayer, Vector2.right); }
        if (Vector2.Dot(vecToPlayer, Vector2.down) > currentMax) { dir = Vector2.down; currentMax = Vector2.Dot(vecToPlayer, Vector2.down); }
        if (Vector2.Dot(vecToPlayer, Vector2.left) > currentMax) { dir = Vector2.left; currentMax = Vector2.Dot(vecToPlayer, Vector2.left); }
        return dir;
    */
        Vector2 vecToPlayer = player.position - transform.position;
        float[] weights = new float[4];
        weights[0] = Vector2.Dot(vecToPlayer, Vector2.left);
        weights[1] = Vector2.Dot(vecToPlayer, Vector2.up);
        weights[2] = Vector2.Dot(vecToPlayer, Vector2.right);
        weights[3] = Vector2.Dot(vecToPlayer, Vector2.down);
        return weights;
    }

    private void Search()
    {
        //head towards the player
        if (canGetWeights)
        {
            StartCoroutine(GetWeights());
        }
        GetWalls();
        for (int i = 0; i < 4; i++) {
            if (!walls[weightsOrder[i]])
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, transform.position.y) + GetDirFromInt(weightsOrder[i]), runsp * Time.deltaTime);
                Quaternion rot = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(transform.forward, GetDirFromInt(weightsOrder[i])), 0.2f);
                transform.rotation = rot;
                break;
            }
        }
        //on raycast hit with wall, travel along the wall until you can turn towards the player.
    }

    private IEnumerator GetWeights()
    {
        canGetWeights = false;
        weights = CardinalPlayerDirections();
        weightsOrder = new int[4] { 0, 1, 2, 3 };
        for (int repeat = 0; repeat < weights.Length; repeat++)
        {
            for (int i = weights.Length - 1; i > 0; i--)
            {
                if (weights[i] > weights[i - 1])
                {
                    float temp = weights[i - 1];
                    weights[i - 1] = weights[i];
                    weights[i] = temp;
                    int tempint = weightsOrder[i - 1];
                    weightsOrder[i - 1] = weightsOrder[i];
                    weightsOrder[i] = tempint;
                }
            }
        }
        yield return new WaitForSeconds(getWeightsDelay);
        canGetWeights = true;
    }
    private Vector2 GetDirFromInt(int compassNum)
    {
        Vector2 dir = Vector2.left;
        if (compassNum == 1) dir = Vector2.up;
        else if (compassNum == 2) dir = Vector2.right;
        else if (compassNum == 3) dir = Vector2.down;

        return dir;
    }
}



public enum skState
{
    Wander,
    Chase,
    Search,
    Attack,
    Arm
}