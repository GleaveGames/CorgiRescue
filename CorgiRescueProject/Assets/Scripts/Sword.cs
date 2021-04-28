using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    /*
     Sword Breakdown
     
    if charge weapon
        attack -> hold ani
        release -> attack ani
    else  !charge weapon
        attack -> attack ani


    GOT TO TURN ON COLLIDER DURING SWING IN SCRIPT BECAUSE OTHERWISE ANIMATOR RUINS COLLIDER SCRIPTS

    ALSO HAVE TO BE TRIGGER DAMAGE

    */

    Coroutine coroutine;
    [SerializeField]
    private bool charge = true;
    private cameraoptions co;
    private Animator ani;
    private string currentState;
    private PlayerControls pc;
    Collider2D col;

    private void Awake()
    {
        pc = new PlayerControls();
        co = FindObjectOfType<cameraoptions>();
        ani = GetComponent<Animator>();
        pc.Game.Fire.canceled += _ => Release();
        col = GetComponent<Collider2D>();
    }

    public void Fire()
    {
        if (charge)
        {
            if (ani.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                ani.Play("Hold");
                co.shakeDuration = 0.01f;
            }
        }
        else 
        {
            if (ani.GetCurrentAnimatorStateInfo(0).IsName("Idle")) 
            {
                ani.Play("Swing");
                co.shakeDuration = 0.01f;
            }
        }
    }

    private void Release()
    {
        if (charge) 
        {
            if (transform.parent != null)
            {
                ani.Play("Swing");
                GetComponent<AudioSource>().Play();
            }
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


    //change animation state doesn't really work if you use transitions
    public void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        ani.Play(newState);
        currentState = newState;
    }


    public void ColliderOn() 
    {
        col.enabled = true;
        col.isTrigger = true;
    }

    public void ColliderOff() 
    {
        col.enabled = false;
        col.isTrigger = false;
    }

    



    /*
     * OLD SWORD
    Coroutine coroutine;
    [SerializeField]
    private bool loaded = true;
    [SerializeField]
    private float reloadTime;
    private cameraoptions co;
    private Animator ani;
    private string currentState;
    private PlayerControls pc;
    [SerializeField]
    private bool chargeWeapon;


    private void Awake()
    {
        pc = new PlayerControls();
        co = FindObjectOfType<cameraoptions>();
        pc.Game.Fire.canceled += _ => Release();
        ani = GetComponent<Animator>();
    }

    private void Update()
    {
        if (transform.parent == null)
        {
            ChangeAnimationState("ColliderOn");
            GetComponent<Collider2D>().isTrigger = false;
        }
    }

    public void Attack()
    {
        if (!chargeWeapon)
        {
            if (loaded)
            {
                co.shakeDuration = 0.05f;
                loaded = false;
                StartCoroutine("Reload");
                GetComponent<AudioSource>().Play();
                //GetComponent<Rigidbody2D>().isKinematic = false;
                ChangeAnimationState("Swing");
            }
        }
    }

    public IEnumerator Reload()
    {
        yield return new WaitForSeconds(reloadTime);
        ChangeAnimationState("Idle");
        loaded = true;
    }

    public void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        ani.Play(newState);
        currentState = newState;
    }

    private void Release()
    {
        
    }
    private void OnEnable()
    {
        pc.Enable();
    }
    private void OnDisable()
    {
        pc.Disable();
    }

    */

}


