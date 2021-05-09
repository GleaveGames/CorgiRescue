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
    public Builds[] builds;

    NetworkManager nm;
    public Team[] teams;
    NetworkIdentity ni;

    private void Start()
    {
        nm = FindObjectOfType<NetworkManagerIso>();
        GetTiles();
    }
    public void SpawnBush(Vector3 touchPos, string thing, int team)
    {
        float yRuff = 0.5f * (touchPos.y / 0.815f - touchPos.x / 1.415f + boundsY - 1);
        float xRuff = touchPos.x / 1.4f + 0.5f * (touchPos.y / 0.815f - touchPos.x / 1.415f + boundsY - 1);
        int y = Mathf.RoundToInt(yRuff);
        int x = Mathf.RoundToInt(xRuff);
        if (tiles[x, y] == 1)
        {
            Vector3 spawn = transform.position;
            spawn.x = (x - y) * 1.415f;
            //need to add a little offset to get centre of cell  eg the +1 below
            spawn.y = (x + y - boundsY + 1) * 0.815f;
            if (thing == "SoldierCamp" || thing == "ArcherCamp" || thing == "ArcherTower" || thing == "SoldierStable" || thing == "ArcherStable")
            {
                CmdSpawnBuild_Server(spawn, thing, team, 2, x, y);
            }
        }
    }

    [Command(requiresAuthority = false)]
    void CmdSpawnBuild_Server(Vector3 objspawnpos, string objthing, int objteam, int objtilenumber, int objx, int objy)
    {
        GameObject building = Instantiate(nm.spawnPrefabs.Find(prefab => prefab.name == objthing), objspawnpos, Quaternion.identity);

        building.GetComponent<CharacterStats>().team = objteam;
        building.GetComponent<SpriteRenderer>().color = teams[objteam].color;
        teams[objteam].things.Add(building);
        tiles[objx, objy] = 2;

        NetworkServer.Spawn(building);

    }

    [Command(requiresAuthority = false)]
    void CmdSpawnTroop_Server(Vector3 objspawnpos, string objthing, int objteam, GameObject parent)
    {
        GameObject troopy = Instantiate(nm.spawnPrefabs.Find(prefab => prefab.name == objthing), objspawnpos, Quaternion.identity);
        troopy.GetComponent<CharacterStats>().team = objteam;
        troopy.GetComponent<SpriteRenderer>().color = teams[objteam].color;
        troopy.transform.parent = parent.transform;
        teams[objteam].things.Add(troopy);
        NetworkServer.Spawn(troopy);
    }


    public void SpawnTroop(GameObject troop, GameObject spawner, Vector3 spawnpos)
    {
        Vector3 spawn = spawnpos;
        CmdSpawnTroop_Server(spawn, troop.name, spawner.GetComponent<CharacterStats>().team, spawner);
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

[System.Serializable]
public class Builds 
{
    public string name;
    public GameObject build;
    public int cost;
    public Sprite sprite;
}