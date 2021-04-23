using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject Gem;
    [SerializeField]
    bool predefined = false;
    [SerializeField]
    int predefinedtype;
    [SerializeField]
    int predefinedLow;
    [SerializeField]
    int predefinedHigh;

    [SerializeField]
    bool wood;
    [SerializeField]
    bool metal;
    

    private void Start()
    {
        if (predefined) 
        {
            SpawnGems(predefinedtype, Random.Range(predefinedLow, predefinedHigh));
        }
        else 
        {
            Gemsplosion();
        }
    }

    public void SpawnGems(int GemType, int NumberOfGems) 
    {
        Transform Player = GameObject.FindGameObjectWithTag("Player").transform;
        for(int i = 0; i < NumberOfGems; i++) 
        {
            //Spawn pos and rotation not determined;
            //Want them to spread out too;
            Vector2 SpawnPos = transform.position;
            SpawnPos.x += Random.Range(-0.4f, 0.4f);
            SpawnPos.y += Random.Range(-0.4f, 0.4f);
            GameObject gem = Instantiate(Gem, SpawnPos, Quaternion.identity);
            gem.GetComponent<Rigidbody2D>().AddForce((SpawnPos - new Vector2(transform.position.x,transform.position.y))*25);
            gem.transform.parent = transform;
            gem.GetComponent<GemObj>().Player = Player;
            if (GemType == 1) 
            {
                gem.GetComponent<GemObj>().Type(1);
            }
            if(GemType == 2) 
            {
                gem.GetComponent<GemObj>().Type(2);
            }
            if (GemType == 3) 
            {
                gem.GetComponent<GemObj>().Type(3);
            }
        }
    }

    public void Gemsplosion()
    {
        Transform Player = GameObject.FindGameObjectWithTag("Player").transform;
        int numgems = Random.Range(1, 20);
        if (metal) 
        {
            for (int i = 0; i <= numgems; i++)
            {
                Vector2 SpawnPos = transform.position;
                SpawnPos.x += Random.Range(-0.4f, 0.4f);
                SpawnPos.y += Random.Range(-0.4f, 0.4f);
                GameObject gem = Instantiate(Gem, SpawnPos, Quaternion.identity);
                gem.GetComponent<Rigidbody2D>().AddForce((SpawnPos - new Vector2(transform.position.x, transform.position.y)) * 25);
                gem.transform.parent = transform;
                gem.GetComponent<GemObj>().Player = Player;
                gem.GetComponent<GemObj>().Type(Random.Range(1, 4));
            }
        }
        else if (wood)
        {
            for (int i = 0; i <= numgems/2; i++)
            {
                Vector2 SpawnPos = transform.position;
                SpawnPos.x += Random.Range(-0.4f, 0.4f);
                SpawnPos.y += Random.Range(-0.4f, 0.4f);
                GameObject gem = Instantiate(Gem, SpawnPos, Quaternion.identity);
                gem.GetComponent<Rigidbody2D>().AddForce((SpawnPos - new Vector2(transform.position.x, transform.position.y)) * 25);
                gem.transform.parent = transform;
                gem.GetComponent<GemObj>().Player = Player;
                gem.GetComponent<GemObj>().Type(Random.Range(1, 4));
            }
        }
        else Debug.Log("Undefined gemsplosion");
    }
}
