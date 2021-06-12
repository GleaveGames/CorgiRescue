using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waves : MonoBehaviour
{
    List<GameObject> waves;

    private void Start()
    {
        waves = new List<GameObject>();
        for(int i = 0; i < transform.childCount; i++) 
        {
            waves.Add(transform.GetChild(i).GetChild(0).gameObject);
            waves[i].GetComponent<Animator>().speed += Random.Range(-0.05f, 0.05f);
            StartCoroutine(StartWave(i));
        }
    }

    private IEnumerator StartWave(int i) 
    {
        yield return new WaitForSeconds(Random.Range(0, 1f));
        waves[i].SetActive(true);
    }
}
