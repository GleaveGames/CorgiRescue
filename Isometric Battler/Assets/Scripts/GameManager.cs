using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public int[,] tiles;
    [SerializeField]
    Tilemap tilemap;
    public int boundsX, boundsY;
    [SerializeField]
    GameObject soldiercamp;
    [SerializeField]
    GameObject archercamp;
    [SerializeField]
    GameObject archertower;
    [SerializeField]
    GameObject soldierstable;
    [SerializeField]
    GameObject archerstable;

    public Team[] teams;

    private void Start()
    {
        GetTiles();
    }

    public void SpawnBush(Vector3 touchPos, string thing, int team)
    {
        float yRuff = 0.5f * (touchPos.y / 0.815f - touchPos.x/1.415f + boundsY -1);
        float xRuff = touchPos.x / 1.4f + 0.5f * (touchPos.y / 0.815f - touchPos.x / 1.415f + boundsY - 1);
        int y = Mathf.RoundToInt(yRuff);
        int x = Mathf.RoundToInt(xRuff);
        if(tiles[x,y] == 1) 
        {
            Vector3 spawn = transform.position;
            spawn.x = (x - y) * 1.415f;
            //need to add a little offset to get centre of cell  eg the +1 below
            spawn.y = (x + y - boundsY + 1) * 0.815f;
            if (thing == "SoldierCamp")
            {
                GameObject building = Instantiate(soldiercamp, spawn, Quaternion.identity);
                building.GetComponent<CharacterStats>().team = team;
                building.GetComponent<SpriteRenderer>().color = teams[team].color;
                teams[team].things.Add(building);
                tiles[x, y] = 2;
            }
            else if (thing == "ArcherCamp")
            {
                GameObject building = Instantiate(archercamp, spawn, Quaternion.identity);
                building.GetComponent<CharacterStats>().team = team;
                building.GetComponent<SpriteRenderer>().color = teams[team].color;
                teams[team].things.Add(building);
                tiles[x, y] = 2;
            }
            else if (thing == "ArcherTower") 
            {
                GameObject tower = Instantiate(archertower, spawn, Quaternion.identity);
                tower.GetComponent<CharacterStats>().team = team;
                tower.GetComponent<SpriteRenderer>().color = teams[team].color;
                teams[team].things.Add(tower);
                tiles[x, y] = 3;
            }
            else if (thing == "SoldierStable") 
            {
                GameObject tower = Instantiate(soldierstable, spawn, Quaternion.identity);
                tower.GetComponent<CharacterStats>().team = team;
                tower.GetComponent<SpriteRenderer>().color = teams[team].color;
                teams[team].things.Add(tower);
                tiles[x, y] = 3;
            }
            else if (thing == "ArcherStable") 
            {
                GameObject tower = Instantiate(archerstable, spawn, Quaternion.identity);
                tower.GetComponent<CharacterStats>().team = team;
                tower.GetComponent<SpriteRenderer>().color = teams[team].color;
                teams[team].things.Add(tower);
                tiles[x, y] = 3;
            }
        }
    }

    void GetTiles()
    {
        boundsX = tilemap.cellBounds.size.x;
        boundsY = tilemap.cellBounds.size.y;
        tiles = new int[boundsX, boundsY];
        TileBase[] allTiles = tilemap.GetTilesBlock(tilemap.cellBounds);
        for (int x = 0; x < boundsX; x++) 
        {
            for(int y = 0; y < boundsY; y++)
            {
                TileBase tile = allTiles[x + y * boundsX];
                if (tile!=null)
                {
                    if (tile.name == "grass")
                    {
                        tiles[x, y] = 1;
                    }
                    else
                    {
                        tiles[x, y] = 0;
                    }
                }
                else
                {
                    tiles[x, y] = 0;
                }
            }
        }
    }


    public void PrintMap()
    {
        Debug.Log("Pressed");
        for(int x = 0; x < boundsX; x++) 
        {
            string row = "";
            for(int y = 0; y < boundsY; y++) 
            {
                row += tiles[x, y].ToString();
            }
            Debug.Log(row);
        }
    }

    public void StopTime() 
    {
        Time.timeScale = 0.001f;
    }

    public void HalfTime() 
    {
        Time.timeScale = 0.5f;
    }

    public void NormalTime() 
    {
        Time.timeScale = 1;
    }

    public void DoubleTime() 
    {
        Time.timeScale = 2;
    }

    public void FiveTime() 
    {
        Time.timeScale = 5;
    }
}

[System.Serializable]
public class Team
{
    public string name;
    public Color color;
    public List<GameObject> things;
}
