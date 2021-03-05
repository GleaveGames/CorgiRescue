using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
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
            if (hit.collider.tag == "Player")
            {
                triggered = true;
            }
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
            else
            {
                if (timer > rotateTime)
                {
                    timer = 0;
                    quaternion = Quaternion.Euler(new Vector3(0, 0, Random.Range(1, 5) * 90 + angle + offset));
                }
                else
                {
                    rot = Quaternion.Lerp(transform.rotation, quaternion, 0.02f);
                    //child.transform.eulerAngles = newRot;
                    transform.rotation = rot;
                    timer += Time.deltaTime;
                }
            }
        }
    }
    void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;

        ani.Play(newState);

        currentState = newState;
    }

    private IEnumerator Flap()
    {
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(3);
        flapPlayed = false;
    }
}
