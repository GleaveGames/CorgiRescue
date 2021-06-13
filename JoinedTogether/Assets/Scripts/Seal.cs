using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seal : Living
{
    [SerializeField]
    GameManager gm;
    Transform target;
    float hunger = 1;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(LateStart());
    }

    private IEnumerator LateStart()
    {
        yield return new WaitForEndOfFrame();
        gm = FindObjectOfType<GameManager>();
        target = GetClosestPenguin();
        StartCoroutine(GetClosestPenguinDelay());
        yield return new WaitForSeconds(Random.Range(0, 2.5f));
        int chance = Random.Range(1, 4);
        if(chance == 1)
        {
            GetComponent<AudioSource>().Play();
        }
    }
    private IEnumerator GetClosestPenguinDelay() 
    {
        yield return new WaitForSeconds(1);
        target = GetClosestPenguin();
        StartCoroutine(GetClosestPenguinDelay());
    }

    public override void Movement() 
    {
        if(target != null) 
        {
           move = ((Vector2)target.position - (Vector2)transform.position).normalized * moveSpeed * Time.deltaTime * hunger;
        }
    }

    private Transform GetClosestPenguin() 
    {
        Transform closestPenguin = gm.penguins[0].transform;
        foreach(GameObject pen in gm.penguins) 
        {
            if(Vector2.Distance(transform.position, pen.transform.position) < Vector2.Distance(transform.position, closestPenguin.position))
            {
                closestPenguin = pen.transform;
            } 
        }
        Debug.Log("Getting closest Penguin");
        return closestPenguin;
    }
}
