using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MinerNew : MonoBehaviour
{
    /*
    //public Tilemap tilemap;
    public ContactPoint2D[] contacts = new ContactPoint2D[10];
    private Collider2D coll;
    public bool pick;
    public bool canMine = false;
    [SerializeField]
    private bool canBreakRock = false;
    [SerializeField]
    private bool canBreakObsidian = false;
    [SerializeField]
    private bool soundOn = true;

    [Header("Tile Stuff")]
    public GameObject dirtParticles;
    public GameObject diamondParticles;
    public GameObject goldParticles;
    public GameObject silverParticles;
    public GameObject snowParticles;
    public GameObject rockParticles;
    public GameObject obsidianParticles;

    [SerializeField]
    private int diamondWorth;
    [SerializeField]
    private int goldWorth;
    [SerializeField]
    private int silverWorth;
    private playerStats ps;
    private GameObject newParticles;
    [SerializeField]
    private AudioManager am;
    [SerializeField]
    private GameObject pebble;

    private void Start()
    {
        coll = GetComponent<CircleCollider2D>();
        ps = FindObjectOfType<playerStats>();
        am = FindObjectOfType<AudioManager>();
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (canMine)
        {
            if (collision.gameObject.tag == "Wall")
            {
                //Debug.Log("space pressed");
                int contactCount = collision.contactCount;
                if (contactCount > contacts.Length)
                    contacts = new ContactPoint2D[contactCount];
                collision.GetContacts(contacts);
                UnityEngine.Vector3 hitPosition = UnityEngine.Vector3.zero;
                if (pick)
                {
                    int closest = 0;
                    float dist = 9999;
                    for (int i = 0; i != contactCount; ++i)
                    {
                        float newdist = UnityEngine.Vector3.Distance(transform.position, hitPosition);
                        if (newdist < dist)
                        {
                            dist = newdist;
                            closest = i;
                        }
                    }
                    hitPosition.x = contacts[closest].point.x;
                    hitPosition.y = contacts[closest].point.y;
                    if (collision.gameObject.TryGetComponent(out Tilemap Tm))
                    {
                        string collisionSprite = null;

                        if (hitPosition != null)
                        {
                            if (Tm != null)
                            {
                                if (Tm.WorldToCell(hitPosition) != null)
                                {
                                    if (Tm.GetSprite(Tm.WorldToCell(hitPosition)) != null)
                                    {
                                        collisionSprite = Tm.GetSprite(Tm.WorldToCell(hitPosition)).name;
                                    }
                                }
                            }
                        }
                        //string collisionSprite = Tm.GetSprite(Tm.WorldToCell(hitPosition)).name;
                        if (collisionSprite == null) return;
                        else if (collisionSprite.Contains("Dirt"))  //Dirt
                        {
                            var newParticles = Instantiate(dirtParticles, new UnityEngine.Vector3(Tm.WorldToCell(hitPosition).x + 0.5f, Tm.WorldToCell(hitPosition).y + 0.5f, 0), UnityEngine.Quaternion.identity);
                            if (Random.Range(0, 250) < 1) Instantiate(pebble, hitPosition, UnityEngine.Quaternion.identity);
                            if (soundOn)
                            {
                                am.Play("DirtRule", new UnityEngine.Vector3(Tm.WorldToCell(hitPosition).x + 0.5f, Tm.WorldToCell(hitPosition).y + 0.5f, 0), true);
                            }
                        }
                        else if (collisionSprite.Contains("Diamond"))
                        {
                            var newParticles = Instantiate(diamondParticles, new UnityEngine.Vector3(Tm.WorldToCell(hitPosition).x + 0.5f, Tm.WorldToCell(hitPosition).y + 0.5f, 0), UnityEngine.Quaternion.identity);
                            if (soundOn)
                            {
                                am.Play(collisionSprite, new UnityEngine.Vector3(Tm.WorldToCell(hitPosition).x + 0.5f, Tm.WorldToCell(hitPosition).y + 0.5f, 0), true);
                            }
                        }
                        else if (collisionSprite.Contains("Gold"))
                        {
                            var newParticles = Instantiate(goldParticles, new UnityEngine.Vector3(Tm.WorldToCell(hitPosition).x + 0.5f, Tm.WorldToCell(hitPosition).y + 0.5f, 0), UnityEngine.Quaternion.identity);
                            ps.money += goldWorth;
                            if (soundOn)
                            {
                                am.Play(collisionSprite, new UnityEngine.Vector3(Tm.WorldToCell(hitPosition).x + 0.5f, Tm.WorldToCell(hitPosition).y + 0.5f, 0));
                            }
                        }
                        else if (collisionSprite.Contains("Silver"))
                        {
                            var newParticles = Instantiate(silverParticles, new UnityEngine.Vector3(Tm.WorldToCell(hitPosition).x + 0.5f, Tm.WorldToCell(hitPosition).y + 0.5f, 0), UnityEngine.Quaternion.identity);
                            ps.money += silverWorth;
                            if (soundOn)
                            {
                                am.Play(collisionSprite, new UnityEngine.Vector3(Tm.WorldToCell(hitPosition).x + 0.5f, Tm.WorldToCell(hitPosition).y + 0.5f, 0));
                            }
                        }
                        else if (collisionSprite.Contains("Snow"))
                        {
                            var newParticles = Instantiate(snowParticles, new UnityEngine.Vector3(Tm.WorldToCell(hitPosition).x + 0.5f, Tm.WorldToCell(hitPosition).y + 0.5f, 0), UnityEngine.Quaternion.identity);
                            if (soundOn)
                            {
                                am.Play("SnowRule", new UnityEngine.Vector3(Tm.WorldToCell(hitPosition).x + 0.5f, Tm.WorldToCell(hitPosition).y + 0.5f, 0));
                            }
                        }
                        Tm.SetTile(Tm.WorldToCell(hitPosition), null);

                    }
                }

                else
                {
                    Debug.Log("Still colliding");
                    //only triggering once
                    for (int i = 0; i != contactCount; ++i)
                    {
                        if (collision.gameObject.TryGetComponent(out Tilemap Tm))
                        {
                            //simpler code for test
                            /*
                            hitPosition.x = contacts[i].point.x;
                            hitPosition.y = contacts[i].point.y;
                            var newParticles = Instantiate(dirtParticles, hitPosition, UnityEngine.Quaternion.identity);
                            miningSound.Play();
                            Tm.SetTile(Tm.WorldToCell(hitPosition), null);
                            GetComponent<Rigidbody2D>().isKinematic = true;
                            // old end tag
                            //original broken code                            
                            //Debug.Log(contactCount);
                            //Debug.Log(i);
                            //Debug.Log("made into for");
                            hitPosition.x = contacts[i].point.x;
                            hitPosition.y = contacts[i].point.y;
                            string collisionSprite = null;
                            if (hitPosition != null)
                            {
                                if (Tm != null)
                                {
                                    if (Tm.WorldToCell(hitPosition) != null)
                                    {
                                        if (Tm.GetSprite(Tm.WorldToCell(hitPosition)) != null)
                                        {
                                            collisionSprite = Tm.GetSprite(Tm.WorldToCell(hitPosition)).name;
                                        }
                                    }
                                }
                            }
                            if (collisionSprite == null) return;

                            else if (collisionSprite.Contains("Dirt"))  //Dirt
                            {
                                Debug.Log(collisionSprite);
                                var newParticles = Instantiate(dirtParticles, new UnityEngine.Vector3(Tm.WorldToCell(hitPosition).x + 0.5f, Tm.WorldToCell(hitPosition).y + 0.5f, 0), UnityEngine.Quaternion.identity);
                                if (Random.Range(0, 250) < 1) Instantiate(pebble, hitPosition, UnityEngine.Quaternion.identity);
                                if (soundOn)
                                {
                                    am.Play("DirtRule", new UnityEngine.Vector3(Tm.WorldToCell(hitPosition).x + 0.5f, Tm.WorldToCell(hitPosition).y + 0.5f, 0));
                                }
                            }
                            else if (collisionSprite.Contains("Diamond"))
                            {
                                var newParticles = Instantiate(diamondParticles, new UnityEngine.Vector3(Tm.WorldToCell(hitPosition).x + 0.5f, Tm.WorldToCell(hitPosition).y + 0.5f, 0), UnityEngine.Quaternion.identity);
                                ps.money += diamondWorth;
                                if (soundOn)
                                {
                                    am.Play(collisionSprite, new UnityEngine.Vector3(Tm.WorldToCell(hitPosition).x + 0.5f, Tm.WorldToCell(hitPosition).y + 0.5f, 0));
                                }
                            }
                            else if (collisionSprite.Contains("Gold"))
                            {
                                var newParticles = Instantiate(goldParticles, new UnityEngine.Vector3(Tm.WorldToCell(hitPosition).x + 0.5f, Tm.WorldToCell(hitPosition).y + 0.5f, 0), UnityEngine.Quaternion.identity);
                                ps.money += goldWorth;
                                if (soundOn)
                                {
                                    am.Play(collisionSprite, new UnityEngine.Vector3(Tm.WorldToCell(hitPosition).x + 0.5f, Tm.WorldToCell(hitPosition).y + 0.5f, 0));
                                }
                            }
                            else if (collisionSprite.Contains("Silver"))
                            {
                                var newParticles = Instantiate(silverParticles, new UnityEngine.Vector3(Tm.WorldToCell(hitPosition).x + 0.5f, Tm.WorldToCell(hitPosition).y + 0.5f, 0), UnityEngine.Quaternion.identity);
                                ps.money += silverWorth;
                                if (soundOn)
                                {
                                    am.Play(collisionSprite, new UnityEngine.Vector3(Tm.WorldToCell(hitPosition).x + 0.5f, Tm.WorldToCell(hitPosition).y + 0.5f, 0));
                                }
                            }
                            else if (collisionSprite.Contains("Snow"))
                            {
                                var newParticles = Instantiate(snowParticles, new UnityEngine.Vector3(Tm.WorldToCell(hitPosition).x + 0.5f, Tm.WorldToCell(hitPosition).y + 0.5f, 0), UnityEngine.Quaternion.identity);
                                if (soundOn)
                                {
                                    am.Play("SnowRule", new UnityEngine.Vector3(Tm.WorldToCell(hitPosition).x + 0.5f, Tm.WorldToCell(hitPosition).y + 0.5f, 0));
                                }
                            }
                            else
                            {
                                return;
                            }
                            Tm.SetTile(Tm.WorldToCell(hitPosition), null);

                            GetComponent<Rigidbody2D>().isKinematic = true;

                        }
                    }
                }
            }
            if (canBreakRock)
            {
                if (collision.gameObject.tag == "Rock")
                {
                    //Debug.Log("space pressed");
                    int contactCount = collision.contactCount;
                    if (contactCount > contacts.Length)
                        contacts = new ContactPoint2D[contactCount];
                    collision.GetContacts(contacts);
                    UnityEngine.Vector3 hitPosition = UnityEngine.Vector3.zero;
                    if (pick)
                    {
                        int closest = 0;
                        float dist = 9999;
                        for (int i = 0; i != contactCount; ++i)
                        {
                            float newdist = UnityEngine.Vector3.Distance(transform.position, hitPosition);
                            if (newdist < dist)
                            {
                                dist = newdist;
                                closest = i;
                            }
                        }
                        hitPosition.x = contacts[closest].point.x;
                        hitPosition.y = contacts[closest].point.y;
                        if (collision.gameObject.TryGetComponent(out Tilemap Tm))
                        {
                            Tm.SetTile(Tm.WorldToCell(hitPosition), null);
                            if (Random.Range(0, 100) < 1) Instantiate(pebble, hitPosition, UnityEngine.Quaternion.identity);
                            if (soundOn)
                            {
                                am.Play("RockRule", new UnityEngine.Vector3(Tm.WorldToCell(hitPosition).x + 0.5f, Tm.WorldToCell(hitPosition).y + 0.5f, 0));
                            }
                            var newParticles = Instantiate(rockParticles, new UnityEngine.Vector3(Tm.WorldToCell(hitPosition).x + 0.5f, Tm.WorldToCell(hitPosition).y + 0.5f, 0), UnityEngine.Quaternion.identity);
                        }
                    }
                    else
                    {
                        for (int i = 0; i != contactCount; ++i)
                        {
                            if (collision.gameObject.TryGetComponent(out Tilemap Tm))
                            {
                                string collisionSprite;
                                hitPosition.x = contacts[i].point.x;
                                hitPosition.y = contacts[i].point.y;
                                if (Tm.GetSprite(Tm.WorldToCell(hitPosition)))
                                {
                                    collisionSprite = Tm.GetSprite(Tm.WorldToCell(hitPosition)).name;
                                }
                                else
                                {
                                    return;
                                }
                                if (collisionSprite == null) return;

                                else if (collisionSprite.Contains("Dirt"))  //Dirt
                                {
                                    var newParticles = Instantiate(dirtParticles, new UnityEngine.Vector3(Tm.WorldToCell(hitPosition).x + 0.5f, Tm.WorldToCell(hitPosition).y + 0.5f, 0), UnityEngine.Quaternion.identity);
                                    if (Random.Range(0, 250) < 1) Instantiate(pebble, hitPosition, UnityEngine.Quaternion.identity);
                                    if (soundOn)
                                    {
                                        am.Play("DirtRule", new UnityEngine.Vector3(Tm.WorldToCell(hitPosition).x + 0.5f, Tm.WorldToCell(hitPosition).y + 0.5f, 0));
                                    }
                                }
                                else if (collisionSprite.Contains("Diamond"))
                                {
                                    var newParticles = Instantiate(diamondParticles, new UnityEngine.Vector3(Tm.WorldToCell(hitPosition).x + 0.5f, Tm.WorldToCell(hitPosition).y + 0.5f, 0), UnityEngine.Quaternion.identity);
                                    ps.money += diamondWorth;
                                    if (soundOn)
                                    {
                                        am.Play("Diamond", new UnityEngine.Vector3(Tm.WorldToCell(hitPosition).x + 0.5f, Tm.WorldToCell(hitPosition).y + 0.5f, 0));
                                    }
                                }
                                else if (collisionSprite.Contains("Gold"))
                                {
                                    var newParticles = Instantiate(goldParticles, new UnityEngine.Vector3(Tm.WorldToCell(hitPosition).x + 0.5f, Tm.WorldToCell(hitPosition).y + 0.5f, 0), UnityEngine.Quaternion.identity);
                                    ps.money += goldWorth;
                                    if (soundOn)
                                    {
                                        am.Play("Gold", new UnityEngine.Vector3(Tm.WorldToCell(hitPosition).x + 0.5f, Tm.WorldToCell(hitPosition).y + 0.5f, 0));
                                    }
                                }
                                else if (collisionSprite.Contains("Silver"))
                                {
                                    var newParticles = Instantiate(silverParticles, new UnityEngine.Vector3(Tm.WorldToCell(hitPosition).x + 0.5f, Tm.WorldToCell(hitPosition).y + 0.5f, 0), UnityEngine.Quaternion.identity);
                                    ps.money += silverWorth;
                                    if (soundOn)
                                    {
                                        am.Play("Silver", new UnityEngine.Vector3(Tm.WorldToCell(hitPosition).x + 0.5f, Tm.WorldToCell(hitPosition).y + 0.5f, 0));
                                    }
                                }
                                else
                                {
                                    var newParticles = Instantiate(rockParticles, new UnityEngine.Vector3(Tm.WorldToCell(hitPosition).x + 0.5f, Tm.WorldToCell(hitPosition).y + 0.5f, 0), UnityEngine.Quaternion.identity);
                                }
                                GetComponent<Rigidbody2D>().isKinematic = true;
                                if (Random.Range(0, 100) < 1) Instantiate(pebble, new UnityEngine.Vector3(Tm.WorldToCell(hitPosition).x + 0.5f, Tm.WorldToCell(hitPosition).y + 0.5f, 0), UnityEngine.Quaternion.identity);

                                if (soundOn)
                                {
                                    am.Play("RockRule", new UnityEngine.Vector3(Tm.WorldToCell(hitPosition).x + 0.5f, Tm.WorldToCell(hitPosition).y + 0.5f, 0));
                                }
                                Tm.SetTile(Tm.WorldToCell(hitPosition), null);
                            }
                        }
                    }
                }
            }
            if (canBreakObsidian)
            {
                if (collision.gameObject.tag == "Obsidian")
                {
                    //Debug.Log("space pressed");
                    int contactCount = collision.contactCount;
                    if (contactCount > contacts.Length)
                        contacts = new ContactPoint2D[contactCount];
                    collision.GetContacts(contacts);
                    UnityEngine.Vector3 hitPosition = UnityEngine.Vector3.zero;
                    if (pick)
                    {
                        int closest = 0;
                        float dist = 9999;
                        for (int i = 0; i != contactCount; ++i)
                        {
                            float newdist = UnityEngine.Vector3.Distance(transform.position, hitPosition);
                            if (newdist < dist)
                            {
                                dist = newdist;
                                closest = i;
                            }
                        }
                        hitPosition.x = contacts[closest].point.x;
                        hitPosition.y = contacts[closest].point.y;
                        if (collision.gameObject.TryGetComponent(out Tilemap Tm))
                        {
                            Tm.SetTile(Tm.WorldToCell(hitPosition), null);
                            var newParticles = Instantiate(obsidianParticles, new UnityEngine.Vector3(Tm.WorldToCell(hitPosition).x + 0.5f, Tm.WorldToCell(hitPosition).y + 0.5f, 0), UnityEngine.Quaternion.identity);
                        }
                    }
                    else
                    {
                        for (int i = 0; i != contactCount; ++i)
                        {
                            if (collision.gameObject.TryGetComponent(out Tilemap Tm))
                            {
                                hitPosition.x = contacts[i].point.x;
                                hitPosition.y = contacts[i].point.y;
                                Tm.SetTile(Tm.WorldToCell(hitPosition), null);
                                GetComponent<Rigidbody2D>().isKinematic = true;
                                var newParticles = Instantiate(obsidianParticles, new UnityEngine.Vector3(Tm.WorldToCell(hitPosition).x + 0.5f, Tm.WorldToCell(hitPosition).y + 0.5f, 0), UnityEngine.Quaternion.identity);
                            }
                        }
                    }
                }
            }
        }
    }
    */
}
