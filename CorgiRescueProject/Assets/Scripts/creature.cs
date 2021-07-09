using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class creature : Living
{
    private Vector3 targetPos;
    private float angle;
    public float offset;
    [SerializeField]
    private AudioSource stompSound;
    [SerializeField]
    private AudioSource roarSound;
    private cameraoptions co;
    [SerializeField]
    private bool boss;
    public GameObject IceSpikeSpawnBall;
    public GameObject BigSnowBall;
    public float IceAttackSpeed;
    public float BallAttackSpeed;
    private bool doingAction;
    public float actionWaitTime = 5f;
    public float AimModifier;
    private bool charging;
    private bool punching;

    // Start is called before the first frame update
    protected override void Start()
    {
        co = FindObjectOfType<cameraoptions>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //rb = GetComponent<Rigidbody2D>();
        Roar();
        ani = GetComponent<Animator>();
        StartCoroutine("WaitAndIdle");
        doingAction = true;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!boss)
        {
            ChangeAnimationState("YetiWalk");
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            targetPos = player.position;
            targetPos.x = targetPos.x - transform.position.x;
            targetPos.y = targetPos.y - transform.position.y;
            angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + offset));
        }
        else
        {
            //Boss Stuff
            LookAtPlayer();
            if (!doingAction)
            {
                //GetAction
                int NextAction = Random.Range(1, 5);
                //int NextAction = 4;
                //print(NextAction);
                if (NextAction == 1) StartCoroutine("IceAttack"); 
                else if (NextAction == 2) StartCoroutine("SnowBallAttack");
                else if (NextAction == 3) StartCoroutine("IceAttack2");
                else if (NextAction == 4) StartCoroutine("ChargeAttack");
            }
            else if (charging)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.position, 1.3f * speed * Time.deltaTime);
                targetPos = player.position;
                targetPos.x = targetPos.x - transform.position.x;
                targetPos.y = targetPos.y - transform.position.y;
                angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + offset));
            }
            else if (punching)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.position, 3f * speed * Time.deltaTime);
                targetPos = player.position;
                targetPos.x = targetPos.x - transform.position.x;
                targetPos.y = targetPos.y - transform.position.y;
                angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + offset));
            }
        }
    }

    private IEnumerator ChargeAttack()
    {
        doingAction = true;
        Debug.Log("ChargeAttack");
        ChangeAnimationState("YetiWalk");
        AimModifier = 0f;
        charging = true;
        yield return new WaitForSeconds(2.5f);
        //Punch Animation
        charging = false;
        ChangeAnimationState("YetiPunch");
        yield return new WaitForSeconds(0.7f);
        punching = true;
        yield return new WaitForSeconds(0.6f);
        punching = false;
        StartCoroutine("WaitAndIdle");
    }

    private IEnumerator IceAttack()
    {
        doingAction = true;
        ChangeAnimationState("YetiIceAttack1");
        //time to do animation
        yield return new WaitForSeconds(1f);
        AimModifier = 0.4f;
        print("IceAttack");
        Vector3 IceBallSpawn = transform.position + targetPos.normalized * 2f;
        GameObject ball = Instantiate(IceSpikeSpawnBall, IceBallSpawn, Quaternion.identity);
        ball.GetComponent<Rigidbody2D>().AddForce(targetPos * IceAttackSpeed);
        AimModifier = 0f;
        StartCoroutine("WaitAndIdle");
    
    }
    private IEnumerator IceAttack2()
    {
        doingAction = true;
        ChangeAnimationState("YetiIceAttack2");
        yield return new WaitForSeconds(1f);
        AimModifier = 0.4f;
        print("IceAttack2");
        Vector3 IceBallSpawn = transform.position + targetPos.normalized * 2f;
        GameObject ball2 = Instantiate(IceSpikeSpawnBall, IceBallSpawn, Quaternion.identity);
        ball2.GetComponent<Rigidbody2D>().AddForce((targetPos + 6 * new Vector3(targetPos.y, targetPos.x).normalized)* IceAttackSpeed);
        GameObject ball3 = Instantiate(IceSpikeSpawnBall, IceBallSpawn, Quaternion.identity);
        ball3.GetComponent<Rigidbody2D>().AddForce((targetPos - 6 * new Vector3(targetPos.y, targetPos.x).normalized)* IceAttackSpeed);
        AimModifier = 0f;
        StartCoroutine("WaitAndIdle");
    }
    private IEnumerator SnowBallAttack()
    {
        doingAction = true;
        ChangeAnimationState("YetiSnowball");
        AimModifier = 0.5f;
        print("SnowballAttack");
        Vector3 IceBallSpawn = transform.position + targetPos.normalized * 1.4f;
        GameObject ball = Instantiate(BigSnowBall, IceBallSpawn, transform.rotation);
        ball.GetComponent<Rigidbody2D>().AddForce(targetPos * BallAttackSpeed);
        yield return new WaitForSeconds(0.5f);
        AimModifier = 0.2f;
        IceBallSpawn = transform.position + targetPos.normalized * 1.4f;
        GameObject ball2 = Instantiate(BigSnowBall, IceBallSpawn, transform.rotation);
        ball2.GetComponent<Rigidbody2D>().AddForce(targetPos * BallAttackSpeed);
        yield return new WaitForSeconds(0.5f);
        AimModifier = 0.4f;
        IceBallSpawn = transform.position + targetPos.normalized * 1.4f;
        GameObject ball3 = Instantiate(BigSnowBall, IceBallSpawn, transform.rotation);
        ball3.GetComponent<Rigidbody2D>().AddForce(targetPos * BallAttackSpeed);
        StartCoroutine("WaitAndIdle");
    }

    private IEnumerator WaitAndIdle()
    {
        print("wait and idle");
        //maybe add in a thing so no rotation idk 
        AimModifier = 0f;
        charging = false;
        punching = false;
        ChangeAnimationState("YetiIdle");
        yield return new WaitForSeconds(actionWaitTime);
        doingAction = false;
    }

    public void StompLeft()
    {
        stompSound.Play();
        co.shakeDuration += 0.1f;
    }
    public void StompRight()
    {
        stompSound.Play();
        co.shakeDuration += 0.1f;
    }
    public void Roar()
    {
        roarSound.pitch = 1 + (Random.Range(-0.3f, 0.3f));
        roarSound.Play();
        StartCoroutine("resetRoar");
        co.shakeDuration = 1;
        speed += 0.4f;
    }

    private IEnumerator resetRoar()
    {
        yield return new WaitForSeconds(Random.Range(10, 13));
        Roar();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!boss)
        {
            if (collision.collider.gameObject.CompareTag("Living"))
            {
                if (collision.collider.gameObject.TryGetComponent(out CharacterStats cs))
                {
                    cs.health -= 999;
                }
            }

            if (collision.collider.gameObject.CompareTag("Trap"))
            {
                Destroy(collision.collider.gameObject);
            }
        }
    }

    private void LookAtPlayer()
    {
        /*
        //Look At Player
        targetPos = player.position;
        targetPos.x = targetPos.x - transform.position.x;
        targetPos.y = targetPos.y - transform.position.y;
        angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + offset));
        */

        //NEW TARGET WITH VELOCITY AIMING
        targetPos = player.position + AimModifier * new Vector3(player.GetComponent<Rigidbody2D>().velocity.x, player.GetComponent<Rigidbody2D>().velocity.y, 0f);
        targetPos.x = targetPos.x - transform.position.x;
        targetPos.y = targetPos.y - transform.position.y;
        angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + offset));
    }
}
