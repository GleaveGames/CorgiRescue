using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Living : MonoBehaviour
{
    public Vector2 move;
    public float moveSpeed;
    public Rigidbody2D rb;
    public Animator ani;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Movement();
        rb.AddForce(move * moveSpeed);
    }

    public virtual void Movement()
    {
        move = Vector2.zero;
    }
}
