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
    Coroutine coroutine;
    public GameObject[] items;
    public ParticleSystem crateparticles;
    private bool spawned = false;
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!spawned)
        {

            if (collision.collider.gameObject.name == ("Bomb(Clone)"))
            {
                if (collision.collider.gameObject.GetComponent<Bomb>().canBreakCreates)
                {
                    spawned = true;
                    StartCoroutine("Spawn");
                }
            }

            else if (wood)
            {
                if (!collision.collider.gameObject.name.Contains("Player") && !collision.collider.gameObject.name.Contains("Bat") && !collision.collider.gameObject.name.Contains("Tilemap") &&
                    !collision.collider.gameObject.name.Contains("Rock") && !collision.collider.gameObject.name.Contains("Wall") && !collision.collider.gameObject.name.Contains("Obsidian") && !collision.collider.gameObject.name.Contains("Snek"))
                {
                    spawned = true;
                    StartCoroutine("Spawn");
                }
            }
        }
    }

    private IEnumerator Spawn()
    {
        Instantiate(crateparticles, transform.position, Quaternion.identity);
        FindObjectOfType<AudioManager>().Play("CrateWood", transform.position, true);
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        GameObject newThing = Instantiate(items[Random.Range(0, items.Length)], transform.position, Quaternion.identity);
        if (!newThing.gameObject.name.Contains("Gemsplosion"))
        {
            newThing.GetComponent<Collider2D>().enabled = false;
            yield return new WaitForSeconds(0.5f);
            newThing.GetComponent<Collider2D>().enabled = true;
        }
        Destroy(gameObject);
    }
}
