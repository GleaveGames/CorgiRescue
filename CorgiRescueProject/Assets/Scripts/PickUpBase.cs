using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpBase : MonoBehaviour
{
    //public bool InHand = false;
    protected Transform leftHand;
    protected Rigidbody2D rb;
    [SerializeField]
    protected float throwPowerModifier = 1;
    [SerializeField]
    public float attackRange = 5;
    protected Collider2D cc;
    Coroutine coroutine;
    protected LevelGenerator lg;
    protected AudioManager am;
    protected int damagethisdoesinit;
    protected LayerMask initLayer;
    playerStats ps;



    protected virtual void Start()
    {
        lg = FindObjectOfType<LevelGenerator>();
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<Collider2D>();
        ps = FindObjectOfType<playerStats>();
        if (transform.parent != null)
        {
            //rb.isKinematic = true;
            rb.velocity = new Vector3(0f, 0f, 0f);
        }
        am = FindObjectOfType<AudioManager>();
        damagethisdoesinit = GetComponent<DamageThisDoes>().damage;
        initLayer = gameObject.layer;
    }


    public void PickUp(Transform lH)
    {
        if (TryGetComponent(out Sword sword))
        {
            sword.ChangeAnimationState("Idle");
        }
        am.Play("PickUp", transform.position, true);
        leftHand = lH;
        //cc.isTrigger = true;
        // need to disable collider and re-enable because otherwise you can just run into enemies with a weapon and it will kill them
        DisableCollision();
        transform.position = lH.position;
        transform.parent = lH;
    }

    public void Throw()
    {
        transform.position = leftHand.parent.position + leftHand.parent.up/3;
        if (TryGetComponent(out Sword sword))
        {
            sword.ChangeAnimationState("Idle");
        }
        am.Play("Throw", transform.position, true);
        EnableCollision();
        rb.AddForce(leftHand.parent.up * throwPowerModifier * ps.throwPower, ForceMode2D.Impulse);
    }

    public void Drop()
    {
        if (TryGetComponent(out Sword sword))
        {
            sword.ChangeAnimationState("Idle");
        }
        am.Play("Drop", transform.position, true);
        EnableCollision();
        rb.AddForce(leftHand.parent.up * ps.throwPower * throwPowerModifier / 5, ForceMode2D.Impulse);
    }

    public IEnumerator WaitforHurt()
    {
        yield return new WaitForSeconds(0.05f);
        //seems to work for now
        //could make value above specific to item depending on its speed etc
        GetComponent<DamagesPlayer>().canHurt = true;
        GetComponent<DamageThisDoes>().damage = GetComponent<DamageThisDoes>().initialDamage;
        yield return new WaitForSeconds(0.6f);
        if(!lg.itemsForPickUp.Contains(gameObject)) lg.itemsForPickUp.Add(gameObject);
    }

    public IEnumerator WaitForHurtBump()
    {
        //For when you bump into an item that's not been thrown or anything
        yield return new WaitForSeconds(0.1f);
        GetComponent<DamagesPlayer>().canHurt = true;
        GetComponent<DamageThisDoes>().damage = GetComponent<DamageThisDoes>().initialDamage;
    }

    public virtual void EnableCollision() 
    {
        GetComponent<DamagesPlayer>().canHurt = false;
        StartCoroutine(WaitforHurt());
        transform.parent = null;
        rb.isKinematic = false;
        cc.enabled = true;
        cc.isTrigger = false;
    }

    public virtual void DisableCollision() 
    {
        cc.enabled = false;
        rb.isKinematic = true;
        rb.velocity = new Vector3(0f, 0f, 0f);
        lg.itemsForPickUp.Remove(gameObject);
    }
}
