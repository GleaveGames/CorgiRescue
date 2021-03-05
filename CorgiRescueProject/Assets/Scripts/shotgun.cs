using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shotgun : MonoBehaviour
{
    [SerializeField]
    private bool sling = false;
    [SerializeField]
    private GameObject bullet;
    //public bool inHand = false;
    [SerializeField]
    private float shotpower;
    [SerializeField]
    private int numBullets;
    Coroutine coroutine;
    [SerializeField]
    private bool loaded = true;
    [SerializeField]
    private float reloadTime;
    private cameraoptions co;
    [SerializeField]
    private Transform bulletspawn;
    private Animator ani;
    private string currentState;
    private PlayerControls pc;

    private void Awake()
    {
        pc = new PlayerControls();
        co = FindObjectOfType<cameraoptions>();
        if (sling)
        {
            ani = GetComponent<Animator>();
        }
        pc.Game.Fire.canceled += _ => Release();
    }

    public void Fire()
    {
        if (!sling)
        {
            if (loaded)
            {
                co.shakeDuration = 0.05f;
                for (int i = 0; i < numBullets; i++)
                {
                    //shoot from end of shotogun

                    GameObject b = Instantiate(bullet, bulletspawn.position, Quaternion.identity);
                    Vector3 spread = transform.right;//transform.parent.parent.up;
                    spread.x += Random.Range(-0.15f, 0.15f);
                    spread.y += Random.Range(-0.15f, 0.15f);
                    b.GetComponent<Rigidbody2D>().AddForce(spread * shotpower, ForceMode2D.Impulse);
                    if (i == 0)
                    {
                        if (transform.root.gameObject.TryGetComponent(out playerMovement pm))
                        {
                            pm.KnockBack(0.3f, -5 * spread * shotpower);
                        }
                        //this is dumb should have just a script that saves these so don't have to check each time but idk means shotgun can be swapped between holders easily
                        else if (transform.parent.parent.parent.gameObject.TryGetComponent(out SKMovement skmov))
                        {
                            skmov.KnockBack(0.3f, -5 * spread * shotpower);
                        }

                    }
                }
                loaded = false;
                StartCoroutine("Reload");
                GetComponent<AudioSource>().Play();
            }
        }
        else if(sling)
        {
            if (loaded)
            {
                ChangeAnimationState("SlingPull");
                co.shakeDuration = 0.01f;
                //shoot from end of shotogun
            }
        }
    }

    public IEnumerator Reload()
    {
        yield return new WaitForSeconds(reloadTime);
        loaded = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (sling)
        {
            if (collision.gameObject.name.Contains("Pebble"))
            {
                if(!loaded)
                {
                    Destroy(collision.gameObject);
                    loaded = true;
                    ChangeAnimationState("SlingLoaded");
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

    private void Release()
    {
        if(transform.parent != null)
        {
            if(sling)
        {
                if (loaded)
                {
                    ChangeAnimationState("SlingFire");
                    GameObject b = Instantiate(bullet, bulletspawn.position, Quaternion.identity);
                    Vector3 spread = transform.right;//transform.parent.parent.up;
                    spread.x += Random.Range(-0.15f, 0.15f);
                    spread.y += Random.Range(-0.15f, 0.15f);
                    b.GetComponent<Rigidbody2D>().AddForce(spread * shotpower, ForceMode2D.Impulse);

                    if (transform.root.gameObject.TryGetComponent(out playerMovement pm))
                    {
                        pm.KnockBack(0.3f, -spread * shotpower);
                    }
                    //this is dumb should have just a script that saves these so don't have to check each time but idk means shotgun can be swapped between holders easily
                    else if (transform.parent.parent.parent.gameObject.TryGetComponent(out SKMovement skmov))
                    {
                        skmov.KnockBack(0.3f, -spread * shotpower);
                    }
                    loaded = false;
                    GetComponent<AudioSource>().Play();
                }
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
}
