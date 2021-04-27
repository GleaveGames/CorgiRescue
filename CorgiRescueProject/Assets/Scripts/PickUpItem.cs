using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PickUpItem : MonoBehaviour
{
    //public bool InHand = false;
    private Transform leftHand;
    private Rigidbody2D rb;
    [SerializeField]
    private float throwPower;
    private Collider2D cc;
    Coroutine coroutine;
    private LevelGenerator lg;
    private AudioManager am;
    [SerializeField]
    private bool AmIBomb = false;
    [SerializeField]
    private bool AmISword = false;
    private int damagethisdoesinit;

    private void Start()
    {
        lg = FindObjectOfType<LevelGenerator>();
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<Collider2D>();
        if(transform.parent != null)
        {
            //rb.isKinematic = true;
            rb.velocity = new Vector3(0f, 0f, 0f);
        }
        if(transform.parent == null)
        {
            lg.itemsForPickUp.Add(gameObject);
        }
        am = FindObjectOfType<AudioManager>();
        damagethisdoesinit = GetComponent<DamageThisDoes>().damage;
    }

    private void Update()
    {

        //could maybe put all this into the throw coroutine or something idk
        if (transform.parent == null)
        {
            if (rb.velocity.magnitude <= 3)
            {
                /*
                if (!AmIBomb)
                {
                    cc.isTrigger = true;
                }
                */
                //rb.velocity = new Vector3(0f, 0f, 0f);
                /*
                if (!AmIBomb)
                {
                    GetComponent<DamagesPlayer>().canHurt = false;
                }
                */
                //changed this from above to try fix bomby from hurting player
                GetComponent<DamagesPlayer>().canHurt = false;
            }
            else
            {
                StartCoroutine("WaitforHurt");
                GetComponent<DamageThisDoes>().damage = damagethisdoesinit;
            }
        }
        else
        {
            if (!gameObject.name.Contains("Shotgun"))
            {
                GetComponent<DamagesPlayer>().canHurt = true;
                GetComponent<DamageThisDoes>().damage = GetComponent<DamageThisDoes>().initialDamage;
            }
        }
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
        transform.position = lH.position;
        transform.parent = lH;
        rb.isKinematic = true;
        rb.velocity = new Vector3(0f,0f,0f);
        lg.itemsForPickUp.Remove(gameObject);
        

        //Quaternion rot = transform.parent.parent.rotation;
        //Debug.Log(rot);
        //Quaternion newRot = transform.localRotation;
        //newRot.z += rot.z;
        //rot.z += 90;
        //transform.localRotation = newRot;
    }

    public void Throw()
    {
        am.Play("Throw", transform.position, true);
        GetComponent<DamagesPlayer>().canHurt = false;
        StartCoroutine("WaitforHurt");        
        transform.parent = null;
        rb.isKinematic = false;
        rb.AddForce(leftHand.parent.up*throwPower, ForceMode2D.Impulse);
        //cc.isTrigger = false;
        //re-enable cc;
        cc.enabled = true;

        //SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
        lg.itemsForPickUp.Add(gameObject);
    }
    public void Drop()
    {
        am.Play("Drop", transform.position, true);
        GetComponent<DamagesPlayer>().canHurt = false;
        StartCoroutine("WaitforHurt");        
        transform.parent = null;
        rb.isKinematic = false;
        rb.AddForce(leftHand.parent.up*throwPower/5, ForceMode2D.Impulse);
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
