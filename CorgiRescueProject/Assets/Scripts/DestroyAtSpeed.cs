using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAtSpeed : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private ParticleSystem deathParticles;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(rb.velocity.magnitude < speed)
        {
            Destroy(gameObject);
            Instantiate(deathParticles, transform.position, Quaternion.identity);
        }
    }


}
