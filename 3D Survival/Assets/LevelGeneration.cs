using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    public int mapWidth, mapHeight;
    public float noiseScale;
    [Range(1,5)]
    public int octaves;
    [Range(0, 0.9f)]
    public float persistance;
    [Range(1,10)]
    public float lacunarity;
    [Range(0,100)]
    public float grassSpawnChance;



    public GameObject block;
    [Range(0,1)]
    public float minBlockValue;
    [Range(0,1)]
    public float minGrassValue;
    [Range(0,1)]
    public float minRockValue;
    [Range(0,1)]
    public float minSnowValue;
    public Material sand;
    public Material grass;
    public Material rock;
    public Material snow;
    public int heightMax;
    public int[,] heightMap;

    public GameObject[] grassSpawns;

    private void Start()
    {
        GenerateMap();
    }



    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth,mapHeight,noiseScale, octaves,persistance,lacunarity);
        int[,] intMap = new int[mapWidth, mapHeight];
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                int yHeight = Mathf.FloorToInt(noiseMap[x, y] * heightMax - minBlockValue * heightMax);
                intMap[x, y] = yHeight;
            }
        }
        for (int y = 0; y < mapHeight; y++) 
        {
            for(int x = 0; x < mapWidth; x++)
            {
                Vector3 pos = transform.position;
                pos.x += block.transform.localScale.x*((-mapWidth / 2) + x);
                pos.z += block.transform.localScale.z*((-mapHeight / 2) + y);
                pos.y += intMap[x,y] * block.transform.localScale.y;
                Vector3 spawnPos = pos;
                spawnPos.y += block.transform.localScale.y / 2;
                if(noiseMap[x,y] > minBlockValue)
                {
                    GameObject newblock = Instantiate(block, pos, Quaternion.identity);
                    newblock.transform.parent = transform;
                    int numBelow = CheckBelows(intMap, x, y);
                    
                    if(noiseMap[x,y] > minSnowValue) 
                    {
                        newblock.GetComponent<Renderer>().material = snow;
                    }
                    else if(noiseMap[x,y] > minRockValue) 
                    {
                        newblock.GetComponent<Renderer>().material = rock;
                    }
                    else if (noiseMap[x, y] > minGrassValue)
                    {
                        newblock.GetComponent<Renderer>().material = grass;
                        float chance = Random.Range(0, 100);
                        if (chance <= grassSpawnChance)
                        {
                            GameObject newGO = Instantiate(grassSpawns[Random.Range(0, grassSpawns.Length)], spawnPos, Quaternion.identity);
                            newGO.transform.parent = transform;
                            if (newGO.CompareTag("Creature"))
                            {
                                newGO.GetComponent<Creature>().pos.x = x; 
                                newGO.GetComponent<Creature>().pos.y = y;
                                newGO.GetComponent<Creature>().lg = this;
                            }
                        }
                    }
                    else
                    {
                        newblock.GetComponent<Renderer>().material = sand;
                    }

                    if (numBelow > 0)
                    {
                        for (int i = 0; i < numBelow; i++)
                        {
                            pos.y -= block.transform.localScale.y * (i + 1);
                            GameObject blockBelow = Instantiate(block, pos, Quaternion.identity);
                            blockBelow.GetComponent<Renderer>().material = rock;
                            blockBelow.transform.parent = transform;
                        }
                    }
                }
            }
        }
        heightMap = intMap;
    }

    private int CheckBelows(int[,] intMap, int x, int y)
    {
        int numBelow = 0;
        int myHeight = intMap[x, y];
        //left
        if(x > 0) numBelow = myHeight - intMap[x-1,y];
        //right
        if (x < mapWidth - 1) if (numBelow < myHeight - intMap[x + 1, y]) numBelow = myHeight - intMap[x + 1, y];
        //down
        if(y > 0) if (numBelow < myHeight - intMap[x, y-1]) numBelow = myHeight - intMap[x, y-1];
        //up
        if (y < mapHeight - 1) if (numBelow < myHeight - intMap[x, y + 1]) numBelow = myHeight - intMap[x, y + 1];
        return numBelow;
    }

    public void DestroyMap() 
    {
        for(int i = transform.childCount - 1; i >= 0 ; i--) 
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) 
        {
            GenerateMap();
            Debug.Log("gen map");
        }
        if (Input.GetKeyDown(KeyCode.L)) 
        {
            DestroyMap();
            Debug.Log("des map");
        }
    }
}
