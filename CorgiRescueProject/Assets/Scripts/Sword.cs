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


    */

    Coroutine coroutine;
    [SerializeField]
    private bool charge = true;
    private cameraoptions co;
    private Animator ani;
    private string currentState;
    private PlayerControls pc;

    private void Awake()
    {
        pc = new PlayerControls();
        co = FindObjectOfType<cameraoptions>();
        ani = GetComponent<Animator>();
        pc.Game.Fire.canceled += _ => Release();
    }

    public void Fire()
    {
        if (charge)
        {
            ChangeAnimationState("Hold");
            co.shakeDuration = 0.01f;
        }
        else 
        {
            ChangeAnimationState("Swing");
            co.shakeDuration = 0.01f;
        }
    }

    private void Release()
    {
        if (transform.parent != null)
        {
            ChangeAnimationState("Swing");
            //this is dumb should have just a script that saves these so don't have to check each time but idk means shotgun can be swapped between holders easily
            GetComponent<AudioSource>().Play();
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

    public void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        ani.Play(newState);
        currentState = newState;
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


