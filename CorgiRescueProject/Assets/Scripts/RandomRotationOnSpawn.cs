using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotationOnSpawn : MonoBehaviour
{
    private void Start()
    {
        Quaternion randomRot = transform.rotation;
        randomRot.z = Random.Range(0, 360);
        transform.rotation = randomRot;
    }
}
