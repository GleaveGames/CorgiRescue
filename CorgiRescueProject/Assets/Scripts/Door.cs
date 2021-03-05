using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem ps;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Key(Clone)")
        {
            Destroy(gameObject);
            FindObjectOfType<LevelGenerator>().itemsForPickUp.Remove(collision.gameObject);
            Destroy(collision.gameObject);
            Instantiate(ps, transform.position, Quaternion.identity);
        }
    }
}
