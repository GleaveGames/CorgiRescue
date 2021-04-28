using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnocksThingsOnTrigger : MonoBehaviour
{
    [SerializeField]
    float power;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PickupItems")) 
        {
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(power * transform.up, ForceMode2D.Impulse);
        }
    }

}        
