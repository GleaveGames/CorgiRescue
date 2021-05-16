using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[AddComponentMenu("")]
public class NetworkManagerIso : NetworkManager
{

    public Transform P1Spawn;
    public Transform P2Spawn;

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        Transform start = numPlayers == 0 ? P1Spawn : P2Spawn;
        int team = numPlayers == 0 ? 1 : 0;
        GameObject player = Instantiate(playerPrefab, start.position, start.rotation);
        player.GetComponent<PlayerInput>().team = team;
        NetworkServer.AddPlayerForConnection(conn, player);
        //base.OnServerAddPlayer(conn);
    }

    public void SpawnBuilding(Vector3 objspawnpos, string objthing, int objteam, int objtilenumber, int objx, int objy) 
    {
        return;
    }

    /*
    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        foreach(Builds b in gm.builds) 
        {
            spawnPrefabs.Add(b.build);
        }
    }
    */
}
