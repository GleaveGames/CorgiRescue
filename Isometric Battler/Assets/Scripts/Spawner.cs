using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Spawner : NetworkBehaviour
{
    [SerializeField]
    AnimationCurve SquirtX;
    [SerializeField]
    AnimationCurve SquirtY;

    [SerializeField]
    float cooldown;
    Coroutine coroutine;
    [SerializeField]
    GameObject Troop;
    [SerializeField]
    int poplimit;
    bool spawning;
    GameManager gm;
    [SyncVar]
    Vector3 spawnpos;
    

    void Start()
    {
        if (!isServer)
        {
            this.enabled = false;
        }
        StartCoroutine(SpawnSoldier());
        spawning = true;
        gm = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if (!spawning)
        {
            if (transform.childCount <= poplimit)
            {
                StartCoroutine(SpawnSoldier());
                spawning = true;
            }
        }
    }
    private IEnumerator SpawnSoldier()
    {
        if (isServer) 
        {
            Vector3 scale = transform.localScale;
            float counter = 0;
            while (counter < cooldown)
            {

                counter += Time.deltaTime;
                yield return null;
            }
            spawnpos = transform.position;
            gm.SpawnTroop(Troop, this.gameObject, spawnpos);
            spawning = false;
        }
    }
}
