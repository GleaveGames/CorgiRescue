using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Mirror;

public class GameManager : NetworkBehaviour
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
    NetworkManager nm;
    public Team[] teams;
    NetworkIdentity ni;

    private void Start()
    {
        nm = FindObjectOfType<NetworkManagerIso>();
        GetTiles();
    }

    void SpawnObject(Vector3 objspawnpos, string objthing, int objteam, int objtilenumber, int objx, int objy) 
    {
        CmdSpawnObj_Server(objspawnpos, objthing, objteam, objtilenumber, objx, objy);
       /*
        GameObject building = Instantiate(nm.spawnPrefabs.Find(prefab => prefab.name == objthing), objspawnpos, Quaternion.identity);
        NetworkServer.Spawn(building);
        building.GetComponent<CharacterStats>().team = objteam;
        building.GetComponent<SpriteRenderer>().color = teams[objteam].color;
        teams[objteam].things.Add(building);
        tiles[objx, objy] = 2;
        */
    }

    [Command(requiresAuthority = false)]
    void CmdSpawnObj_Server(Vector3 objspawnpos, string objthing, int objteam, int objtilenumber, int objx, int objy)
    {
        GameObject building = Instantiate(nm.spawnPrefabs.Find(prefab => prefab.name == objthing), objspawnpos, Quaternion.identity);
        building.GetComponent<CharacterStats>().team = objteam;
        building.GetComponent<SpriteRenderer>().color = teams[objteam].color;
        teams[objteam].things.Add(building);
        tiles[objx, objy] = 2;
        NetworkServer.Spawn(building);
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
            if (thing == "SoldierCamp" || thing == "ArcherCamp" || thing == "ArcherTower" || thing == "SoldierStable" || thing == "ArcherStable")
            {
                SpawnObject(spawn, thing, team, 2, x, y);
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
