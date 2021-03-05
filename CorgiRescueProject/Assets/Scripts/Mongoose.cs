using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mongoose : MonoBehaviour
{
    private GameObject player;
    [SerializeField]
    private bool triggered;
    [SerializeField]
    private float speed;
    private Animator ani;
    private string currentState;
    private Quaternion rot;
    private float timer = 0;
    private float timer2 = 0;
    [SerializeField]
    private ParticleSystem ps;
    private bool digSoundDone = true;
    Coroutine coroutine;
    private bool rotated;
    private float turntimer;
    public bool angered;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (angered)
        {
            triggered = true;
            angered = false;
        }
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right);
        if (triggered)
        {
            if (timer2 > 1)
            {
                rot = Quaternion.Euler(0, 0, Random.Range(-30, 30));
                timer2 = 0;
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime);
            transform.position = Vector2.MoveTowards(transform.position, transform.position + transform.right, speed * Time.deltaTime);
            if (digSoundDone)
            {
                StartCoroutine("DigSound");
                digSoundDone = false;
            }
            timer += Time.deltaTime;
            timer2 += Time.deltaTime;
            
            if (timer > 4)
            {
                triggered = false;
                timer = 0;
            }            
        }        
        else if (Vector2.Distance(hit.point, transform.position) > 0.05)
        {
            //timer = 0;
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Player") || hit.collider.name == "Snek")
                {
                    triggered = true;
                    ps.Stop();
                }
            }
            //see if out in open
            RaycastHit2D[] hitBack = Physics2D.RaycastAll(transform.position, -transform.right);
            if(Vector2.Distance(hitBack[1].point, transform.position) > 0.6)
            {
                triggered = true;
            }
        }
        else
        {
            ps.Play();
            triggered = false;
            timer2 += Time.deltaTime;
            if (timer2 > 1)
            {
                rot = Quaternion.Euler(0, 0, Random.Range(-180, 181));
                timer2 = 0;
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 3);
            Vector3 target = transform.position + transform.right;
            transform.position = Vector2.MoveTowards(transform.position, target, (speed/2)*Time.deltaTime);
        }
        TurnOnRockandObsidian();
    }   
    private IEnumerator DigSound()
    {
        yield return new WaitForSeconds(Random.Range(0.2f,0.35f));
        GetComponent<AudioSource>().Play();
        digSoundDone = true;
    }
    private void TurnOnRockandObsidian()
    {
        if (!rotated)
        {
            RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, transform.up, 0.3f);
            Debug.DrawRay(transform.position, transform.up);
            for (int j = 0; j < hit.Length; j++)
            {
                //Debug.Log(hit[j].collider.name);
                //check if hit
                if (hit[j].collider.gameObject.CompareTag("Rock") || hit[j].collider.gameObject.CompareTag("Obsidian"))
                {
                    rotated = true;
                    transform.Rotate(0, 0, 180f);
                    turntimer = 0;
                }
            }
        }
        if(turntimer > 2)
        {
            rotated = false;
        }
        turntimer += Time.deltaTime;
    }
}
