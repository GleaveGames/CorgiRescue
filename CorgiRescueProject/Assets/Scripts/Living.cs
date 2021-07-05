using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Living : MonoBehaviour
{
    public float speed;
    public bool stunned;
    public bool attacking;
    public bool pickupable;
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

    protected virtual void Start()
    {
        lg = FindObjectOfType<LevelGenerator>();
        am = FindObjectOfType<AudioManager>();
        ani = GetComponent<Animator>();
        StartCoroutine(GetPlayer());
    }

    protected virtual void Update()
    {
        if (health < 1)
        {
            Die();
        }
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
}
