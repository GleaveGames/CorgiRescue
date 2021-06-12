using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Living : MonoBehaviour
{
    [HideInInspector]
    public Vector2 move;
    public float moveSpeed;
    [HideInInspector]
    public Rigidbody2D rb;
    [HideInInspector]
    public Animator ani;
    [HideInInspector]
    public float moveMag;
    [HideInInspector]
    public Vector2 lastMove;
    public bool dead;

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
            lastMove = move;
            ani.speed = 1;
        }
        else 
        {
            ani.speed = 0;
        }
        ani.SetFloat("moveX", lastMove.x);
        ani.SetFloat("moveY", lastMove.y);
    }

    public virtual void Movement()
    {
        move = Vector2.zero;
    }
}
