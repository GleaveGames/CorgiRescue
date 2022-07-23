using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{

    public List<Tile> Tiles;

    public int mapWidth, mapHeight;
    
    public List<GameObject> tileGO;

    public List<bool> boolList;

    List<bool>[,] tileOptions;

    float[,] entropies;

    int[,] confirmedTiles;

    int[] tileCounts;

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

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                tileOptions[x, y] = new List<bool>();
                tileOptions[x, y].AddRange(boolList);
                confirmedTiles[x, y] = -1;
            }
        }


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
        //INSTANTIATING SHIT
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                yield return new WaitForSeconds(0.01f);

                PickTile(GetEntropies());
            }
        }
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
        Debug.Log(myOptionsWeight.Count);
        tileCounts[tileChoice]++;

        GameObject newTile = Instantiate(tileGO[tileChoice], new Vector3(position.x, position.y, 0), Quaternion.identity);
        if(tileChoice == 3)
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
        Debug.Log(lowestEntropy);

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
        //Permiate the Next layer


        Permiate2(new Vector2Int(position.x, position.y + 1), new Vector2Int(position.x, position.y + 2));
        Permiate2(new Vector2Int(position.x, position.y + 1), new Vector2Int(position.x + 1, position.y + 1));
        Permiate2(new Vector2Int(position.x, position.y + 1), new Vector2Int(position.x - 1, position.y + 1));
        Permiate2(new Vector2Int(position.x + 1, position.y), new Vector2Int(position.x + 2, position.y));
        Permiate2(new Vector2Int(position.x, position.y - 1), new Vector2Int(position.x, position.y - 2));
        Permiate2(new Vector2Int(position.x, position.y + 1), new Vector2Int(position.x + 1, position.y - 1));
        Permiate2(new Vector2Int(position.x, position.y + 1), new Vector2Int(position.x - 1, position.y - 1));
        Permiate2(new Vector2Int(position.x - 1, position.y), new Vector2Int(position.x - 2, position.y));

        /*
        if (CheckValid(position, new Vector2Int(0, 2)))
        {
            //initialise fake tileOptions bool list
            List<bool> fakeOptions = new List<bool>();
            foreach (Tile t in Tiles) fakeOptions.Add(false);
            for (int i = 0; i < boolList.Count; i++)
            {
                if (tileOptions[position.x, position.y + 1][i])
                {
                    for (int j = 0; j < boolList.Count; j++) if (Tiles[i].rules[j].canConnect) fakeOptions[j] = true;
                }
            }

            //fake options is now a list of all potential tile choices but need to check if any are illegal from before
            for (int i = 0; i < Tiles.Count; i++)
            {
                if (!fakeOptions[i]) tileOptions[position.x, position.y + 2][i] = false;
            }
        }
        */

        /*
        //NEEDS TO EXTEND BEYOND ONCE ITS CHANGED THESE ONES
        for(int i = 0; i < boolList.Count; i++)
        {


        if (Mathf.Abs(tileChoice - i) > 1)
        {
            if(CheckValid(position,new Vector2Int(0,1))) tileOptions[position.x, position.y+1][i] = false;
            if (CheckValid(position, new Vector2Int(0, -1))) tileOptions[position.x, position.y-1][i] = false;
            if (CheckValid(position, new Vector2Int(1, 0))) tileOptions[position.x+1, position.y][i] = false;
            if (CheckValid(position, new Vector2Int(-1, 0))) tileOptions[position.x-1, position.y][i] = false;

        }
        if(Mathf.Abs(tileChoice - i) > 2)
        {
            if (CheckValid(position, new Vector2Int(1, -1))) tileOptions[position.x + 1, position.y - 1][i] = false;
            if (CheckValid(position, new Vector2Int(-1, -1))) tileOptions[position.x - 1, position.y - 1][i] = false;
            if (CheckValid(position, new Vector2Int(1, 1))) tileOptions[position.x + 1, position.y + 1][i] = false;
            if (CheckValid(position, new Vector2Int(-1, 1))) tileOptions[position.x - 1, position.y + 1][i] = false;

            if (CheckValid(position, new Vector2Int(2, 0))) tileOptions[position.x + 2, position.y][i] = false;
            if (CheckValid(position, new Vector2Int(-2, 0))) tileOptions[position.x - 2, position.y][i] = false;
            if (CheckValid(position, new Vector2Int(0, 2))) tileOptions[position.x, position.y+2][i] = false;
            if (CheckValid(position, new Vector2Int(0, -2))) tileOptions[position.x + 0, position.y-2][i] = false;
        }
        if (Mathf.Abs(tileChoice - i) > 3)
        {
            if (CheckValid(position, new Vector2Int(0, 3))) tileOptions[position.x, position.y + 3][i] = false;
            if (CheckValid(position, new Vector2Int(1, 2))) tileOptions[position.x + 1, position.y + 2][i] = false;
            if (CheckValid(position, new Vector2Int(2, 1))) tileOptions[position.x + 2, position.y + 1][i] = false;
            if (CheckValid(position, new Vector2Int(3, 0))) tileOptions[position.x + 3, position.y][i] = false;
            if (CheckValid(position, new Vector2Int(2, -1))) tileOptions[position.x + 2, position.y-1][i] = false;
            if (CheckValid(position, new Vector2Int(1, -2))) tileOptions[position.x + 1, position.y-2][i] = false;
            if (CheckValid(position, new Vector2Int(0, -3))) tileOptions[position.x, position.y-3][i] = false;
            if (CheckValid(position, new Vector2Int(-1, -2))) tileOptions[position.x-1, position.y-2][i] = false;
            if (CheckValid(position, new Vector2Int(-2, -1))) tileOptions[position.x-2, position.y-1][i] = false;
            if (CheckValid(position, new Vector2Int(-3, 0))) tileOptions[position.x-3, position.y][i] = false;
            if (CheckValid(position, new Vector2Int(-2, 1))) tileOptions[position.x-2, position.y+1][i] = false;
            if (CheckValid(position, new Vector2Int(-1, 2))) tileOptions[position.x-1, position.y+2][i] = false;
        }
        //4
        //5
    }
    */
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

        GameObject newTile = Instantiate(tileGO[tileChoice], new Vector3(position.x, position.y, 0), Quaternion.identity);
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
    public List<Rule> rules;

    /*
    public Tile(string name, int id)
    {
        name = name;
        id = id;
        rules = new List<Rule>(); 
    }


    public Tile()
    {

    }

    */
}

[System.Serializable]
public class Rule
{
    public string name;
    public int value;
    public bool canConnect;
}

