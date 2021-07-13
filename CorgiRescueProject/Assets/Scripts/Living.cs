using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Living : MonoBehaviour
{
    public float speed;
    public bool stunned;
    public bool stunnable;
    public bool attacking;
    public bool pickupable;
    public bool pickedUp;
    public bool angered;
    public bool canMove;
    public bool knockbackable = true;
    protected Coroutine coroutine;
    [HideInInspector]
    public Animator ani;
    public string currentState;
    [HideInInspector]
    public Transform player;
    public int health = 1;
    private LevelGenerator lg;
    private AudioManager am;
    [SerializeField]
    private GameObject blood;
    public bool attackSoundPlayed;
    [SerializeField]
    AudioSource attackSoundAudio;
    protected Rigidbody2D rb;
    [SerializeField]
    LayerMask tiles;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!pickupable) Destroy(GetComponent<PickUpEnemy>());
        lg = FindObjectOfType<LevelGenerator>();
        am = FindObjectOfType<AudioManager>();
        ani = GetComponent<Animator>();
        ani.speed = Random.Range(0.9f, 1.1f);
        StartCoroutine(GetPlayer());
    }

    protected virtual void Update()
    {
        if (health < 1) Die();
        if(!canMove) return;
    }

    private IEnumerator GetPlayer() 
    {
        while(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            yield return null; 
        }
    }

    public void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        ani.Play(newState);
        currentState = newState;
    }

    public void Die() 
    {
        lg.livingThings.Remove(gameObject);
        Destroy(gameObject);
        Instantiate(blood, transform.position, Quaternion.identity);
        am.Play("Hit", transform.position, true);
    }

    protected RaycastHit2D ClosestRaycast(Vector2 direction)
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

    protected RaycastHit2D ClosestWall(Vector2 direction)
    { 
        //working YOU MUST USE A DISTANCE FOR LAYERMASK TO WORK
        RaycastHit2D closestWall = Physics2D.Raycast(transform.position, direction, 9999, tiles);
        return closestWall;
    }

    protected IEnumerator AttackSound()
    {
        attackSoundAudio.Play();
        yield return new WaitForSeconds(3);
        attackSoundPlayed = false;
    }

    public void KnockBack(float time, Vector3 force)
    {
        StartCoroutine(StartKnockBack(force, time));
    }

    public IEnumerator StartKnockBack(Vector2 knockbackForce, float time)
    {
        rb.AddForce(knockbackForce, ForceMode2D.Impulse);
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }
}
