using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomThing : MonoBehaviour
{
    [SerializeField]
    public Thing[] things;
    private float totalChance;
    private float choice;
    void Start()
    {
        for (int i = 0; i < things.Length; i++)
        {
            totalChance += things[i].chance;
        }
        choice = Random.Range(0, totalChance);
        totalChance = 0;
        for(int i = 0; i < things.Length; i++)
        {
            totalChance += things[i].chance;
            if(choice < totalChance)
            {
                if (things[i].thisObj != null)
                {
                    Instantiate(things[i].thisObj, transform.position, Quaternion.identity);
                }
                Destroy(gameObject);
                return;
            }
        }
    }
}
[System.Serializable]
public class Thing 
{
    public GameObject thisObj;
    public float chance;
}

