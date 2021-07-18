using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Miner : MonoBehaviour
{
    [SerializeField]
    bool OneAtATime = false;

    [HideInInspector]
    public Tilemap[] tilemaps;
    [HideInInspector]
    public ContactPoint2D[] contacts = new ContactPoint2D[10];
    public bool canMine = false;
    [SerializeField]
    private bool canBreakRock = false;
    [SerializeField]
    private bool canDamageRock = false;
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
    public GameObject littleRockParticles;
    public GameObject obsidianParticles;
    public GameObject iceParticles;
    public GameObject woodParticles;

    [Header("Rock Tiles")]
    [SerializeField]
    private TileBase[] rockTiles; 

    [SerializeField]
    private AudioManager am;
    [SerializeField]
    private GameObject pebble;

    private void Start()
    {
        am = FindObjectOfType<AudioManager>();
        tilemaps = new Tilemap[4];
        StartCoroutine(WaitToGetTiles());
    }

    private IEnumerator MinePause() 
    {
        canMine = false;
        yield return new WaitForSeconds(0.3f);
        canMine = true;
    }

    private IEnumerator WaitToGetTiles() 
    {
        if (canMine)
        {
            canMine = false;
            yield return new WaitForSeconds(1.5f);
            tilemaps[0] = GameObject.FindGameObjectWithTag("Node1").transform.GetChild(0).transform.Find("Walls").GetComponent<Tilemap>();
            tilemaps[1] = GameObject.FindGameObjectWithTag("Node1").transform.GetChild(0).transform.Find("Rock").GetComponent<Tilemap>();
            tilemaps[2] = GameObject.FindGameObjectWithTag("Node1").transform.GetChild(0).transform.Find("Obsidian").GetComponent<Tilemap>();
            tilemaps[3] = GameObject.FindGameObjectWithTag("Node1").transform.GetChild(0).transform.Find("GemsTilemap(Clone)").GetComponent<Tilemap>();
            canMine = true;
        }
        else 
        {
            yield return new WaitForSeconds(1.5f);
            tilemaps[0] = GameObject.FindGameObjectWithTag("Node1").transform.GetChild(0).transform.Find("Walls").GetComponent<Tilemap>();
            tilemaps[1] = GameObject.FindGameObjectWithTag("Node1").transform.GetChild(0).transform.Find("Rock").GetComponent<Tilemap>();
            tilemaps[2] = GameObject.FindGameObjectWithTag("Node1").transform.GetChild(0).transform.Find("Obsidian").GetComponent<Tilemap>();
            tilemaps[3] = GameObject.FindGameObjectWithTag("Node1").transform.GetChild(0).transform.Find("GemsTilemap(Clone)").GetComponent<Tilemap>();
        }
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        if (canMine)
        {
            int contactCount = collision.contactCount;
            if (contactCount > contacts.Length) contacts = new ContactPoint2D[contactCount];
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
                    //TEST CODE
                    else if(canDamageRock)
                    {
                        if (tilemaps[1].GetSprite(tilemaps[1].WorldToCell(hitPosition)) != null)
                        {
                            collisionSprite = tilemaps[1].GetSprite(tilemaps[1].WorldToCell(hitPosition)).name;
                            if (collisionSprite.Contains("Rock"))
                            {
                                RockTileUpdate(collisionSprite, hitPosition);   
                            }
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



    private void RockTileUpdate(string collisionSprite, UnityEngine.Vector3 hitPosition)
    {
        //if(collisionSprite == rockTiles[rockTiles.Length - 1].name) 
        if(collisionSprite.Contains("3"))
        {
            tilemaps[1].SetTile(tilemaps[1].WorldToCell(hitPosition), null);
            TileDestroy(collisionSprite, tilemaps[1], hitPosition);
        }
        else 
        {
            if (collisionSprite.Contains("1")) tilemaps[1].SetTile(tilemaps[1].WorldToCell(hitPosition), rockTiles[2]);
            else if (collisionSprite.Contains("2")) tilemaps[1].SetTile(tilemaps[1].WorldToCell(hitPosition), rockTiles[3]);
            //etc
            else tilemaps[1].SetTile(tilemaps[1].WorldToCell(hitPosition), rockTiles[1]);
            if (Random.Range(0, 100) < 1) Instantiate(pebble, hitPosition, UnityEngine.Quaternion.identity);
            Instantiate(littleRockParticles, new UnityEngine.Vector3(tilemaps[1].WorldToCell(hitPosition).x + 0.5f, tilemaps[1].WorldToCell(hitPosition).y + 0.5f, 0), UnityEngine.Quaternion.identity);
            if (soundOn)
            {
                am.Play("Rock", new UnityEngine.Vector3(tilemaps[1].WorldToCell(hitPosition).x + 0.5f, tilemaps[1].WorldToCell(hitPosition).y + 0.5f, 0), false);
            }
        }
        if (OneAtATime) StartCoroutine(MinePause());
    }

    private void GemDestroy(UnityEngine.Vector3 hitPosition)
    {
        if (tilemaps[3].GetSprite(tilemaps[3].WorldToCell(hitPosition)) != null)
        {
            string name = tilemaps[3].GetSprite(tilemaps[3].WorldToCell(hitPosition)).name;
            tilemaps[3].SetTile(tilemaps[3].WorldToCell(hitPosition), null);
            if (name.Contains("Diamond"))
            {
                Instantiate(diamondParticles, new UnityEngine.Vector3(tilemaps[3].WorldToCell(hitPosition).x + 0.5f, tilemaps[3].WorldToCell(hitPosition).y + 0.5f, 0), UnityEngine.Quaternion.identity);
            }
            else if (name.Contains("Gold"))
            {
                Instantiate(goldParticles, new UnityEngine.Vector3(tilemaps[3].WorldToCell(hitPosition).x + 0.5f, tilemaps[3].WorldToCell(hitPosition).y + 0.5f, 0), UnityEngine.Quaternion.identity);
            }
            else if (name.Contains("Silver"))
            {
                Instantiate(silverParticles, new UnityEngine.Vector3(tilemaps[3].WorldToCell(hitPosition).x + 0.5f, tilemaps[3].WorldToCell(hitPosition).y + 0.5f, 0), UnityEngine.Quaternion.identity);
            }
        }
    }


    private void TileDestroy(string name, Tilemap Tm, UnityEngine.Vector3 hitPosition)
    {
        string sound = null;
        if (name.Contains("Dirt"))
        {
            sound = "Dirt";
            Instantiate(dirtParticles, new UnityEngine.Vector3(Tm.WorldToCell(hitPosition).x + 0.5f, Tm.WorldToCell(hitPosition).y + 0.5f, 0), UnityEngine.Quaternion.identity);
            if (Random.Range(0, 100) < 1) Instantiate(pebble, hitPosition, UnityEngine.Quaternion.identity);
        }
        else if (name.Contains("Rock"))
        {
            sound = "Rock";
            if (Random.Range(0, 100) < 1) Instantiate(pebble, hitPosition, UnityEngine.Quaternion.identity);
            Instantiate(rockParticles, new UnityEngine.Vector3(Tm.WorldToCell(hitPosition).x + 0.5f, Tm.WorldToCell(hitPosition).y + 0.5f, 0), UnityEngine.Quaternion.identity);
        }
        
        else if (name.Contains("Snow"))
        {
            sound = "Snow";
            Instantiate(snowParticles, new UnityEngine.Vector3(Tm.WorldToCell(hitPosition).x + 0.5f, Tm.WorldToCell(hitPosition).y + 0.5f, 0), UnityEngine.Quaternion.identity);
        }
        else if (name.Contains("Ice"))
        {
            sound = "Ice";
            Instantiate(iceParticles, new UnityEngine.Vector3(Tm.WorldToCell(hitPosition).x + 0.5f, Tm.WorldToCell(hitPosition).y + 0.5f, 0), UnityEngine.Quaternion.identity);
        }
        else if (name.Contains("Wood")) 
        {
            sound = "Wood";
            Instantiate(woodParticles, new UnityEngine.Vector3(Tm.WorldToCell(hitPosition).x + 0.5f, Tm.WorldToCell(hitPosition).y + 0.5f, 0), UnityEngine.Quaternion.identity);
        }
        else{
            Debug.Log(name + " tile hasn't been configured.");
        }
        if (soundOn)
        {
            am.Play(sound, new UnityEngine.Vector3(Tm.WorldToCell(hitPosition).x + 0.5f, Tm.WorldToCell(hitPosition).y + 0.5f, 0), false);
        }
        GemDestroy(hitPosition);
        if (OneAtATime) StartCoroutine(MinePause());
    }
}
