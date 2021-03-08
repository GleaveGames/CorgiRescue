using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    Vector2 move;
    public float runsp = 3.0f;
    public float turnSpeed;
    private Rigidbody2D rb;
    private GameObject child;
    private Animator ani;
    private string currentState;
    [SerializeField]
    private GameObject bomb;
    [SerializeField]
    private Transform pickPos;
    [SerializeField]
    private playerStats ps;
    public bool canMove = true;
    [SerializeField]
    private GameObject blood;
    public bool invinc = false;
    [SerializeField]
    private float invincTimePerFlash;
    [SerializeField]
    private SpriteRenderer[] playersprites;
    [SerializeField]
    private int numFlashes;
    Coroutine coroutine;
    public float miningSpeed;
    private float knockbackTime;
    private Vector3 knockbackForce;
    public bool canMine = true;
    public bool mining;
    [SerializeField]
    private PlayerControls pc;
    private AudioManager am;
    private CanPickUp cpu;
    public bool IceSlip;
    [SerializeField]
    [Range(0,1)]
    private float SlipFactor;
    private bool minePressed;
    private bool walkPressed;
    private float walkSpeed = 1.6f;

    private void Awake()
    {
        pc = new PlayerControls();
        pc.Game.Bomb.performed += _ => Bomb();
        pc.Game.Mine.started += _ => TriggerMine();
        pc.Game.Mine.canceled += _ => StopMine();
        pc.Game.Walk.started += _ => TriggerWalk();
        pc.Game.Walk.canceled += _ => StopWalk();
        pc.Game.Cheat.performed += _ => Cheat();
        pc.Game.Debug.performed += _ => Debug();
        am = FindObjectOfType<AudioManager>();
    }

    private void Start()
    {
        //Cursor.lockState = CursorLockMode.None;
        rb = GetComponent<Rigidbody2D>();
        child = transform.GetChild(0).gameObject;
        ani = child.GetComponent<Animator>();
        ps = FindObjectOfType<playerStats>();
        miningSpeed = ps.miningSpeed;
        ChangeAnimationState("Idle");
        cpu = GetComponentInParent<CanPickUp>();
        runsp = ps.moveSpeed;
    }

    private void TriggerMine()
    {
        minePressed = true;
    }
    private void StopMine()
    {
        minePressed = false;
    }
    private void TriggerWalk()
    {
        walkPressed = true;
    }
    private void StopWalk()
    {
        walkPressed = false;
    }


    void Update()
    {
        if(ps == null)
        {
            ps = FindObjectOfType<playerStats>();
        }
        if (canMove)
        {
            move = pc.Game.Move.ReadValue<Vector2>();
            if (move != Vector2.zero)
            {
                Vector3 newRot = child.transform.eulerAngles;

                if (move.y < 0)
                {
                    if (move.x < 0)
                    {
                        newRot.z = Mathf.Rad2Deg * Mathf.Atan(move.x / move.y) + 90;
                    }
                    else if (move.x > 0)
                    {
                        newRot.z = Mathf.Rad2Deg * Mathf.Atan(move.x / move.y) - 90;
                    }
                    else
                    {
                        newRot.z = Mathf.Rad2Deg * Mathf.Atan(move.x / move.y) - 180;
                    }

                }
                else
                {
                    newRot.z = Mathf.Rad2Deg * Mathf.Atan(-move.x / move.y);

                }
                Quaternion quaternion = Quaternion.Euler(newRot.x, newRot.y, newRot.z);
                Quaternion rot = Quaternion.Lerp(transform.rotation, quaternion, turnSpeed);
                //child.transform.eulerAngles = newRot;
                transform.rotation = rot;
            }
            if (!mining)
            {
                if (rb.velocity.magnitude > 0)
                {
                    ChangeAnimationState("PlayerMove");
                    ani.SetFloat("MoveSpeed", rb.velocity.magnitude / runsp);
                }
                else
                {
                    ChangeAnimationState("Idle");
                }
            }
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(transform.forward, -rb.velocity), turnSpeed) ;
        }

        if (ps.health <= 0)
        {
            canMove = false;
            ChangeAnimationState("Death");
            FindObjectOfType<Menu>().Fade();
            //GetComponent<CircleCollider2D>().enabled = false;
        }

    }
    private void FixedUpdate()
    {
        if (canMove)
        {
            if (!IceSlip)
            {
                rb.velocity = new Vector2(0, 0);
                if (!walkPressed && !mining)
                {
                    rb.velocity = new Vector2(move.x * ps.moveSpeed, move.y * ps.moveSpeed);
                }
                else
                {
                    rb.velocity = new Vector2(move.x * walkSpeed, move.y * walkSpeed);
                }
            }
            else
            {
                rb.velocity += SlipFactor * new Vector2(move.x * ps.moveSpeed, move.y * ps.moveSpeed);
                if (rb.velocity.magnitude >= new Vector2(0.7f * ps.moveSpeed, 0.7f * ps.moveSpeed).magnitude) 
                {
                    rb.velocity = rb.velocity.normalized * new Vector2(0.7f * ps.moveSpeed, 0.7f * ps.moveSpeed).magnitude;
                }
                move.x = 0;
                move.y = 0;
            }
        }
        if (minePressed)
        {
            Mine();
        }
    }


    private IEnumerator InvincibilityFrames()
    {
        for (int i = 0; i < numFlashes; i++)
        {
            Color tmp = playersprites[0].color;

            tmp.a = 0;
            for (int j = 0; j < playersprites.Length; j++)
            {
                playersprites[j].color = tmp;
            }
            yield return new WaitForSeconds(invincTimePerFlash / 2);

            tmp.a = 255;
            for (int j = 0; j < playersprites.Length; j++)
            {
                playersprites[j].color = tmp;
            }
            yield return new WaitForSeconds(invincTimePerFlash / 2);
        }
        invinc = false;
    }

    public void StartInvinc()
    {
        StartCoroutine("InvincibilityFrames");
    }

    public void KnockBack(float time, Vector3 force)
    {
        knockbackForce = force;
        knockbackTime = time;
        StartCoroutine("StartKnockBack");
    }

    public IEnumerator StartKnockBack()
    {
        rb.AddForce(knockbackForce, ForceMode2D.Impulse);
        canMove = false;
        yield return new WaitForSeconds(knockbackTime);
        canMove = true;
    }

    private IEnumerator MiningCool()
    {
        yield return new WaitForSeconds(miningSpeed);
        canMine = true;
    }

    public void MineCool()
    {
        StartCoroutine("MiningCool");
        canMine = true;
    }

    public void Cheat()
    {
        ps.health += 1;
    }
    public void Debug()
    {
        if (GetComponent<DebugStuff.Debugger>().enabled)
        {
            GetComponent<DebugStuff.Debugger>().enabled = false;
        }
        else
        {
            GetComponent<DebugStuff.Debugger>().enabled = true;
        }

    }

    private void OnEnable()
    {
        pc.Enable();
    }
    private void OnDisable()
    {
        pc.Disable();
    }

    private void Bomb()
    {
        if (canMove)
        {
            if (ps.bombs > 0)
            {
                GameObject b;
                Vector3 bombSpawn = transform.position + 0.6f * transform.GetChild(0).up;
                if (Physics2D.OverlapCircle(bombSpawn, 0.3f) == null)
                {
                    b = Instantiate(bomb, bombSpawn, Quaternion.identity);
                }
                else
                {
                    b = Instantiate(bomb, cpu.leftHand.transform.position, Quaternion.identity);
                }
                ps.bombs -= 1;
                if (ps.bigbombs)
                {
                    b.transform.localScale = b.transform.localScale * 2;
                    b.GetComponent<HoldingThisSlowsPlayer>().percentageSlowed = 70;
                }
            }
        }        
    }

    private void Mine()
    {
        if (canMine)
        {
            am.Play("Swing", transform.position);
            ChangeAnimationState("Mine");
            canMine = false;
            mining = true;
            StartCoroutine("MiningCool");
        }        
    }



    public void ChangeAnimationState(string newState)
    {
        if (currentState == "Death") return;
        if (currentState == newState) return;

        ani.Play(newState);

        currentState = newState;
    }
}

   
