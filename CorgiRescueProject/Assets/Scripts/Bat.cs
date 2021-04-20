using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
    [SerializeField]
    private float swoopDist;
    private bool swooping;
    private bool swoopend;

    public LayerMask IgnoreMe;
    public bool inSwarm;
    private GameObject player;
    public bool triggered;
    [SerializeField]
    private float speed;
    private Animator ani;
    private string currentState;
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

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        ani = GetComponent<Animator>();
        ani.speed = Random.Range(0.9f, 1.1f);
        quaternion = Quaternion.Euler(new Vector3(0, 0, Random.Range(1, 5) * 90 + angle + offset));
    }

    void Update()
    {
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
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player");
            }
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up);
            if (hit.collider.CompareTag("Player"))
            {
                triggered = true;
            }

            if (triggered) 
            {
                if (Vector2.Distance(hit.point, transform.position) > 0.2)
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
                        if(Vector2.Distance(transform.position, swooppos2) < 0.2f) 
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
            /*
            if (triggered)
            {
                if (Vector2.Distance(hit.point, transform.position) > 0.2)   //might need to give raycast all a range
                {
                    if (!flapPlayed)
                    {
                        StartCoroutine("Flap");
                        flapPlayed = true;
                    }
                    targetPos = player.transform.position;
                    targetPos.x = targetPos.x - transform.position.x;
                    targetPos.y = targetPos.y - transform.position.y;
                    angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
                    Quaternion quaternion = Quaternion.Euler(new Vector3(0, 0, angle + offset));
                    rot = Quaternion.Lerp(transform.rotation, quaternion, 0.1f);
                    //child.transform.eulerAngles = newRot;
                    transform.rotation = rot;
                    transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            




                    //anim stuff
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
            */
            else
            {
                transform.rotation = Quaternion.identity;
                RaycastHit2D closestHitR = ClosestRaycast(Vector2.right);
                if (closestHitR.collider.gameObject.CompareTag("Player"))
                {
                    triggered = true;
                }
                //
                RaycastHit2D closestHitL = ClosestRaycast(-Vector2.right);
                if (closestHitL.collider.gameObject.CompareTag("Player"))
                {
                    triggered = true;
                }
                //
                RaycastHit2D closestHitU = ClosestRaycast(Vector2.up);
                if (closestHitU.collider.gameObject.CompareTag("Player"))
                {
                    triggered = true;
                }
                //
                RaycastHit2D closestHitD = ClosestRaycast(-Vector2.up);
                if (closestHitD.collider.gameObject.CompareTag("Player"))
                {
                    triggered = true;
                }
                /*
                if (timer > rotateTime)
                {
                    timer = 0;
                    quaternion = Quaternion.Euler(new Vector3(0, 0, Random.Range(1, 5) * 90 + angle + offset));
                }
                else
                {
                    rot = Quaternion.Lerp(transform.rotation, quaternion, Time.deltaTime);
                    transform.rotation = rot;
                    timer += Time.deltaTime;
                }
                */
            }
        }
    }
    void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;

        ani.Play(newState);

        currentState = newState;
    }

    private IEnumerator Swoop()
    {
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
        GetComponent<AudioSource>().Play();
        if (coreBat)
        {
            ChangeAnimationState("CoreBatFly");
        }
        else
        {
            ChangeAnimationState("BatFly");
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
        Debug.Log("Finishes swoop");
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
}
