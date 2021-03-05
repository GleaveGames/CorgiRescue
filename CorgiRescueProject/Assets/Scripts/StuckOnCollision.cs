using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuckOnCollision : MonoBehaviour
{
    public bool stuck;
    private Rigidbody2D rb;
    private Collider2D col;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
        transform.parent = collision.gameObject.transform;
        rb.angularVelocity = 0;
        col.isTrigger = true;
    }
}
