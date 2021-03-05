using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiesToBombs : MonoBehaviour
{
    [SerializeField]
    private GameObject particles;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.name == ("Bomb(Clone)"))
        {
            if (collision.collider.gameObject.GetComponent<Bomb>().canBreakCreates)
            {
                Instantiate(particles, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}