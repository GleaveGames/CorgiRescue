using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnEnemy : MonoBehaviour
{
    [Header ("Snake")]
    [SerializeField]
    private bool snake;
    [SerializeField]
    private GameObject snakeObj;
    [Header ("Mole")]
    [SerializeField]
    private bool mole;
    [SerializeField]
    private GameObject moleObj;
    [Header ("Bat")]
    [SerializeField]
    private bool bat;
    [SerializeField]
    private GameObject batObj;
    [Header ("Mongoose")]
    [SerializeField]
    private bool mong;
    [SerializeField]
    private GameObject mongObj;

    Coroutine coroutine;


    public void StartSpawn()
    {
        transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        StartCoroutine("WaitToSpawn");
    }

    private IEnumerator WaitToSpawn()
    {
        yield return new WaitForSeconds(2);
        Spawn();
    }

    private void Spawn()
    {
        if (bat)
        {
            Instantiate(batObj, transform.position, Quaternion.identity);
        }
        else if (snake)
        {
            Instantiate(snakeObj, transform.position, Quaternion.identity);
        }
        else if (mong)
        {
            Instantiate(mongObj, transform.position, Quaternion.identity);
        }
        else if (mole)
        {
            Instantiate(moleObj, transform.position, Quaternion.identity);
        }
        transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
    }

    private IEnumerator WaitToDestroy()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
