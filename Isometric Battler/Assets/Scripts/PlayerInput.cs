using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerInput : NetworkBehaviour
{

    GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();   
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q)) 
        {
            if (Input.GetMouseButtonUp(0))
            {
                Vector3 v3 = Input.mousePosition;
                v3.z = 10.0f;
                v3 = Camera.main.ScreenToWorldPoint(v3);
                gm.SpawnBush(v3, "SoldierCamp", 0);
            }

            if (Input.GetMouseButtonUp(1))
            {
                Vector3 v3 = Input.mousePosition;
                v3.z = 10.0f;
                v3 = Camera.main.ScreenToWorldPoint(v3);
                gm.SpawnBush(v3, "SoldierCamp", 1);
            }
        }
        else if (Input.GetKey(KeyCode.W)) 
        {
            if (Input.GetMouseButtonUp(0))
            {
                Vector3 v3 = Input.mousePosition;
                v3.z = 10.0f;
                v3 = Camera.main.ScreenToWorldPoint(v3);
                gm.SpawnBush(v3, "ArcherCamp", 0);
            }

            if (Input.GetMouseButtonUp(1))
            {
                Vector3 v3 = Input.mousePosition;
                v3.z = 10.0f;
                v3 = Camera.main.ScreenToWorldPoint(v3);
                gm.SpawnBush(v3, "ArcherCamp", 1);
            }
        }
        
        else if (Input.GetKey(KeyCode.E)) 
        {
            if (Input.GetMouseButtonUp(0))
            {
                Vector3 v3 = Input.mousePosition;
                v3.z = 10.0f;
                v3 = Camera.main.ScreenToWorldPoint(v3);
                gm.SpawnBush(v3, "ArcherTower", 0);
            }

            if (Input.GetMouseButtonUp(1))
            {
                Vector3 v3 = Input.mousePosition;
                v3.z = 10.0f;
                v3 = Camera.main.ScreenToWorldPoint(v3);
                gm.SpawnBush(v3, "ArcherTower", 1);
            }
        }
        
        else if (Input.GetKey(KeyCode.R)) 
        {
            if (Input.GetMouseButtonUp(0))
            {
                Vector3 v3 = Input.mousePosition;
                v3.z = 10.0f;
                v3 = Camera.main.ScreenToWorldPoint(v3);
                gm.SpawnBush(v3, "SoldierStable", 0);
            }

            if (Input.GetMouseButtonUp(1))
            {
                Vector3 v3 = Input.mousePosition;
                v3.z = 10.0f;
                v3 = Camera.main.ScreenToWorldPoint(v3);
                gm.SpawnBush(v3, "SoldierStable", 1);
            }
        }
        
        else if (Input.GetKey(KeyCode.T)) 
        {
            if (Input.GetMouseButtonUp(0))
            {
                Vector3 v3 = Input.mousePosition;
                v3.z = 10.0f;
                v3 = Camera.main.ScreenToWorldPoint(v3);
                gm.SpawnBush(v3, "ArcherStable", 0);
            }

            if (Input.GetMouseButtonUp(1))
            {
                Vector3 v3 = Input.mousePosition;
                v3.z = 10.0f;
                v3 = Camera.main.ScreenToWorldPoint(v3);
                gm.SpawnBush(v3, "ArcherStable", 1);
            }
        }
    }
}
