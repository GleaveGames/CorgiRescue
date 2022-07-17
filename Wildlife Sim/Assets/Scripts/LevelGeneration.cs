using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    public int mapWidth, mapHeight;
    
    public List<GameObject> tileGO;

    public List<bool> boolList;

    List<bool>[,] tileOptions;

    int[,] confirmedTiles;

    private void Start()
    {
        tileOptions = new List<bool>[mapWidth, mapHeight];
        confirmedTiles = new int[mapWidth, mapHeight];
        GenerateMap();
    }

    private float CalculateEntropy(Vector2Int position, int[,] terrainTiles) {
        float value = 0;
                
        //foreach ()

        return value;
    }

    private void PickTile(Vector2Int position) {
        //loop through bools in tileOptions and see how many are still available
        int numTrue = 0;
        int tileChoice = -10;
        List<int> myOptions = new List<int>();
        for (int i =0; i < tileOptions[position.x, position.y].Count; i++)
        {
            if (tileOptions[position.x, position.y][i]) myOptions.Add(i);
        }
        int randChoice = Random.Range(0, myOptions.Count);
        tileChoice = myOptions[randChoice];


        //SpawnTile
        GameObject newTile = Instantiate(tileGO[tileChoice], new Vector3(position.x, position.y, 0), Quaternion.identity);
        newTile.transform.parent = gameObject.transform;

        //Permiate
        Permiate(position, tileChoice);
        
    }

    void Permiate(Vector2Int position, int tileChoice)
    {
        //RULES
        if (position.x == 0 || position.x == mapWidth - 1 || position.y == 0 || position.y == mapHeight - 1) return ;
        else
        {
            for(int i = 0; i < boolList.Count; i++)
            {
                if (Mathf.Abs(tileChoice - i) > 1)
                {
                    tileOptions[position.x, position.y+1][i] = false;
                    tileOptions[position.x+1, position.y+1][i] = false;
                    tileOptions[position.x, position.y-1][i] = false;
                    tileOptions[position.x-1, position.y-1][i] = false;
                    tileOptions[position.x+1, position.y][i] = false;
                    tileOptions[position.x+1, position.y-1][i] = false;
                    tileOptions[position.x-1, position.y][i] = false;
                    tileOptions[position.x-1, position.y+1][i] = false;
                }
            }
        }
    }



    public void GenerateMap()
    {
        //INSTANTIATING SHIT

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++) {
                tileOptions[x, y] = new List<bool>();
                tileOptions[x,y].AddRange(boolList);
                confirmedTiles[x, y] = -1;
            } 
        }

        /*
        for (int i = 0; i < 1000; i++) {
            int xRand = Random.Range(0, mapWidth);
            int yRand = Random.Range(0, mapHeight);
            if (confirmedTiles[xRand, yRand] < 0) PickTile(new Vector2Int(xRand, yRand));
        } 
        */

        
        for (int x = 0; x < mapWidth; x++) {
            for (int y = 0; y < mapHeight; y++) PickTile(new Vector2Int(x, y));
        }

        //PickTile(new Vector2Int(mapWidth / 2, mapHeight / 2));
        

    }

    public void DestroyMap()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
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

/*
public class Tile {
    public GameObject go;
    public string name;

}
*/