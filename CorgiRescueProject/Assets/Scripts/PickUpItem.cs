using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PickUpItem : PickUpBase
{
    [SerializeField]
    private bool AmIBomb = false;
    [SerializeField]
    private bool AmISword = false;

    protected override void Start() 
    {
        base.Start();
        if (transform.parent == null)
        {
            lg.itemsForPickUp.Add(gameObject);
        }
    }

    private void Update()
    {
        //could maybe put all this into the throw coroutine or something idk
        if (transform.parent == null)
        {
            if (rb.velocity.magnitude <= 3)
            {
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
            //get rid of this
            if (!gameObject.name.Contains("Shotgun"))
            {
                GetComponent<DamagesPlayer>().canHurt = true;
                GetComponent<DamageThisDoes>().damage = GetComponent<DamageThisDoes>().initialDamage;
            }
        }
    }

}
