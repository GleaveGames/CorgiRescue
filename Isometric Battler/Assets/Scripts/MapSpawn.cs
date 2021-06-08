using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Tilemaps;

public class MapSpawn : NetworkBehaviour
{
    public List<GameObject> maps;
    [SerializeField]
    GameObject TribeSelect;
    [SerializeField]
    GameObject MapSelect;

    public void SpawnMapServer(int mapChoice)
    {
        if (isServer) SpawnMap(mapChoice);
    }

    [ClientRpc]
    public void SpawnMap(int mapChoice) 
    {
        GameObject map = Instantiate(maps[mapChoice]);
        GetComponent<GameManager>().MapSetup(map.transform.GetChild(0).GetComponent<Tilemap>());
        TribeSelect.SetActive(true);
        MapSelect.SetActive(false);
    }
}
