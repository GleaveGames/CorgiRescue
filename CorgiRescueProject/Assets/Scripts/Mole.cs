using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole : Living
{
    private Vector3 dest;
    private float angeredSpeed;
    private float angle;
    [SerializeField]
    private float offset;
    private bool called;
    private bool digSoundDone = true;
    [SerializeField]
    private bool boss;
    private ParticleSystem ZZZ;
    private bool charged = false;
    private bool charging = false;
    private Vector3 targetPos;
    private AudioManager am;


    protected override void Start()
    {
        base.Start();
        if (boss)
        {
            ZZZ = transform.GetChild(0).GetComponent<ParticleSystem>();
            am = FindObjectOfType<AudioManager>();
            am.Play("Snore", transform.position, true);
        }
        else
        {
            StartCoroutine("GetNewDest");
        }
        angeredSpeed = 2 * speed;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (boss)
        {
            if (!angered)
            {
                ChangeAnimationState("BigMoleIdle");
                ZZZ.Play();
                rb.isKinematic = true;
            }
            else
            {
                //prime and look at player
                //charge forward
                //repeat..
                if (!charged)
                {
                    charged = true;
                    StartCoroutine("Charge");                    
                }
                else
                {
                    if (charging)
                    {
                        targetPos = player.transform.position;
                        targetPos.x = targetPos.x - transform.position.x;
                        targetPos.y = targetPos.y - transform.position.y;
                        angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
                        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + offset));
                    }
                    else
                    {
                        transform.position = Vector2.MoveTowards(transform.position, transform.position + transform.up, speed * Time.deltaTime);
                        TurnOnRockandObsidian();
                    }
                }
            }
        }
        else
        {
            if (angered)
            {
                dest = transform.position;
                speed = angeredSpeed;
                angered = false;
            }
            if (transform.position == dest)
            {
                GetComponent<AudioSource>().Stop();
                StartCoroutine("GetNewDest");
                called = true;
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, dest, speed * Time.deltaTime);
                called = false;
                if (digSoundDone)
                {
                    StartCoroutine("DigSound");
                    digSoundDone = false;
                }
            }
            TurnOnRockandObsidian();
        }
    }
    private IEnumerator GetNewDest()
    {
        ani.speed = 0;
        Vector3 tempDest = transform.position;
        if(Random.Range(0,2) < 1)
        {
            int r = Random.Range(-5, 6);
            if(r == 0)
            {
                r = Random.Range(0, 2) * 2 - 1;
            }
            tempDest.x += Random.Range(-5, 6);
        }
        else
        {
            int r = Random.Range(-5, 6);
            if (r == 0)
            {
                r = Random.Range(0, 2) * 2 - 1;
            }
            tempDest.y += Random.Range(-5, 6);
        }
        Vector3 angleVec = tempDest;
        angleVec.x = angleVec.x - transform.position.x;
        angleVec.y = angleVec.y - transform.position.y;
        angle = Mathf.Atan2(angleVec.y, angleVec.x) * Mathf.Rad2Deg;
        yield return new WaitForSeconds(1f);
        ani.speed = 1;
        dest = tempDest;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + offset));
    }



    private IEnumerator DigSound()
    {
        yield return new WaitForSeconds(Random.Range(0.2f,0.35f));
        GetComponent<AudioSource>().Play();
        digSoundDone = true;
    }

    private IEnumerator Charge()
    {
        //prime and look at player
        //charge forward
        //repeat..
        rb.isKinematic = true;
        ZZZ.Stop();
        ani.speed = 2;
        ChangeAnimationState("BigMoleIdle");
        charging = true;
        yield return new WaitForSeconds(3);
        charging = false;
        ani.speed = 1;
        rb.isKinematic = false;
        ChangeAnimationState("BigMole");
        Debug.Log("Charge");
        yield return new WaitForSeconds(3);
        charged = false;

    }

    private void TurnOnRockandObsidian()
    {
        RaycastHit2D[] hit;
        if(boss)
        {
            hit = Physics2D.RaycastAll(transform.position, transform.up, 1.2f);
        }
        else
        {
            hit = Physics2D.RaycastAll(transform.position, transform.up, 0.3f);
        }
        Debug.DrawRay(transform.position, transform.up);
        for (int j = 0; j < hit.Length; j++)
        {
            if (hit[j].collider.gameObject.CompareTag("Rock") || hit[j].collider.gameObject.CompareTag("Obsidian"))
            {
                if (!boss)
                {
                    dest = 2 * transform.position - dest;
                }
                else
                {
                    charged = false;
                }                
            }
        }
    }

    private void SnoreFromAni()
    {
        am.Play("Snore", transform.position, true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Rock") || collision.collider.CompareTag("Obsidian"))
        {
            if (!boss)
            {
                dest = 2 * transform.position - dest;
            }
            else
            {
                charged = false;
            }
        }
    }
}
