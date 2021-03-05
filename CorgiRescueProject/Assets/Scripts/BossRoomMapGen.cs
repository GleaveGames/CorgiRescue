using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomMapGen : MonoBehaviour
{
    public GameObject part2;
    public GameObject part3;
    void Start()
    {
        part2.transform.parent = transform.root.Find("Node (11)");
        part3.transform.parent = transform.root.Find("Node (12)");
        transform.root.GetComponent<LevelGenerator>().StartCoroutine("WaitAndMerge");
    }
}
