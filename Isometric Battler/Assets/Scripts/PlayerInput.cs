using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerInput : NetworkBehaviour
{
    public bool loaded;
    public GameObject build;
    GameManager gm;
    [SerializeField]
    Canvas canvas;
    public int team;


    // Start is called before the first frame update
    void Start()
    {
        if (isLocalPlayer) 
        {
            gm = FindObjectOfType<GameManager>();
            Instantiate(canvas);
            FindObjectOfType<BuildButtons>().pi = this;
            gm.pi = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) return;
        else if (loaded) 
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 v3 = Input.mousePosition;
                v3.z = 10.0f;
                v3 = Camera.main.ScreenToWorldPoint(v3);
                gm.SpawnBush(v3, build.name, team);
            }
        }

        /*
        if (!isLocalPlayer) return;

        else if (Input.GetKey(KeyCode.Q)) 
        {
            if (Input.GetMouseButtonUp(0))
            {
                Vector3 v3 = Input.mousePosition;
                v3.z = 10.0f;
                v3 = Camera.main.ScreenToWorldPoint(v3);
                gm.SpawnBush(v3, "SoldierCamp", 0);
            }

            else if (Input.GetMouseButtonUp(1))
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

            else if (Input.GetMouseButtonUp(1))
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

            else if (Input.GetMouseButtonUp(1))
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

            else if (Input.GetMouseButtonUp(1))
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

            else if (Input.GetMouseButtonUp(1))
            {
                Vector3 v3 = Input.mousePosition;
                v3.z = 10.0f;
                v3 = Camera.main.ScreenToWorldPoint(v3);
                gm.SpawnBush(v3, "ArcherStable", 1);
            }
        }
        */


    }
}
