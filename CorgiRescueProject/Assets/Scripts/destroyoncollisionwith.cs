using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyoncollisionwith : MonoBehaviour
{
    [SerializeField]
    private string thing;
    [SerializeField]
    private GameObject particles;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name.Contains(thing))
        {
            Destroy(gameObject);
            Instantiate(particles, transform.position, Quaternion.identity);
        }
    }
}
