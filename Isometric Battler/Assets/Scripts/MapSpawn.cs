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
    [SerializeField]
    AnimationCurve juiceY;
    [SerializeField]
    AnimationCurve juiceX;
    [SerializeField]
    float juiceTime;
    [SerializeField]
    float juiceMultiplierY;
    [SerializeField]
    float juiceMultiplierX;

    public void SpawnMapServer(int mapChoice)
    {
        if (isServer) SpawnMap(mapChoice);
    }

    [ClientRpc]
    public void SpawnMap(int mapChoice) 
    {
        GameObject map = Instantiate(maps[mapChoice]);
        StartCoroutine(MapJuice(map));
        MapSelect.SetActive(false);
    }

    IEnumerator MapJuice(GameObject map) 
    {
        float counter = 0;
        while (counter < juiceTime) 
        {
            map.transform.position = new Vector2(juiceMultiplierX * juiceX.Evaluate(counter / juiceTime), juiceMultiplierY*juiceY.Evaluate(counter / juiceTime));
            counter += Time.deltaTime;
            yield return null;
        }
        map.transform.position = Vector2.zero;
        GetComponent<GameManager>().MapSetup(map.transform.GetChild(0).GetComponent<Tilemap>());
        TribeSelect.SetActive(true);
    }
}
