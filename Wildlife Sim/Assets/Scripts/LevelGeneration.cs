using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    [SerializeField]
    [Range (0,0.05f)]
    float spawnSpeed;

    public List<Tile> Tiles;

    public int mapWidth, mapHeight;
    
    public GameObject tileGO;

    public List<bool> boolList;

    List<bool>[,] tileOptions;

    float[,] entropies;

    public int[,] confirmedTiles;

    public GameObject[,] tileObjects;

    int[] tileCounts;

    [SerializeField]
    bool IslandMode;

    public bool mapLoaded;

    private void Start()
    {
        StartCoroutine(GenerateMap());
    }

    public IEnumerator GenerateMap()
    {
        yield return new WaitForSeconds(0.02f);

        tileOptions = new List<bool>[mapWidth, mapHeight];
        confirmedTiles = new int[mapWidth, mapHeight];
        entropies = new float[mapWidth, mapHeight];
        tileCounts = new int[boolList.Count];
        tileObjects = new GameObject[mapWidth, mapHeight];

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                tileOptions[x, y] = new List<bool>();
                tileOptions[x, y].AddRange(boolList);
                confirmedTiles[x, y] = -1;
            }
        }

        if (IslandMode)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                SetTile(new Vector2Int(x, 0), 0);
                SetTile(new Vector2Int(x, 1), 0);
                SetTile(new Vector2Int(x, mapHeight - 1), 0);
                SetTile(new Vector2Int(x, mapHeight - 2), 0);
            }
            for (int y = 2; y < mapHeight - 2; y++)
            {
                SetTile(new Vector2Int(0, y), 0);
                SetTile(new Vector2Int(1, y), 0);
                SetTile(new Vector2Int(mapWidth - 1, y), 0);
                SetTile(new Vector2Int(mapWidth - 2, y), 0);
            }
        }

        //INSTANTIATING SHIT
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                yield return new WaitForSeconds(spawnSpeed);

                PickTile(GetEntropies());
            }
        }

        mapLoaded = true;
    }


    private void PickTile(Vector2Int position) {
        //loop through bools in tileOptions and see how many are still available
        int tileChoice = -10;
        List<int> myOptions = new List<int>();
        List<float> myOptionsWeight = new List<float>();
        float sumWeights = 0;
        for (int i =0; i < tileOptions[position.x, position.y].Count; i++)
        {
            if (tileOptions[position.x, position.y][i])
            {
                myOptions.Add(i);
                myOptionsWeight.Add(1f / (tileCounts[i]+1));
                sumWeights += 1f / (tileCounts[i]+1);
            }

        }
        float randChoice = Random.Range(0, sumWeights);
        sumWeights = 0;
        for(int i = 0; i < myOptionsWeight.Count; i++)
        {
            sumWeights += myOptionsWeight[i];
            if (randChoice <= sumWeights) {
                tileChoice = myOptions[i];
                break;
            }
        }
        confirmedTiles[position.x,position.y] = tileChoice;
        tileCounts[tileChoice]++;

        GameObject newTile = Instantiate(tileGO, new Vector3(position.x, position.y, 0), Quaternion.identity);
        Tile tileType = Tiles[tileChoice];
        newTile.name = tileType.name;
        newTile.GetComponent<SpriteRenderer>().sprite = tileType.sprites[Random.Range(0, tileType.sprites.Length)];
        newTile.GetComponent<SpriteRenderer>().sortingOrder = tileType.layer;
        tileObjects[position.x, position.y] = newTile;

        if (tileType.child)
        {
            GameObject newTileChild = Instantiate(tileGO, new Vector3(position.x, position.y, 0), Quaternion.identity);
            Vector3 randOffset = new Vector2(Random.Range(-0.1f,0.1f), Random.Range(-0.1f, 0.1f));
            newTileChild.transform.position += randOffset;
            newTileChild.name = tileType.name;
            newTileChild.transform.parent = newTile.transform;
            newTileChild.GetComponent<SpriteRenderer>().sprite = tileType.childSprites[Random.Range(0, tileType.childSprites.Length)];
            newTileChild.GetComponent<SpriteRenderer>().sortingOrder = tileType.childLayer;
            tileObjects[position.x, position.y] = newTileChild;

        }


        //random rotation
        if (tileChoice == 2)
        {
            int choice = Random.Range(0, 4);
            if (choice == 1) newTile.transform.Rotate(new Vector3(0,0,90));
            else if (choice == 2) newTile.transform.Rotate(new Vector3(0, 0, 180));
            else if (choice == 3) newTile.transform.Rotate(new Vector3(0, 0, 270));
        }
        newTile.transform.parent = gameObject.transform;

        //Permiate
        Permiate(position, tileChoice);
        
    }

    private Vector2Int GetEntropies() 
    {
        Vector2Int lowestEntropyPosition = new Vector2Int(0, 0);
        float lowestEntropy = 99999999f;
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                if (confirmedTiles[x, y] == -1)
                {
                    float sumWeights = 0;
                    for (int i = 0; i < tileOptions[x, y].Count; i++)
                    {
                        if (tileOptions[x, y][i])
                        {
                            sumWeights += (tileCounts[i]+1f)/(mapWidth*mapHeight+1);
                        }
                    }
                    entropies[x, y] = sumWeights;
                    if(lowestEntropy > sumWeights)
                    {
                        lowestEntropy = sumWeights;
                        lowestEntropyPosition = new Vector2Int(x, y);
                    }
                    if (entropies[x, y] == 0) Debug.Log("Simulation failed.");

                }
            }
        }

        return lowestEntropyPosition;

    }

    void Permiate2(Vector2Int permiator, Vector2Int permiated)
    {
        if (CheckValid2(permiated) && CheckValid2(permiator))
        {
            //initialise fake tileOptions bool list
            List<bool> fakeOptions = new List<bool>();
            foreach (Tile t in Tiles) fakeOptions.Add(false);
            for (int i = 0; i < boolList.Count; i++)
            {
                if (tileOptions[permiator.x, permiator.y][i])
                {
                    for (int j = 0; j < boolList.Count; j++) if (Tiles[i].rules[j].canConnect) fakeOptions[j] = true;
                }
            }

            //fake options is now a list of all potential tile choices but need to check if any are illegal from before
            for (int i = 0; i < Tiles.Count; i++)
            {
                if (!fakeOptions[i]) tileOptions[permiated.x,permiated.y][i] = false;
            }
        }
    }

    void Permiate(Vector2Int position, int tileChoice)
    {
        //RULES

        //Permiate Adjacents
        if (CheckValid(position, new Vector2Int(0, 1))) for (int i = 0; i < boolList.Count; i++) if (!Tiles[tileChoice].rules[i].canConnect) tileOptions[position.x, position.y + 1][i] = false;
        if (CheckValid(position, new Vector2Int(0, -1))) for (int i = 0; i < boolList.Count; i++) if (!Tiles[tileChoice].rules[i].canConnect) tileOptions[position.x, position.y - 1][i] = false;
        if (CheckValid(position, new Vector2Int(1, 0))) for (int i = 0; i < boolList.Count; i++) if (!Tiles[tileChoice].rules[i].canConnect) tileOptions[position.x + 1, position.y][i] = false;
        if (CheckValid(position, new Vector2Int(-1, 0))) for (int i = 0; i < boolList.Count; i++) if (!Tiles[tileChoice].rules[i].canConnect) tileOptions[position.x - 1, position.y][i] = false;


        //Reverse Sytem, start false and add true
        //Permiate the Next layer 1
        Permiate2(new Vector2Int(position.x, position.y + 1), new Vector2Int(position.x, position.y + 2));
        Permiate2(new Vector2Int(position.x, position.y + 1), new Vector2Int(position.x + 1, position.y + 1));
        Permiate2(new Vector2Int(position.x, position.y + 1), new Vector2Int(position.x - 1, position.y + 1));
        Permiate2(new Vector2Int(position.x + 1, position.y), new Vector2Int(position.x + 2, position.y));
        Permiate2(new Vector2Int(position.x, position.y - 1), new Vector2Int(position.x, position.y - 2));
        Permiate2(new Vector2Int(position.x, position.y - 1), new Vector2Int(position.x + 1, position.y - 1));
        Permiate2(new Vector2Int(position.x, position.y - 1), new Vector2Int(position.x - 1, position.y - 1));
        Permiate2(new Vector2Int(position.x - 1, position.y), new Vector2Int(position.x - 2, position.y));

        //Permiate the Next layer 2
        Permiate2(new Vector2Int(position.x, position.y + 2), new Vector2Int(position.x, position.y + 3));
        Permiate2(new Vector2Int(position.x, position.y + 2), new Vector2Int(position.x-1, position.y + 2));
        Permiate2(new Vector2Int(position.x, position.y + 2), new Vector2Int(position.x+1, position.y + 2));

        Permiate2(new Vector2Int(position.x, position.y - 2), new Vector2Int(position.x, position.y - 3));
        Permiate2(new Vector2Int(position.x, position.y - 2), new Vector2Int(position.x - 1, position.y - 2));
        Permiate2(new Vector2Int(position.x, position.y - 2), new Vector2Int(position.x + 1, position.y - 2));

        Permiate2(new Vector2Int(position.x + 2, position.y), new Vector2Int(position.x + 3, position.y));
        Permiate2(new Vector2Int(position.x + 2, position.y), new Vector2Int(position.x + 2, position.y+1));
        Permiate2(new Vector2Int(position.x + 2, position.y), new Vector2Int(position.x + 2, position.y-1));

        Permiate2(new Vector2Int(position.x - 2, position.y), new Vector2Int(position.x - 3, position.y));
        Permiate2(new Vector2Int(position.x - 2, position.y), new Vector2Int(position.x - 2, position.y + 1));
        Permiate2(new Vector2Int(position.x - 2, position.y), new Vector2Int(position.x - 2, position.y - 1));

        
        //Permiate the Next layer 3
        Permiate2(new Vector2Int(position.x, position.y + 3), new Vector2Int(position.x, position.y + 4));
        Permiate2(new Vector2Int(position.x, position.y + 3), new Vector2Int(position.x - 1, position.y + 3));
        Permiate2(new Vector2Int(position.x, position.y + 3), new Vector2Int(position.x + 1, position.y + 3));

        Permiate2(new Vector2Int(position.x, position.y - 3), new Vector2Int(position.x, position.y - 4));
        Permiate2(new Vector2Int(position.x, position.y - 3), new Vector2Int(position.x - 1, position.y - 3));
        Permiate2(new Vector2Int(position.x, position.y - 3), new Vector2Int(position.x + 1, position.y - 3));

        Permiate2(new Vector2Int(position.x + 3, position.y), new Vector2Int(position.x + 4, position.y));
        Permiate2(new Vector2Int(position.x + 3, position.y), new Vector2Int(position.x + 3, position.y + 1));
        Permiate2(new Vector2Int(position.x + 3, position.y), new Vector2Int(position.x + 3, position.y - 1));

        Permiate2(new Vector2Int(position.x - 3, position.y), new Vector2Int(position.x - 4, position.y));
        Permiate2(new Vector2Int(position.x - 3, position.y), new Vector2Int(position.x - 3, position.y + 1));
        Permiate2(new Vector2Int(position.x - 3, position.y), new Vector2Int(position.x - 3, position.y - 1));

        Permiate2(new Vector2Int(position.x - 1, position.y+2), new Vector2Int(position.x - 2, position.y + 2));
        Permiate2(new Vector2Int(position.x + 1, position.y+2), new Vector2Int(position.x + 2, position.y + 2));
        Permiate2(new Vector2Int(position.x + 1, position.y-2), new Vector2Int(position.x + 2, position.y - 2));
        Permiate2(new Vector2Int(position.x + 1, position.y-2), new Vector2Int(position.x + 2, position.y - 2));


    }
    bool CheckValid(Vector2Int pos, Vector2Int change)
    {
        bool valid = true;
        if ((pos + change).x < 0 || (pos+change).x >= mapWidth || (pos+change).y < 0 || (pos+change).y >= mapHeight) valid = false;
        return valid;
    }
    
    bool CheckValid2(Vector2Int pos)
    {
        bool valid = true;
        if (pos.x < 0 || pos.x >= mapWidth || pos.y < 0 || pos.y >= mapHeight) valid = false;
        return valid;
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
            StartCoroutine(GenerateMap());
            Debug.Log("gen map");
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            DestroyMap();
            Debug.Log("des map");
        }
    }




    private void SetTile(Vector2Int position, int tileChoice)
    {

        confirmedTiles[position.x, position.y] = tileChoice;
        tileCounts[tileChoice]++;

        GameObject newTile = Instantiate(tileGO, new Vector3(position.x, position.y, 0), Quaternion.identity);
        if (tileChoice == 3)
        {
            int choice = Random.Range(0, 4);
            if (choice == 1) newTile.transform.Rotate(new Vector3(0, 0, 90));
            else if (choice == 2) newTile.transform.Rotate(new Vector3(0, 0, 180));
            else if (choice == 3) newTile.transform.Rotate(new Vector3(0, 0, 270));
        }
        newTile.transform.parent = gameObject.transform;

        //Permiate
        Permiate(position, tileChoice);
    }



}

[System.Serializable]
public class Tile
{
    public string name;
    public int value;
    public Sprite[] sprites;
    public int layer;
    public List<Rule> rules;
    public bool child;
    public Sprite[] childSprites;
    public int childLayer;
    
}

[System.Serializable]
public class Rule
{
    public string name;
    public int value;
    public bool canConnect;
}

