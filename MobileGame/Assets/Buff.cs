using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    float buffTime;
    void Start()
    {
        buffTime = FindObjectOfType<GameController>().buffTime;
        StartCoroutine(KillSelf());
    }

    private IEnumerator KillSelf()
    {
        yield return new WaitForSeconds(buffTime+0.1f);
        Destroy(gameObject);
    }
}
