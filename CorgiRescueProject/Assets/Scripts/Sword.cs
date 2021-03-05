using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
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
            ChangeAnimationState("AndurilColliderOn");
            GetComponent<Collider2D>().isTrigger = false;
        }
    }

    public void Fire()
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
                ChangeAnimationState("AndurilSwing");
            }
        }
    }

    public IEnumerator Reload()
    {
        yield return new WaitForSeconds(reloadTime);
        ChangeAnimationState("AndurilIdle");
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

}


