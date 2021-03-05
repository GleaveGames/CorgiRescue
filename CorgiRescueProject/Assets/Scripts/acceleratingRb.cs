using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class acceleratingRb : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField]
    private float acceleration;
    [SerializeField]
    private bool BigSnowBall = true;
    private RaycastHit2D hit;
    public Transform[] raycastspawns;
    [SerializeField]
    private float rayrange = 0.3f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        rb.AddForce(rb.velocity.normalized * acceleration);
    }

    private void Update()
    {
        if (BigSnowBall)
        {
            for(int i = 0; i<raycastspawns.Length; i++)
            {
                hit = Physics2D.Raycast(raycastspawns[i].position, transform.up, rayrange);
                if (hit.collider != null)
                {
                    if (hit.collider.name.Contains("IceSpike"))
                    {
                        Destroy(hit.collider.gameObject);
                    }
                }
            }
                
        }
    }
}
