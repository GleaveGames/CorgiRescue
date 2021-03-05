using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathDrop : MonoBehaviour
{
    [SerializeField]
    private DropItem[] drops;

    private void OnDestroy()
    {
        float chanceTotal = 0;
        float currentChance = 0;
        for (int i = 0; i < drops.Length; i++)
        {
            chanceTotal += drops[i].chance;
        }
        float DropChoice = Random.Range(0, chanceTotal);
        for(int i = 0; i < drops.Length; i++)
        {
            currentChance += drops[i].chance;
            if(DropChoice < currentChance)
            {
                Instantiate(drops[i].obj, transform.position, Quaternion.identity);
                return;
            }           
        }
    }
}

[System.Serializable]
public class DropItem
{
    public GameObject obj;
    public float chance;
}

