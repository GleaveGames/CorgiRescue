using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[AddComponentMenu("")]
public class NetworkManagerIso : NetworkManager
{
    public Transform P1Spawn;
    public Transform P2Spawn;
    [SerializeField]
    Canvas canvas;

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        Transform start = P1Spawn;
        int team = 0;
        if(numPlayers == 1) 
        {
            start = P2Spawn;
            team = 1;
            canvas.transform.Find("MapSelect").gameObject.SetActive(true);
        }
        GameObject player = Instantiate(playerPrefab, start.position, start.rotation);
        player.GetComponent<PlayerInput>().team = team;
        NetworkServer.AddPlayerForConnection(conn, player);
        //i think that's just the original code below
        //base.OnServerAddPlayer(conn);
    }
}
