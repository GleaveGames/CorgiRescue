using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boomerang : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D cc;
    [SerializeField]
    private float throwPower;
    private bool fired = false;
    [SerializeField] 
    private float tangentForce;
    private Animator ani;
    private string currentState;
    Coroutine coroutine;
    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = null;
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<Collider2D>();
        ani = transform.GetChild(0).gameObject.GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (fired)
        {
            rb.AddForce(-transform.right * tangentForce, ForceMode2D.Impulse);
            transform.up = rb.velocity;
            if (rb.velocity.magnitude <= 5)
            {
                rb.drag = 2;
                fired = false;
                GetComponent<AudioSource>().Stop();
                ChangeAnimationState("BoomerangIdle");
            }
        }
        else
        {
            fired = false;
            ChangeAnimationState("BoomerangIdle");
        }
    }

    public void Fire()
    {
        GetComponent<AudioSource>().Play();
        GetComponent<DamagesPlayer>().canHurt = false;
        StartCoroutine("WaitforHurt");
        cc.isTrigger = false;
        cc.enabled = true;
        rb.drag = 0.3f;
        fired = true;
        Transform leftHand = transform.parent;
        transform.parent = null;
        rb.isKinematic = false;
        Vector3 spread = leftHand.parent.up  + 0.5f * leftHand.parent.right;//transform.parent.parent.up;
        rb.AddForce(spread * throwPower, ForceMode2D.Impulse);
        ChangeAnimationState("BoomerangThrow");
        transform.parent = null;
        FindObjectOfType<LevelGenerator>().itemsForPickUp.Add(gameObject);
    }

    void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;

        ani.Play(newState);

        currentState = newState;
    }
    public IEnumerator WaitforHurt()
    {
        yield return new WaitForSeconds(0.1f);
        Debug.Log("Waited");
        GetComponent<DamagesPlayer>().canHurt = true;
    }
}
