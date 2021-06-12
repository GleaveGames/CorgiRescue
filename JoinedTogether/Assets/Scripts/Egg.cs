using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    public bool dead;
    [SerializeField]
    Color grey;
    [SerializeField]
    Color white;
    [SerializeField]
    float DeathTime;

    private void Start()
    {
        StartCoroutine(DeathTimer());
    }

    private IEnumerator DeathTimer() 
    {
        float counter = 0f;
        while(counter < DeathTime) 
        {
            GetComponent<SpriteRenderer>().color = Color.Lerp(white, grey, counter / DeathTime);
            counter += Time.deltaTime;
            yield return null;
        }
        dead = true;
    }
}
