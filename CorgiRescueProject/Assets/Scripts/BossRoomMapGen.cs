using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomMapGen : MonoBehaviour
{
    public GameObject part2;
    public GameObject part3;
    Coroutine coroutine;
    
    void Start()
    {
        part2.transform.parent = transform.root.Find("Node (11)");
        part3.transform.parent = transform.root.Find("Node (12)");
        StartCoroutine(Wait());
    }

    private IEnumerator Wait() 
    {
        yield return new WaitForSeconds(0.4f);
        transform.root.GetComponent<LevelGenerator>().StartCoroutine("WaitAndMerge");
    }
}
