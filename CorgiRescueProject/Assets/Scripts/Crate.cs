using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    [Header("CrateType")]
    [SerializeField]
    private bool metal;
    [SerializeField]
    private bool wood;
    [SerializeField]
    private bool present;

    public GameObject[] items;
    public ParticleSystem crateparticles;
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.name == ("Bomb(Clone)"))
        {
            if (collision.collider.gameObject.GetComponent<Bomb>().canBreakCreates)
            {
                Instantiate(crateparticles, transform.position, Quaternion.identity);
                GameObject newThing = Instantiate(items[Random.Range(0, items.Length)], transform.position, Quaternion.identity);
                Destroy(gameObject);
            }            
        }

        else if (wood)
        {
            if (!collision.collider.gameObject.name.Contains("Player") && !collision.collider.gameObject.name.Contains("Bat") && !collision.collider.gameObject.name.Contains("Tilemap") && 
                !collision.collider.gameObject.name.Contains("Rock") && !collision.collider.gameObject.name.Contains("Wall") && !collision.collider.gameObject.name.Contains("Obsidian") && !collision.collider.gameObject.name.Contains("Snek"))
            {
                Instantiate(crateparticles, transform.position, Quaternion.identity);
                GameObject newThing = Instantiate(items[Random.Range(0, items.Length)], transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}
