using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Miner : MonoBehaviour
{
    [HideInInspector]
    public Tilemap[] tilemaps;
    [HideInInspector]
    public ContactPoint2D[] contacts = new ContactPoint2D[10];
    public bool canMine = false;
    [SerializeField]
    private bool canBreakRock = false;
    [SerializeField]
    private bool canBreakObsidian = false;
    [SerializeField]
    private bool soundOn = true;

    [Header ("Tile Stuff")]
    public GameObject dirtParticles;
    public GameObject diamondParticles;
    public GameObject goldParticles;
    public GameObject silverParticles;
    public GameObject snowParticles;
    public GameObject rockParticles;
    public GameObject obsidianParticles;
    public GameObject iceParticles;

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
        ps = FindObjectOfType<playerStats>();
        am = FindObjectOfType<AudioManager>();
        tilemaps = new Tilemap[3];
        tilemaps[0] = GameObject.FindGameObjectWithTag("Node1").transform.GetChild(0).transform.Find("Walls").GetComponent<Tilemap>();
        tilemaps[1] = GameObject.FindGameObjectWithTag("Node1").transform.GetChild(0).transform.Find("Rock").GetComponent<Tilemap>();
        tilemaps[2] = GameObject.FindGameObjectWithTag("Node1").transform.GetChild(0).transform.Find("Obsidian").GetComponent<Tilemap>();
    }
    private void Update()
    {
        if(tilemaps[0] == null)
        {
            tilemaps[0] = GameObject.FindGameObjectWithTag("Node1").transform.GetChild(0).transform.Find("Walls").GetComponent<Tilemap>();
            tilemaps[1] = GameObject.FindGameObjectWithTag("Node1").transform.GetChild(0).transform.Find("Rock").GetComponent<Tilemap>();
            tilemaps[2] = GameObject.FindGameObjectWithTag("Node1").transform.GetChild(0).transform.Find("Obsidian").GetComponent<Tilemap>();
        }
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        if (canMine)
        {
            int contactCount = collision.contactCount;
            if (contactCount > contacts.Length)
                contacts = new ContactPoint2D[contactCount];
            collision.GetContacts(contacts);
            UnityEngine.Vector3 hitPosition = UnityEngine.Vector3.zero;
            for (int i = 0; i != contactCount; ++i)
            {
                hitPosition.x = contacts[i].point.x;
                hitPosition.y = contacts[i].point.y;
                string collisionSprite = null;
                //Below actually gives true for any tile in that position in any of the tilemaps.
                if (tilemaps[0].WorldToCell(hitPosition) != null)
                {
                    if (tilemaps[0].GetSprite(tilemaps[0].WorldToCell(hitPosition)) != null)
                    {
                        collisionSprite = tilemaps[0].GetSprite(tilemaps[0].WorldToCell(hitPosition)).name;
                        tilemaps[0].SetTile(tilemaps[0].WorldToCell(hitPosition), null);
                        TileDestroy(collisionSprite, tilemaps[0], hitPosition);
                    }
                    if (canBreakRock)
                    {
                        if (tilemaps[1].GetSprite(tilemaps[1].WorldToCell(hitPosition)) != null)
                        {
                            collisionSprite = tilemaps[1].GetSprite(tilemaps[1].WorldToCell(hitPosition)).name;
                            tilemaps[1].SetTile(tilemaps[1].WorldToCell(hitPosition), null);
                            TileDestroy(collisionSprite, tilemaps[1], hitPosition);
                        }
                    }
                    if (canBreakObsidian)
                    {
                        if (tilemaps[2].GetSprite(tilemaps[1].WorldToCell(hitPosition)) != null)
                        {
                            collisionSprite = tilemaps[2].GetSprite(tilemaps[2].WorldToCell(hitPosition)).name;
                            tilemaps[2].SetTile(tilemaps[2].WorldToCell(hitPosition), null);
                            TileDestroy(collisionSprite, tilemaps[2], hitPosition);
                        }
                    }
                }
            }
        }        
    }
    private void TileDestroy(string name, Tilemap Tm, UnityEngine.Vector3 hitPosition)
    {
        string sound = null;
        if (name.Contains("Diamond"))
        {
            sound = "Dirt";
            var newParticles = Instantiate(diamondParticles, new UnityEngine.Vector3(Tm.WorldToCell(hitPosition).x + 0.5f, Tm.WorldToCell(hitPosition).y + 0.5f, 0), UnityEngine.Quaternion.identity);
        }
        else if (name.Contains("Gold"))
        {
            sound = "Dirt";
            var newParticles = Instantiate(goldParticles, new UnityEngine.Vector3(Tm.WorldToCell(hitPosition).x + 0.5f, Tm.WorldToCell(hitPosition).y + 0.5f, 0), UnityEngine.Quaternion.identity);
        }
        else if (name.Contains("Silver"))
        {
            sound = "Dirt";
            var newParticles = Instantiate(silverParticles, new UnityEngine.Vector3(Tm.WorldToCell(hitPosition).x + 0.5f, Tm.WorldToCell(hitPosition).y + 0.5f, 0), UnityEngine.Quaternion.identity);
        }
        else if (name.Contains("Dirt"))
        {
            sound = "Dirt";
            var newParticles = Instantiate(dirtParticles, new UnityEngine.Vector3(Tm.WorldToCell(hitPosition).x + 0.5f, Tm.WorldToCell(hitPosition).y + 0.5f, 0), UnityEngine.Quaternion.identity);
            if (Random.Range(0, 250) < 1) Instantiate(pebble, hitPosition, UnityEngine.Quaternion.identity);
        }
        else if (name.Contains("Rock"))
        {
            sound = "Rock";
            if (Random.Range(0, 100) < 1) Instantiate(pebble, hitPosition, UnityEngine.Quaternion.identity);
            var newParticles = Instantiate(rockParticles, new UnityEngine.Vector3(Tm.WorldToCell(hitPosition).x + 0.5f, Tm.WorldToCell(hitPosition).y + 0.5f, 0), UnityEngine.Quaternion.identity);
        }
        
        else if (name.Contains("Snow"))
        {
            sound = "Snow";
            var newParticles = Instantiate(snowParticles, new UnityEngine.Vector3(Tm.WorldToCell(hitPosition).x + 0.5f, Tm.WorldToCell(hitPosition).y + 0.5f, 0), UnityEngine.Quaternion.identity);
        }
        else if (name.Contains("Ice"))
        {
            sound = "Ice";
            var newParticles = Instantiate(iceParticles, new UnityEngine.Vector3(Tm.WorldToCell(hitPosition).x + 0.5f, Tm.WorldToCell(hitPosition).y + 0.5f, 0), UnityEngine.Quaternion.identity);
        }
        else{
            Debug.Log(name + " tile hasn't been configured.");
        }
        if (soundOn)
        {
            am.Play(sound, new UnityEngine.Vector3(Tm.WorldToCell(hitPosition).x + 0.5f, Tm.WorldToCell(hitPosition).y + 0.5f, 0), false);
        }
    }
}
