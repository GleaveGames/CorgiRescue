using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Living : MonoBehaviour
{
    public Vector2 move;
    public float moveSpeed;
    public Rigidbody2D rb;
    public Animator ani;
    public float moveMag;
    public Vector2 lastMove;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
    }

    protected virtual void FixedUpdate()
    {
        rb.AddForce(move * moveSpeed);
    }

    protected virtual void Update()
    {
        Movement();
        moveMag = move.magnitude;
        if (moveMag > 0.05f)
        {
            ani.SetBool("Idle", true);
            lastMove = move;
        }
        else 
        {
            ani.SetBool("Idle", true);
        }
        ani.SetFloat("moveX", lastMove.x);
        ani.SetFloat("moveY", lastMove.y);
    }

    public virtual void Movement()
    {
        move = Vector2.zero;
    }
}
