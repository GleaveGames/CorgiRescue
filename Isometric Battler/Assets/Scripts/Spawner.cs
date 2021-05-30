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
    public List<GameObject> childSoldiers;
    

    void Start()
    {
        if (!isServer)
        {
            this.enabled = false;
        }
        StartCoroutine(SpawnSoldier());
        spawning = true;
        gm = FindObjectOfType<GameManager>();
        StartCoroutine(CheckChildSoldiers());
    }

    private void Update()
    {
        if (!spawning)
        {
            if (childSoldiers.Count <= poplimit)
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
            Vector3 scaleinit = transform.localScale;
            float counter = 0;
            while (counter < cooldown)
            {
                scale.x = SquirtX.Evaluate(counter / cooldown);
                scale.y = SquirtY.Evaluate(counter / cooldown);
                transform.localScale = scale;
                counter += Time.deltaTime;
                yield return null;
            }
            spawnpos = transform.position;
            gm.SpawnTroop(Troop, this.gameObject, spawnpos);
            spawning = false;
            transform.localScale = scaleinit;
        }
    }

    private IEnumerator CheckChildSoldiers()
    {
        yield return new WaitForSeconds(1);
        for (int i = 0; i < childSoldiers.Count; i++) 
        {
            if(childSoldiers[i] == null) 
            {
                childSoldiers.RemoveAt(i);
            }
        }
        StartCoroutine(CheckChildSoldiers());
    }
}
