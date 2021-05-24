using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using Mirror;
using UnityEngine.UI;


public class GameManager : NetworkBehaviour
{
    public int[,] tiles;
    [SerializeField]
    Tilemap tilemap;
    public int boundsX, boundsY;
    public Guild[] guilds;
    NetworkManager nm;
    public Team[] teams;
    NetworkIdentity ni;
    public PlayerInput pi;
    [SerializeField]
    GameObject ResultCanvas;
    bool GameEnded;
    public bool GameStarted;
    public GameObject P1Base;
    public GameObject P2Base;
    [SerializeField]
    Button StartButton;

    private void Start()
    {
        nm = FindObjectOfType<NetworkManagerIso>();
        GetTiles();
    }

    [Command(requiresAuthority =false)]
    public void EndGame() 
    {
        ClientEndGame();
    }

    [Command(requiresAuthority = false)]
    public void StartGame() 
    {
        GameStarted = true;
        StartGameClient();
        StartButton.gameObject.SetActive(false);
        pi.loaded = true;
    }


    [ClientRpc]
    private void StartGameClient() 
    {
        GameStarted = true;
        pi.loaded = true;
    }

    [ClientRpc]
    private void ClientEndGame()
    {
        int team = pi.team;
        int winningteam = 0;
        if (P1Base == null) winningteam = 1;
        else if (P2Base == null) winningteam = 0;
        GameObject resultcanvas = Instantiate(ResultCanvas);
        if (winningteam == team)
        {
            resultcanvas.transform.GetChild(0).GetComponent<Text>().text = "Victory";
        }
        else resultcanvas.transform.GetChild(0).GetComponent<Text>().text = "Defeat";
    }
    

    public void SpawnBuild(Vector3 touchPos, string thing, int team)
    {
        float yRuff = 0.5f * (touchPos.y / 0.815f - touchPos.x / 1.415f + boundsY - 1);
        float xRuff = touchPos.x / 1.4f + 0.5f * (touchPos.y / 0.815f - touchPos.x / 1.415f + boundsY - 1);
        int y = Mathf.RoundToInt(yRuff);
        int x = Mathf.RoundToInt(xRuff);
        Vector3Int pos = new Vector3Int(x-25, y-25, 0);
        if (tiles[x, y] == 1 && (tilemap.GetColor(pos) == teams[team].areacolor1 || tilemap.GetColor(pos) == teams[team].areacolor2 || thing == "Base"))
        {
            Vector3 spawn = transform.position;
            spawn.x = (x - y) * 1.415f;
            //need to add a little offset to get centre of cell  eg the +1 below
            spawn.y = (x + y - boundsY + 1) * 0.815f;

            //if (thing == "SoldierCamp" || thing == "ArcherCamp" || thing == "ArcherTower" || thing == "SoldierStable" || thing == "ArcherStable")
            //{
                CmdSpawnBuild_Server(spawn, thing, team, 3, x, y);
            //}
            pi.loaded = false;
            if(thing == "Base")
            {
                pi.BasePlaced();
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
        ColorSurroundingTiles(objx, objy, objteam);
        NetworkServer.Spawn(building);
        SetVariablesBuild(building, objteam, objx, objy, objtilenumber);
    }

    [ClientRpc]
    void SetVariablesBuild(GameObject spawnedObj, int team, int x, int y, int tile) 
    {
        spawnedObj.GetComponent<CharacterStats>().team = team;
        spawnedObj.GetComponent<SpriteRenderer>().color = teams[team].color;
        teams[team].things.Add(spawnedObj);
        tiles[x, y] = tile;
        ColorSurroundingTiles(x, y, team);
    }

    void ColorSurroundingTiles(int x, int y, int team) 
    {
        StartCoroutine(ColorTile(x, y, teams[team].areacolor1, team));
        StartCoroutine(ColorTile(x + 1, y, teams[team].areacolor1, team));
        StartCoroutine(ColorTile(x - 1, y, teams[team].areacolor1, team));
        StartCoroutine(ColorTile(x, y - 1, teams[team].areacolor1, team));
        StartCoroutine(ColorTile(x, y + 1, teams[team].areacolor1, team));

        StartCoroutine(ColorTile(x, y+2, teams[team].areacolor2, team));
        StartCoroutine(ColorTile(x+1, y+1, teams[team].areacolor2, team));
        StartCoroutine(ColorTile(x+2, y, teams[team].areacolor2, team));
        StartCoroutine(ColorTile(x+1, y-1, teams[team].areacolor2, team));
        StartCoroutine(ColorTile(x, y-2, teams[team].areacolor2, team));
        StartCoroutine(ColorTile(x-1, y-1, teams[team].areacolor2, team));
        StartCoroutine(ColorTile(x-2, y, teams[team].areacolor2, team));
        StartCoroutine(ColorTile(x-1, y-1, teams[team].areacolor2, team));
    }



    [ClientRpc]
    void SetVariablesTroop(GameObject spawnedObj, int team, GameObject parent) 
    {
        spawnedObj.GetComponent<CharacterStats>().team = team;
        spawnedObj.GetComponent<SpriteRenderer>().color = teams[team].color;
        spawnedObj.transform.parent = parent.transform;
        spawnedObj.transform.position = parent.transform.position;
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
        SetVariablesTroop(troopy, objteam, parent);
    }


    public void SpawnTroop(GameObject troop, GameObject spawner, Vector3 spawnpos)
    {
        //gets correct value for spawnpos
        CmdSpawnTroop_Server(spawnpos, troop.name, spawner.GetComponent<CharacterStats>().team, spawner);
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
                    if (tile.name.Contains("river") || tile.name.Contains("ice")) 
                    {
                        tiles[x, y] = 2;
                    }
                    else 
                    {
                        tiles[x, y] = 1;
                    }
                }
                else
                {
                    tiles[x, y] = 0;
                }
            }
        }
    }

    IEnumerator ColorTile(int x, int y, Color col, int team) 
    {
        Color teamCol = teams[team].color;
        Vector3Int pos = new Vector3Int(x - 25, y - 25, 0);
        tilemap.SetTileFlags(pos, TileFlags.None);
        float colortime = 0.04f;
        if (tilemap.HasTile(pos))
        {
            if (tilemap.GetColor(pos) == teams[team].areacolor1) yield break;
            Color temp = tilemap.GetColor(pos);
            float counter = 0;
            while (counter < colortime) 
            {
                temp = Color.Lerp(temp, teamCol, counter/colortime);
                tilemap.SetColor(pos, temp);
                counter += Time.deltaTime;
                yield return null;
            }
            counter = 0;
            temp = tilemap.GetColor(pos);
            while (counter < colortime) 
            {
                temp = Color.Lerp(temp, col, counter/colortime);
                tilemap.SetColor(pos, temp);
                counter += Time.deltaTime;
                yield return null;
            }
            tilemap.SetColor(pos, col);
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
}

[System.Serializable]
public class Team
{
    [SyncVar]
    public string name;
    [SyncVar]
    public Color color;
    [SyncVar]
    public Color areacolor1;  
    [SyncVar]
    public Color areacolor2;
    [SyncVar]
    public List<GameObject> things;
}

[System.Serializable]
public class Builds 
{
    [SyncVar]
    public string name;
    [SyncVar]
    public GameObject build;
    [SyncVar]
    public int cost;
    [SyncVar]
    public Sprite sprite;
}

[System.Serializable]
public class Guild 
{
    [SyncVar]
    public string name;
    [SyncVar]
    public Builds[] builds;
}