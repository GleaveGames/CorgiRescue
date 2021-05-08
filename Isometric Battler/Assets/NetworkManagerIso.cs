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
        GameObject player = Instantiate(playerPrefab, start.position, start.rotation);
        NetworkServer.AddPlayerForConnection(conn, player);
        //base.OnServerAddPlayer(conn);
    }

    public void SpawnBuilding(Vector3 objspawnpos, string objthing, int objteam, int objtilenumber, int objx, int objy) 
    {
        return;
    
    }
    
}
