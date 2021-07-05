using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpBase : MonoBehaviour
{
    //public bool InHand = false;
    protected Transform leftHand;
    protected Rigidbody2D rb;
    [SerializeField]
    protected float throwPower;
    protected Collider2D cc;
    Coroutine coroutine;
    protected LevelGenerator lg;
    protected AudioManager am;
    protected int damagethisdoesinit;

    protected virtual void Start()
    {
        lg = FindObjectOfType<LevelGenerator>();
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<Collider2D>();
        if (transform.parent != null)
        {
            //rb.isKinematic = true;
            rb.velocity = new Vector3(0f, 0f, 0f);
        }
        if (transform.parent == null)
        {
            lg.itemsForPickUp.Add(gameObject);
        }
        am = FindObjectOfType<AudioManager>();
        damagethisdoesinit = GetComponent<DamageThisDoes>().damage;
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
        cc.enabled = false;
        Debug.Log("cc.enabled = false");
        transform.position = lH.position;
        transform.parent = lH;
        rb.isKinematic = true;
        rb.velocity = new Vector3(0f, 0f, 0f);
        lg.itemsForPickUp.Remove(gameObject);
    }

    public void Throw()
    {
        if (TryGetComponent(out Sword sword))
        {
            sword.ChangeAnimationState("Idle");
        }
        am.Play("Throw", transform.position, true);
        GetComponent<DamagesPlayer>().canHurt = false;
        StartCoroutine("WaitforHurt");
        transform.parent = null;
        rb.isKinematic = false;
        rb.AddForce(leftHand.parent.up * throwPower, ForceMode2D.Impulse);
        //cc.isTrigger = false;
        //re-enable cc;
        cc.enabled = true;

        //SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
        lg.itemsForPickUp.Add(gameObject);
    }
    public void Drop()
    {
        if (TryGetComponent(out Sword sword))
        {
            sword.ChangeAnimationState("Idle");
        }
        am.Play("Drop", transform.position, true);
        GetComponent<DamagesPlayer>().canHurt = false;
        StartCoroutine("WaitforHurt");
        transform.parent = null;
        rb.isKinematic = false;
        rb.AddForce(leftHand.parent.up * throwPower / 5, ForceMode2D.Impulse);
        cc.isTrigger = false;
        lg.itemsForPickUp.Add(gameObject);
        //SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
    }

    public IEnumerator WaitforHurt()
    {
        yield return new WaitForSeconds(0.1f);
        //seems to work for now
        //could make value above specific to item depending on its speed etc
        GetComponent<DamagesPlayer>().canHurt = true;
        GetComponent<DamageThisDoes>().damage = GetComponent<DamageThisDoes>().initialDamage;
    }
}
