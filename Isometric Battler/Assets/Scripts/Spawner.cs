using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    float cooldown;
    [SerializeField]
    GameObject Cooldown;
    Coroutine coroutine;
    [SerializeField]
    GameObject Troop;
    [SerializeField]
    int poplimit;
    bool spawning;
    GameObject fill;
    GameObject cooldownObj;
    GameManager gm;


    void Start()
    {
        cooldownObj = Instantiate(Cooldown, transform.position, Quaternion.identity);
        cooldownObj.transform.parent = transform;
        fill = cooldownObj.transform.GetChild(0).gameObject;
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
                cooldownObj.SetActive(true);
                StartCoroutine(SpawnSoldier());
                spawning = true;
            }
            else 
            {
                cooldownObj.SetActive(false);
            }
        }
    }


    private IEnumerator SpawnSoldier()
    {
        float timer = 0;
        Vector3 scale = fill.transform.localScale;
        while(timer < cooldown) 
        {
            timer += Time.deltaTime;
            scale.x = 2.2f * timer / cooldown;
            fill.transform.localScale = scale;
            yield return null;
        }
        GameObject troop = Instantiate(Troop, transform.position, Quaternion.identity);
        troop.GetComponent<CharacterStats>().team = GetComponent<CharacterStats>().team;
        gm.teams[GetComponent<CharacterStats>().team].things.Add(troop);
        troop.GetComponent<Transform>().parent = transform;
        troop.GetComponent<SpriteRenderer>().color = gm.teams[GetComponent<CharacterStats>().team].color;
        spawning = false;
        scale.x = 0;
        fill.transform.localScale = scale;
    }
}
