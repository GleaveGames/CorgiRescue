using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    float buffTime;
    public bool good = true;

    void Start()
    {
        buffTime = FindObjectOfType<GameController>().buffTime;
        FindObjectOfType<SoundManager>().PlayThrow();
        StartCoroutine(KillSelf());
    }

    private IEnumerator KillSelf()
    {
        yield return new WaitForSeconds(5*buffTime+0.1f);
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        if (good) FindObjectOfType<SoundManager>().PlayBuff();
        else FindObjectOfType<SoundManager>().PlayHurt();
    }

}
