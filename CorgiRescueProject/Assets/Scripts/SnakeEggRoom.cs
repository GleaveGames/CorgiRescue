using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeEggRoom : MonoBehaviour
{
    [SerializeField]
    private GameObject[] snakespawners;

    [SerializeField]
    private GameObject egg;

    private bool spawned = false;

    private void Update()
    {
        if (!spawned)
        {
            if (egg.transform.parent != transform)
            {
                for (int i = 0; i < snakespawners.Length; i++)
                {
                    snakespawners[i].GetComponent<spawnEnemy>().StartSpawn();
                }
                spawned = true;
            }
        }
    }
}
