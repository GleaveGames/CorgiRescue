using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class IceSpikeSpawner : MonoBehaviour
{
    public GameObject iceSpike;
    private bool Spawned;
    Coroutine coroutine;
    private Tilemap slippyIce;
    [SerializeField]
    private Tile ice;

    private void Start()
    {
        slippyIce = GameObject.FindGameObjectWithTag("SlippyIce").GetComponent<Tilemap>();
    }

    public void Spawn()
    {
        if (!Spawned)
        {
            Quaternion rotation = Quaternion.Euler(0,0, Random.Range(0.0f, 360.0f));
            Instantiate(iceSpike, transform.position, rotation);
            Spawned = true;
            StartCoroutine("Waitforrespawn");
            slippyIce.SetTile(slippyIce.WorldToCell(transform.position), ice);
        }
    }


    IEnumerator Waitforrespawn()
    {
        yield return new WaitForSeconds(3);
        Spawned = false; 
    }



}
