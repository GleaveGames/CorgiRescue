using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField]
    private AudioSource tick;
    [SerializeField]
    private float bombPushForce;
    [SerializeField]
    private ParticleSystem explosionParticles;
    private AudioManager am;
    private cameraoptions co;
    public bool canBreakCreates = false;
    private LevelGenerator lg;

    private void Start()
    {
        am = FindObjectOfType<AudioManager>();
        co = FindObjectOfType<cameraoptions>();
        lg = FindObjectOfType<LevelGenerator>();
        if (FindObjectOfType<playerStats>().bigbombs) GetComponent<Animator>().speed = 0.5f;
        //lg.itemsForPickUp.Add(gameObject);
    }
    public void DestroyFromAni()
    {
        Destroy(gameObject);
    }

    public void ExplosionSound()
    {
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        GetComponent<CircleCollider2D>().isTrigger = false;
        lg.itemsForPickUp.Remove(gameObject);
        canBreakCreates = true;
        am.Play("Explosion", transform.position);
        ParticleSystem parts = Instantiate(explosionParticles, transform.position, Quaternion.identity);
        parts.transform.localScale = transform.localScale;
        GetComponent<Miner>().canMine = true;
        GetComponent<DamagesPlayer>().canHurt = true;
        GetComponent<DamagesPlayer>().canHurtPlayer = true;
        co.shakeDuration = 0.3f;
    }

    public void BombTick()
    {
        tick.pitch = 1 + Random.Range(-0.3f, 0.3f);
        tick.Play();
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.name == "Player" || collision.collider.name == "Living")
        {
            Vector3 dir = collision.gameObject.transform.position - transform.position;
            // We then get the opposite (-Vector3) and normalize it
            dir = -dir.normalized;
            // And finally we add force in the direction of dir and multiply it by force. 
            // This will push back the player
            if(TryGetComponent(out Rigidbody2D rb))
            {
                rb.AddForce(dir * bombPushForce);
            }
        }
    }

}
