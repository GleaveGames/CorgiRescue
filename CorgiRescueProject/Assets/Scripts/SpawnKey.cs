using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnKey : MonoBehaviour
{
    void Start()
    {
        FindObjectOfType<LevelGenerator>().GenerateKey();
    }    
}
