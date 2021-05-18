using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerInput : NetworkBehaviour
{
    public bool loaded;
    public bool basePlaced;
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
            gm.pi = this;
        }
        else 
        {
            this.enabled = false;
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
        else if (!basePlaced)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 v3 = Input.mousePosition;
                v3.z = 10.0f;
                v3 = Camera.main.ScreenToWorldPoint(v3);
                gm.SpawnBush(v3, "Base", team);
            }
        }
    }

    public void BasePlaced() 
    {
        if (isLocalPlayer) 
        {
            Instantiate(canvas);
            FindObjectOfType<BuildButtons>().pi = this;
            basePlaced = true;
        }
    }
}